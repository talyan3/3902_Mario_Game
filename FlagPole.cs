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

        private readonly SmallMarioSprite _smallMario;
        private readonly BigMarioSprite _bigMario;
        private StaticSprite _currentMario;

        private readonly MarioStateController _marioState;
        private readonly ScreenManager _screenManager;
        private readonly Vector2 _spawnPoint;
        private readonly ICamera _camera;
        private readonly HUDScreen _hud;

        private bool _isSliding = false;
        private bool _isWalking = false;
        private bool _hasWon = false;

        private float _slideSpeed = 40f;
        private float _walkSpeed = 90f;

        private float _soundDelayTimer = 0f;
        private const float SOUND_DELAY = 1.5f;

        private float _walkDelayTimer = 2.5f;
        private const float WALK_DELAY = 2.5f;

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

            // ===============================
            // HIT POLE (RUNNING OR FALLING)
            // ===============================
            bool fallingOntoPole =
                marioBounds.Bottom <= _poleRect.Top + 24 &&
                marioBounds.Right > _poleRect.Left &&
                marioBounds.Left < _poleRect.Right;

            bool fromLeft =
                marioBounds.Right <= _poleRect.Left + 14;

            bool validHit = fallingOntoPole || fromLeft;

            if (!_hasWon && validHit && marioBounds.Intersects(_poleRect))
            {
                _hasWon = true;
                _isSliding = true;

                Game1.DebugGodMode = true;
                Game1.InputLocked = true;

                _screenManager.FreezeTime();
                SoundManager.Instance.StopSong();
                SoundManager.Instance.PlayEffect("flagpole");

                //  Snap Mario flush to pole
                float attachX = _poleRect.Left - marioBounds.Width / 2f - 2f;
                mario.Position = new Vector2(attachX, mario.Position.Y);
            }

            // ===============================
            //  SLIDE DOWN
            // ===============================
            if (_isSliding)
            {
                Vector2 pos = mario.Position;
                pos.Y += _slideSpeed * dt;

                Game1.InputLocked = true;

                if (pos.Y >= _poleRect.Bottom - marioBounds.Height)
                {
                    pos.Y = _poleRect.Bottom - marioBounds.Height;
                    mario.Position = pos;

                    if (mario is SmallMarioSprite sm)
                        sm.verticalVelocity = 0;

                    if (mario is BigMarioSprite bm)
                        bm.verticalVelocity = 0;

                    _isSliding = false;
                    _soundDelayTimer = SOUND_DELAY;
                    _walkDelayTimer = WALK_DELAY;

                    SoundManager.Instance.PlaySong("levelComplete", false);
                    return;
                }

                mario.Position = pos;
                _flagRect.Y = (int)pos.Y - _flagTexture.Height;
                return;
            }

            // ===============================
            //  WAIT AT BOTTOM
            // ===============================
            if (_hasWon && !_isWalking)
            {
                if ((_soundDelayTimer -= dt) > 0) return;
                if ((_walkDelayTimer -= dt) > 0) return;

                _isWalking = true;
            }

            // ===============================
            //  WALK TO CASTLE (REAL WALK)
            // ===============================
            if (_isWalking)
            {
                Game1.InputLocked = true;
                Vector2 pos = mario.Position;

                // FORCE REAL WALK MODE (ANIMATION + PHYSICS)
                if (mario is SmallMarioSprite sm)
                    sm.ForceAutoWalkRight = true;

                if (mario is BigMarioSprite bm)
                    bm.ForceAutoWalkRight = true;

                mario.Update(gameTime);  // now animation + gravity both work

                _camera.LookAt(mario.Position);

                if (mario.Position.X >= _poleRect.Right + 140)
                {
                    // TURN OFF AUTO WALK
                    if (mario is SmallMarioSprite sm2)
                        sm2.ForceAutoWalkRight = false;

                    if (mario is BigMarioSprite bm2)
                        bm2.ForceAutoWalkRight = false;

                    _isWalking = false;

                    Vector2 rightSideSpawn = new Vector2(
                        _poleRect.Right + 160,
                        _spawnPoint.Y
                    );

                    _smallMario.Position = rightSideSpawn;
                    _bigMario.Position   = rightSideSpawn;

                    Game1.InputLocked = false;
                    Game1.DebugGodMode = false;

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