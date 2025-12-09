using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonogameTest.Sounds;
using MonogameTest.Screens;
using MonogameTest.Managers;
using System.Threading;

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
            System.Action<int> addScore,
            System.Action addCoin,
            System.Action resetScoreAndCoins
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
                HandleTileCollision(activeMario);
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
            HandleVoid(activeMario,camera);

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
                marioState.smallMario.verticalVelocity = 0f;
                marioState.bigMario.verticalVelocity = 0f;

                if (hitTile.TileName == "Question")
                {
                    if ((activeMario.Position.X < tileSize * 21 &&
                        activeMario.Position.X > tileSize * 19) || 
                        (activeMario.Position.X < tileSize * 80 &&
                        activeMario.Position.X > tileSize * 76) || 
                        (activeMario.Position.X < tileSize * 110 &&
                        activeMario.Position.X > tileSize * 108 && activeMario.Position.Y > tileSize * 10))
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
                    sound.PlayEffect("break");
                    if (activeMario.Position.X <= tileSize * 102 &&
                        activeMario.Position.X >= tileSize * 101)
                    {
                        powerupManager.Spawn(PowerupType.Star, spawnPos);
                    }
                }
            }
            if ((hitTile.TileName == "Question" || hitTile.TileName == "Brick" || hitTile.TileName == "PipeBodyLeft" || hitTile.TileName == "PipeBodyRight" || hitTile.TileName == "Stair" || hitTile.TileName == "Ground" || hitTile.TileName == "DarkGround" || hitTile.TileName == "DarkBrick") &&
                res.Side == typeCollision.Top)
            {
                int tileTop = hitTile.Bounds.Top;

                marioState.smallMario.Position = new Vector2(marioState.smallMario.Position.X, tileTop + 1);
                marioState.bigMario.Position = new Vector2(marioState.bigMario.Position.X, tileTop + 1);

                marioState.smallMario.verticalVelocity = 0f;
                marioState.smallMario._isJumping = false;
                //marioState.smallMario.gravity = 0f;
                marioState.bigMario.verticalVelocity = 0f;
                marioState.bigMario._isJumping = false;
                //marioState.bigMario.gravity = 0;
            }
            if ((hitTile.TileName == "Ground" || hitTile.TileName == "PipeTopLeft" || hitTile.TileName == "PipeTopRight" || hitTile.TileName == "PipeBodyLeft" || hitTile.TileName == "PipeBodyRight" || hitTile.TileName == "Stair" || hitTile.TileName == "DarkGround") && (marioState.smallMario._isJumping == false || marioState.bigMario._isJumping == false) && (res.Side == typeCollision.Right || res.Side == typeCollision.Left))
            {
                marioState.smallMario.verticalVelocity = -15f;
                marioState.bigMario.verticalVelocity = -15f;
            }
            if ((hitTile.TileName == "Ground" || hitTile.TileName == "PipeTopLeft" || hitTile.TileName == "PipeTopRight" || hitTile.TileName == "PipeBodyLeft" || hitTile.TileName == "PipeBodyRight" || hitTile.TileName == "Stair" || hitTile.TileName == "DarkGround") && (marioState.smallMario._isJumping == true || marioState.bigMario._isJumping == true) && (res.Side == typeCollision.Right || res.Side == typeCollision.Left))
            {
                marioState.smallMario._isJumping = false;
                marioState.bigMario._isJumping = false;
            }
        }

        // =========================================================
        // ENEMY COLLISIONS
        // =========================================================
        private void HandleEnemyCollision(StaticSprite activeMario, CameraManager camera)
        {
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

                        if (screenManager.CurrentState != GameState.GameOver)
                            screenManager.ChangeState(GameState.LevelIntro);

                        usedQuestionBlocks.Clear();
                        resetScoreAndCoins?.Invoke();
                    });
            }
        }
        private void HandleVoid(StaticSprite activeMario, CameraManager camera)
        {
            if (marioState.smallMario.Position.Y > tileSize * 18 || marioState.bigMario.Position.Y > tileSize * 18)
            {
                marioState.ForceSmall(spawnPoint);

                enemyManager.Reset();
                camera.Reset(spawnPoint);
                camera.LookAt(spawnPoint);

                screenManager.LoseLife();

                if (screenManager.CurrentState != GameState.GameOver)
                    screenManager.ChangeState(GameState.LevelIntro);

                usedQuestionBlocks.Clear();
                resetScoreAndCoins?.Invoke();
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
                    addScore?.Invoke(200);
                    break;

                case PowerupType.Coin:
                    addCoin?.Invoke();
                    addScore?.Invoke(100);
                    break;

                case PowerupType.GreenMushroom:
                    sound.PlayEffect("oneUp");
                    screenManager.AddScore(200);
                    screenManager.ChangeState(screenManager.CurrentState);
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
