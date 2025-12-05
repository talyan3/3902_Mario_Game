using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonogameTest.Sounds;
using MonogameTest.Screens;
using MonogameTest.Managers;

namespace MonogameTest.Managers
{
    public class CollisionManager
    {
        private SmallMarioSprite smallMario;
        private BigMarioSprite bigMario;
        private StaticSprite currentMario;

        private List<Tile> mapTiles;
        private HashSet<Tile> usedQuestionBlocks = new HashSet<Tile>();

        private EnemyManager enemyManager;
        private PowerupFieldManager powerupManager;
        private ScreenManager screenManager;
        private SoundManager sound;
        private Flagpole flagpole;

        private Vector2 spawnPoint;
        private int tileSize;
        private readonly System.Action onMushroomCollected;
        private readonly System.Action<int> addScore;
        private readonly System.Action addCoin;
        private readonly System.Action resetScoreAndCoins;

        private bool _isHurt = false;
        private double _hurtTimer = 0;

        public CollisionManager(
            SmallMarioSprite small,
            BigMarioSprite big,
            StaticSprite current,
            List<Tile> tiles,
            EnemyManager enemies,
            PowerupFieldManager powerups,
            ScreenManager screens,
            SoundManager snd,
            Flagpole flag,
            Vector2 spawn,
            int tileSize,
            System.Action onMushroomCollected,
            System.Action<int> addScore,
            System.Action addCoin,
            System.Action resetScoreAndCoins
            )
        {
            smallMario = small;
            bigMario = big;
            currentMario = current;

            mapTiles = tiles;
            enemyManager = enemies;
            powerupManager = powerups;
            screenManager = screens;
            sound = snd;
            flagpole = flag;

            spawnPoint = spawn;
            this.tileSize = tileSize;
            this.onMushroomCollected = onMushroomCollected;
            this.addScore = addScore;
            this.addCoin = addCoin;
            this.resetScoreAndCoins = resetScoreAndCoins;
        }

        /// Main collision update, called from Game1.Update
        public void Update(GameTime gameTime, StaticSprite activeMario, bool isBig, CameraManager camera)
        {
            currentMario = activeMario;
            // === HURT INVINCIBILITY TIMER ===
            if (_isHurt)
            {
                _hurtTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (_hurtTimer <= 0)
                    _isHurt = false;
            }

            HandleTileCollision(activeMario);
            HandleEnemyCollision(activeMario, isBig, camera);
            HandlePowerups(activeMario, gameTime, isBig);
            flagpole.Update(gameTime);
        }

        // ---------------------------------------------------------
        // TILE COLLISION + QUESTION BLOCK LOGIC
        // ---------------------------------------------------------
        private void HandleTileCollision(StaticSprite activeMario)
        {
            if (!StaticCollisionHandler.HandleMany(activeMario, mapTiles, out var res, out var hitTile))
                return;

            // Only act if Mario hits the **bottom of his head to top of tile**
            if ((hitTile.TileName == "Question" || hitTile.TileName == "Brick") &&
                res.Side == typeCollision.Bottom && // top of tile is hit
                !usedQuestionBlocks.Contains(hitTile))
            {
                usedQuestionBlocks.Add(hitTile);
                sound.PlayEffect("bump");
                smallMario.verticalVelocity = 0;
                bigMario.verticalVelocity = 0;
                Vector2 spawnPos = hitTile.Position;
                spawnPos.Y -= tileSize; // offset to safely spawn powerups above block

                if (hitTile.TileName == "Question")
                {
                    // custom mushroom range, example logic
                    if (activeMario.Position.X < tileSize * 22 &&
                        activeMario.Position.X > tileSize * 19)
                    {
                        powerupManager.Spawn(PowerupType.Mushroom, spawnPos);
                        sound.PlayEffect("powerUpAppears");
                    }
                    else
                    {
                        powerupManager.Spawn(PowerupType.Coin, spawnPos);
                        sound.PlayEffect("coin");
                        addScore?.Invoke(100);
                        addCoin?.Invoke();
                    }
                }
                else if (hitTile.TileName == "Brick")
                {
                    // Optional: break brick logic or play effect
                    sound.PlayEffect("break");
                }
            }
            if ((hitTile.TileName == "Question" || hitTile.TileName == "Brick" || hitTile.TileName == "PipeBodyLeft" || hitTile.TileName == "PipeBodyRight") &&
                res.Side == typeCollision.Top)
            {
                smallMario.verticalVelocity = 0;
                smallMario._isJumping = false;
                bigMario.verticalVelocity = 0;
                bigMario._isJumping = false;
            }
        }

        // ---------------------------------------------------------
        // ENEMY COLLISIONS
        // ---------------------------------------------------------
        private void HandleEnemyCollision(StaticSprite activeMario, bool isBig, CameraManager camera)
        {
            var enemies = enemyManager.GetLiveEnemies();
            if (!_isHurt)
            {
                if (isBig)
                {
                    EnemyCollisionHandler.HandleMarioEnemyCollision(
                        bigMario,
                        enemies,
                        onBigHit: () =>
                        {
                            // Mario gets hit, shrink to small
                            smallMario.Position = bigMario.Position;

                            float shift = bigMario.Bounds.Bottom - smallMario.Bounds.Bottom;
                            smallMario.Position = new Vector2(
                            smallMario.Position.X,
                            smallMario.Position.Y + shift);
                            onMushroomCollected?.Invoke();
                            activeMario = smallMario;
                            // Optional: update currentMario reference if needed
                            currentMario = activeMario;
                            // Start invincibility
                            _isHurt = true;
                            _hurtTimer = 1.5; // Mario flashes for 1.2 seconds
                        });
                }
                else
                {
                    EnemyCollisionHandler.HandleMarioEnemyCollision(
                        smallMario,
                        enemies,
                        restart: () =>
                        {
                            activeMario = smallMario;

                            //smallMario.verticalVelocity = -5f;
                            smallMario.Position = spawnPoint;

                            enemyManager.Reset();

                            camera.Reset(spawnPoint);
                            camera.LookAt(spawnPoint);

                            screenManager.Lives--;
                            if (screenManager.Lives <= 0)
                                screenManager.ChangeState(GameState.GameOver);
                            else
                                screenManager.ChangeState(GameState.LevelIntro);

                            currentMario = activeMario;
                            usedQuestionBlocks.Clear(); 
                            resetScoreAndCoins?.Invoke();
                        });
                }
            }
        }

        // ---------------------------------------------------------
        // POWERUPS
        // ---------------------------------------------------------
        private void HandlePowerups(StaticSprite activeMario, GameTime gameTime, bool isBig)
        {
            var pickedUp = powerupManager.Update(gameTime, activeMario);
            if (pickedUp == null)
                return;

            pickedUp.IsAlive = false;

            switch (pickedUp.Type)
            {
                case PowerupType.Mushroom:
                    sound.PlayEffect("powerUp");
                    if(isBig == false)
                        onMushroomCollected?.Invoke();
                    addScore?.Invoke(200);
                    break;
                case PowerupType.Coin:
                    break;
                case PowerupType.GreenMushroom:
                    sound.PlayEffect("oneUp");
                    screenManager.Lives += 1;
                    addScore?.Invoke(200);
                    break;
                case PowerupType.Star:
                    sound.PlayEffect("powerUp");
                    addScore?.Invoke(500);
                    break;
                case PowerupType.FireFlower:
                    sound.PlayEffect("powerUp");
                    addScore?.Invoke(300);
                    break;
            }
        }
    }
}

