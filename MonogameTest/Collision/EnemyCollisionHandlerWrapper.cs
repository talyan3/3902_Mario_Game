using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonogameTest.Sounds;
using MonogameTest.Managers;
using MonogameTest.Screens;

namespace MonogameTest.Managers
{
    public class EnemyCollisionHandlerWrapper
    {
        private readonly MarioStateController _marioState;
        private readonly EnemyManager _enemyManager;
        private readonly ScreenManager _screenManager;
        private readonly SoundManager _sound;
        private readonly Vector2 _spawnPoint;
        private readonly TileCollisionHandler _tileHandler;

        private bool _isHurt = false;
        private double _hurtTimer = 0;

        public EnemyCollisionHandlerWrapper(
            MarioStateController marioState,
            EnemyManager enemyManager,
            ScreenManager screenManager,
            SoundManager sound,
            Vector2 spawnPoint,
            TileCollisionHandler tileHandler)
        {
            _marioState = marioState;
            _enemyManager = enemyManager;
            _screenManager = screenManager;
            _sound = sound;
            _spawnPoint = spawnPoint;
            _tileHandler = tileHandler;
        }
        private void HandleEnemy(StaticSprite activeMario, CameraManager camera)
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

            if(_isHurt)
            {
                _hurtTimer -= Game1.GameTime.ElapsedGameTime.TotalSeconds;
                if (_hurtTimer <= 0)
                    _isHurt = false;
            }
        }

    }
}