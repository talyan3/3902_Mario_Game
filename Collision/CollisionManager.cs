using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonogameTest.Sounds;
using MonogameTest.Screens;
using MonogameTest.Managers;
using System.Threading;
using System;
using System.IO;

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

private void LogToFile(string message)
{
    try
    {
        File.AppendAllText("answer.txt", message + Environment.NewLine);
    }
    catch (Exception ex)
    {
        Console.WriteLine("File write error: " + ex.Message);
    }
}
        // =========================================================
        // MAIN UPDATE
        // =========================================================
        public void Update(GameTime gameTime, StaticSprite activeMario, CameraManager camera)
        {
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
                // DEBUG LOG — prints to console or VS Output window
   string msg =
        $"Mario hit tile={hitTile.TileName}, gid={hitTile.Gid}, tileCoord=({(int)(hitTile.Position.X / tileSize)}, {(int)(hitTile.Position.Y / tileSize)}), worldPos=({hitTile.Position.X},{hitTile.Position.Y})";

    LogToFile(msg);


                usedQuestionBlocks.Add(hitTile);
                sound.PlayEffect("bump");

                Vector2 spawnPos = hitTile.Position;
                spawnPos.Y -= tileSize;
                marioState.smallMario.verticalVelocity = 0f;
                marioState.bigMario.verticalVelocity = 0f;

                if (hitTile.TileName == "Question")
                
                {
                     int tileX = (int)(hitTile.Position.X / tileSize);
    int tileY = (int)(hitTile.Position.Y / tileSize);
                   if (tileX == 109 && tileY == 5)
    {
        if (marioState.IsBig || marioState.IsFire)
        {
            powerupManager.Spawn(PowerupType.FireFlower, spawnPos);
            sound.PlayEffect("powerUpAppears");
        }
        else
        {
            powerupManager.Spawn(PowerupType.Mushroom, spawnPos);
            sound.PlayEffect("powerUpAppears");
        }
    }

    //
    // 2️⃣ ALWAYS-MUSHROOM BLOCK (106,9)
    //
    else if (tileX == 106 && tileY == 9)
    {
        powerupManager.Spawn(PowerupType.Mushroom, spawnPos);
        sound.PlayEffect("powerUpAppears");
    }

    //
    // 3️⃣ YOUR ORIGINAL REGION MUSHROOM BLOCK
    //
    else if (activeMario.Position.X < tileSize * 22 &&
             activeMario.Position.X > tileSize * 19)
    {
        powerupManager.Spawn(PowerupType.Mushroom, spawnPos);
        sound.PlayEffect("powerUpAppears");
    }

    //
    // 4️⃣ DEFAULT = COIN
    //
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
                }
            }
            if ((hitTile.TileName == "Question" || hitTile.TileName == "Brick" ||
     hitTile.TileName == "PipeBodyLeft" || hitTile.TileName == "PipeBodyRight" ||
     hitTile.TileName == "Stair" || hitTile.TileName == "Ground") &&
    res.Side == typeCollision.Top)
{
    int tileTop = hitTile.Bounds.Top;

    // Small Mario
    marioState.smallMario.Position =
        new Vector2(marioState.smallMario.Position.X, tileTop + 1);
    marioState.smallMario.verticalVelocity = 0f;
    marioState.smallMario._isJumping = false;

    // Big Mario
    marioState.bigMario.Position =
        new Vector2(marioState.bigMario.Position.X, tileTop + 1);
    marioState.bigMario.verticalVelocity = 0f;
    marioState.bigMario._isJumping = false;

    // Fire Mario (ADDED)
    if (marioState.fireMario != null)
    {
        marioState.fireMario.Position =
            new Vector2(marioState.fireMario.Position.X, tileTop + 1);

        marioState.fireMario.verticalVelocity = 0f;
        marioState.fireMario._isJumping = false;
    }
}

            if (hitTile.TileName != "Air" &&
    marioState.smallMario._isJumping == false &&
    (res.Side == typeCollision.Right || res.Side == typeCollision.Left))
{
    marioState.smallMario.verticalVelocity = -15f;
    marioState.bigMario.verticalVelocity = -15f;

    if (marioState.fireMario != null)
        marioState.fireMario.verticalVelocity = -15f;
}

        }

        // =========================================================
        // ENEMY COLLISIONS
        // =========================================================
        private void HandleEnemyCollision(StaticSprite activeMario, CameraManager camera)
{
    var enemies = enemyManager.GetLiveEnemies();
    if (_isHurt) return;

  
    if (marioState.IsFire)
    {
        EnemyCollisionHandler.HandleMarioEnemyCollision(
            marioState.CurrentMario as FireMarioSprite,
            enemies,
            onFireHit: () =>
            {
                marioState.Shrink();  
                _isHurt = true;
                _hurtTimer = 1.5;
            });

        return; 
    }


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
                    marioState.Fire();   
                    break;
            }
        }
    }
}
