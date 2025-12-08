using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameTest.Sounds;
using MonogameTest.Screens;

namespace MonogameTest
{
    public class Flagpole
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _flagTexture;
        private readonly Rectangle _poleRect;
        private Rectangle _flagRect;

        private readonly MarioStateController _marioState;
        private readonly ScreenManager _screenManager;

        private readonly SmallMarioSprite _smallMario;
        private readonly BigMarioSprite _bigMario;
        private StaticSprite _currentMario;

        private readonly Vector2 _spawnPoint;
        private readonly ICamera _camera;
        private readonly HUDScreen _hud;

        private bool _isSliding = false;
        private bool _isWalking = false;
        private bool _hasWon = false;

        private float _slideSpeed = 18f;
        private float _walkSpeed = 90f;

        private float _soundDelayTimer = 0f;
        private const float SOUND_DELAY = 2.8f;

        private float _walkDelayTimer = 0f;
        private const float WALK_DELAY = 1.2f; 


        private float _victoryTimer = 0f;
        private const float VICTORY_DELAY = 1.5f;

        private readonly DetectCollisions _collision = new DetectCollisions();

        public Flagpole(
            SpriteBatch spriteBatch,
            Texture2D flagTexture,
            Rectangle poleRect,
            SmallMarioSprite smallMario,
            BigMarioSprite bigMario,
            StaticSprite currentMario,
            MarioStateController marioState,
            ScreenManager screenManager,
            Vector2 spawnPoint,
            ICamera camera,
            HUDScreen hud)
        {
            _spriteBatch = spriteBatch;
            _flagTexture = flagTexture;
            _poleRect = poleRect;

            _smallMario = smallMario;
            _bigMario = bigMario;
            _currentMario = currentMario;

            _marioState = marioState;
            _screenManager = screenManager;
            _spawnPoint = spawnPoint;
            _camera = camera;
            _hud = hud;

            _flagRect = new Rectangle(
                poleRect.Right - flagTexture.Width,
                poleRect.Top,
                flagTexture.Width,
                flagTexture.Height);
        }

        public void Update(GameTime gameTime)
        {
            if (_screenManager.CurrentState != GameState.Playing)
                return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var mario = _marioState.CurrentMario;
            Rectangle marioBounds = mario.Bounds;

            //COLLISION AT ANY HEIGHT
            if (!_hasWon && marioBounds.Intersects(_poleRect))
            {
                _hasWon = true;
                _isSliding = true;

                Game1.DebugGodMode = true;
                Game1.InputLocked = true;

                _screenManager.FreezeTime();

                SoundManager.Instance.StopSong();
                SoundManager.Instance.PlayEffect("flagpole");

                float hitHeight = _poleRect.Bottom - marioBounds.Bottom;
                int heightScore =
                    hitHeight > 120 ? 5000 :
                    hitHeight > 96 ? 4000 :
                    hitHeight > 64 ? 2000 :
                    hitHeight > 32 ? 1000 : 400;

                _screenManager.AddScore(heightScore);

                mario.Position = new Vector2(
                    _poleRect.Right + 4,
                    mario.Position.Y
                );
            }

            //  SLOW SLIDE DOWN
            if (_isSliding)
            {
                Vector2 marioPos = mario.Position;
                marioPos.Y += _slideSpeed * dt;

                if (marioPos.Y >= _poleRect.Bottom - mario.Bounds.Height)
                {
                    marioPos.Y = _poleRect.Bottom - mario.Bounds.Height;
                    _isSliding = false;
                    _soundDelayTimer = SOUND_DELAY;
                    _walkDelayTimer = WALK_DELAY; 
                    SoundManager.Instance.PlaySong("levelComplete", loop: false);
                }

                mario.Position = marioPos;
                _flagRect.Y = (int)marioPos.Y - _flagTexture.Height;
                return;
            }

            //  WAIT FOR FULL SOUND
            if (_hasWon && !_isSliding && !_isWalking)
            {
                if (_soundDelayTimer > 0f)
                {
                    _soundDelayTimer -= dt;
                    return;
                }

                if (_walkDelayTimer > 0f)   
                {
                    _walkDelayTimer -= dt;
                    return;
                }
            }

            //  AUTO WALK TO CASTLE
            if (_hasWon && !_isSliding && !_isWalking)
            {
                _isWalking = true;
            }

            if (_isWalking)
            {
                // FORCE AUTO WALK MODE
                if (mario is SmallMarioSprite sm)
                    sm.ForceAutoWalkRight = true;

                if (mario is BigMarioSprite bm)
                    bm.ForceAutoWalkRight = true;

                mario.Update(gameTime);

                _camera.LookAt(mario.Position);

                if (mario.Position.X >= _poleRect.Right + 60)
                {
                    // turn off auto walk
                    if (mario is SmallMarioSprite sm2)
                        sm2.ForceAutoWalkRight = false;

                    if (mario is BigMarioSprite bm2)
                        bm2.ForceAutoWalkRight = false;

                    _isWalking = false;
                    _screenManager.ResetLevel();
                    _screenManager.ChangeState(GameState.Title);
                }

                return;
            }

            // END SCENE
            if (_hasWon)
            {
                _victoryTimer -= dt;

                _screenManager.ConvertTimeToScore();

                if (_victoryTimer <= 0f && _screenManager.Time <= 0)
                {
                    ResetManager.FullLevelReset(
                        _smallMario,
                        _bigMario,
                        ref _currentMario,
                        _spawnPoint,
                        _camera,
                        _hud,
                        _screenManager
                    );

                    _hasWon = false;
                    _screenManager.ResetLevel();
                    _screenManager.ChangeState(GameState.Title);
                }
            }
        }

        public void Draw()
        {
            _spriteBatch.Draw(_flagTexture, _flagRect, Color.White);
        }
    }
}