using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonogameTest.Sounds;
using MonogameTest.Screens;
using MonogameTest.Managers;

namespace MonogameTest.Managers
{
    public class CollisionManager
    {
        private readonly MarioStateController marioState;

        private readonly List<Tile> mapTiles;
        private readonly HashSet<Tile> usedQuestionBlocks = new HashSet<Tile>();

        private readonly EnemyManager enemyManager;
        private readonly PowerupFieldManager powerupManager;
        private readonly ScreenManager screenManager;
        private readonly SoundManager sound;
        private readonly Flagpole flagpole;

        private readonly Vector2 spawnPoint;
        private readonly int tileSize;

        private readonly System.Action<int> addScore;
        private readonly System.Action addCoin;
        private readonly System.Action resetScoreAndCoins;

        private bool _isHurt = false;
        private double _hurtTimer = 0;

        private readonly SmallMarioSprite smallMario;
        private readonly BigMarioSprite bigMario;
        private readonly ICamera camera;
        private readonly Game1 game;




        public CollisionManager(
            MarioStateController marioState,
            List<Tile> tiles,
            EnemyManager enemies,
            PowerupFieldManager powerups,
            ScreenManager screens,
            SoundManager snd,
            Flagpole flag,
            Vector2 spawn,
            int tileSize,
            SmallMarioSprite smallMario,
            BigMarioSprite bigMario,
            ICamera camera
        )
        {
            this.marioState = marioState;

            mapTiles = tiles;
            enemyManager = enemies;
            powerupManager = powerups;
            screenManager = screens;
            sound = snd;
            flagpole = flag;

            spawnPoint = spawn;
            this.tileSize = tileSize;

            this.addScore = addScore;
            this.addCoin = addCoin;
            this.resetScoreAndCoins = resetScoreAndCoins;
        }

        // =========================================================
        // MAIN UPDATE
        // =========================================================
        public void Update(GameTime gameTime, StaticSprite activeMario, CameraManager camera)
        {

            if (Game1.InputLocked)
            {
                flagpole.Update(gameTime);  // allow victory sequence
                return;
            }

            // === HURT INVINCIBILITY TIMER ===
            if (_isHurt)
            {
                _hurtTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (_hurtTimer <= 0)
                    _isHurt = false;
            }

            HandleTileCollision(activeMario);
            HandleEnemyCollision(activeMario, camera);
            HandlePowerups(activeMario, gameTime);

            flagpole.Update(gameTime);
        }

        // =========================================================
        // TILE COLLISION + QUESTION BLOCK LOGIC
        // =========================================================
        private void HandleTileCollision(StaticSprite activeMario)
        {
            if (!StaticCollisionHandler.HandleMany(activeMario, mapTiles, out var res, out var hitTile))
                return;

            // --- HEAD HIT LOGIC ---
            if ((hitTile.TileName == "Question" || hitTile.TileName == "Brick") &&
                res.Side == typeCollision.Bottom &&
                !usedQuestionBlocks.Contains(hitTile))
            {
                usedQuestionBlocks.Add(hitTile);
                sound.PlayEffect("bump");

                Vector2 spawnPos = hitTile.Position;
                spawnPos.Y -= tileSize;

                if (hitTile.TileName == "Question")
                {
                    if (activeMario.Position.X < tileSize * 22 &&
                        activeMario.Position.X > tileSize * 19)
                    {
                        powerupManager.Spawn(PowerupType.Mushroom, spawnPos);
                        sound.PlayEffect("powerUpAppears");
                    }
                    else
                    {
                        powerupManager.Spawn(PowerupType.Coin, spawnPos);
                        if (!Game1.ChristmasMode)
                        {
                            SoundManager.Instance.PlayEffect("coin");
                        } else
                        {
                            SoundManager.Instance.PlayEffect("jingle");
                        }
                        ScreenManager.Instance.AddScore(200);
                        addCoin?.Invoke();
                    }
                }
                else if (hitTile.TileName == "Brick")
                {
                    SoundManager.Instance.PlayEffect("breakBlock");
                    ScreenManager.Instance.AddScore(50);
                }
            }
            

        }

        // =========================================================
        // ENEMY COLLISIONS
        // =========================================================
        private void HandleEnemyCollision(StaticSprite activeMario, CameraManager camera)
        {
            // --- SNAIL INSTANT-KILL COLLISION ---
            if (Game1.ChristmasMode && enemyManager.Snail != null)
            {
                var snail = enemyManager.Snail;
                Rectangle marioRect = activeMario.Bounds;
                Rectangle snailRect = snail.Bounds;   // we must add this property (see below)

                if (marioRect.Intersects(snailRect))
                {
                    // Trigger game over or restart logic
                    marioState.ForceSmall(spawnPoint);
                    enemyManager.Reset();
                    camera.Reset(spawnPoint);
                    while (screenManager.Lives > 0)
                    {
                        screenManager.LoseLife();
                    }



                    screenManager.ChangeState(GameState.GameOver);

                    return;
                }
            }

            if (Game1.DebugGodMode)
            return;

            var enemies = enemyManager.GetLiveEnemies();
            if (_isHurt) return;

            if (marioState.IsBig)
            {
                EnemyCollisionHandler.HandleMarioEnemyCollision(
                    marioState.CurrentMario as BigMarioSprite,   
                    enemies,
                    onBigHit: () =>
                    {
                        marioState.Shrink();
                        _isHurt = true;
                        _hurtTimer = 1.5;
                    });
            }
            else
            {
                EnemyCollisionHandler.HandleMarioEnemyCollision(
                    marioState.CurrentMario as SmallMarioSprite, 
                    enemies,
                    restart: () =>
                    {
                        marioState.ForceSmall(spawnPoint);

                        enemyManager.Reset();
                        camera.Reset(spawnPoint);
                        camera.LookAt(spawnPoint);

                        screenManager.LoseLife();
                        

                        // Freeze gameplay and start the death timer
                        Game1.InputLocked = true;
                        SoundManager.Instance.StopSong();
                        SoundManager.Instance.PlaySong("youreDead");
                        Game1.StartDeathTimer();



                        if (screenManager.CurrentState != GameState.GameOver)
                            screenManager.ChangeState(GameState.LevelIntro);

                        usedQuestionBlocks.Clear();
                        resetScoreAndCoins?.Invoke();
                    });
            }
        }


        // =========================================================
        // POWERUPS
        // =========================================================
        private void HandlePowerups(StaticSprite activeMario, GameTime gameTime)
        {
            var pickedUp = powerupManager.Update(gameTime, activeMario);
            if (pickedUp == null)
                return;

            pickedUp.IsAlive = false;

            switch (pickedUp.Type)
            {
                case PowerupType.Mushroom:
                    marioState.Grow();
                    ScreenManager.Instance.AddScore(200);
                    break;

                case PowerupType.Coin:
                    addCoin?.Invoke();
                    ScreenManager.Instance.AddScore(100);
                    break;

                case PowerupType.GreenMushroom:
                    sound.PlayEffect("oneUp");
                    screenManager.AddScore(200);
                    ScreenManager.Instance.GainLife();
                    screenManager.ChangeState(screenManager.CurrentState);
                    break;

                case PowerupType.Star:
                    sound.PlayEffect("powerUp");
                    ScreenManager.Instance.AddScore(500);
                    break;

                case PowerupType.FireFlower:
                    sound.PlayEffect("powerUp");
                    ScreenManager.Instance.AddScore(300);
                    break;
            }
        }
    }
}
