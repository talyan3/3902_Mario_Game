using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonogameTest.Sounds;
using MonogameTest.Screens;
using MonogameTest.Managers;

namespace MonogameTest.Managers
{
    public class CollisionManager
    {
        //other collision handlers
        private readonly TileCollisionHandler _tileHandler;
        private readonly EnemyCollisionHandlerWrapper _enemyHandler;
        private readonly PowerupCollisionHandlerWrapper _powerupHandler;
        private readonly Flagpole _flagpole;  

        public CollisionManager(
            TileCollisionHandler tileHandler,
            EnemyCollisionHandlerWrapper enemyHandler,
            PowerupCollisionHandlerWrapper powerupHandler,
            Flagpole flagpole)
        {
            _tileHandler = tileHandler;
            _enemyHandler = enemyHandler;
            _powerupHandler = powerupHandler;
            _flagpole = flagpole;
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

            _tileHandler.HandleTileCollision(activeMario);
            _enemyHandler.HandleEnemy(activeMario, camera);
            _powerupHandler.HandlePowerups(activeMario, gameTime);


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
                        addScore?.Invoke(100);
                        addCoin?.Invoke();
                    }
                }
                else if (hitTile.TileName == "Brick")
                {
                    sound.PlayEffect("break");
                }
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


    }
}
