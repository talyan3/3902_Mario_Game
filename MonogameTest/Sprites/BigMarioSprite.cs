using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameTest.Sounds;

namespace MonogameTest
{
    public class BigMarioSprite : StaticSprite
    {
        public override Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Scale { get; set; } = new Vector2(1f, 1f);

        private float _moveSpeed = 100f;
        private float _sprintMultiplier = 1.8f;
        private SpriteEffects _effects = SpriteEffects.None;

        private const int FrameW = 16;
        private const int FrameH = 32;

        private readonly List<TextureRegion> _runFrames = new();
        private TextureRegion _idleFrame;
        private TextureRegion _jumpFrame;
        private TextureRegion _crouchFrame;
        private TextureRegion _current;

        private int _frameIndex = 0;
        private float _frameTimer = 0f;
        private float _frameTime = 0.12f;

        private bool _isJumping = false;
        private bool _isCrouching = false;
        private float _jumpOffset = 100f;
		private Vector2 _groundPos;
		public SoundManager SoundManager { get; set; }

        private const float CROUCH_DRAW_OFFSET = 6f; // how far lower the crouch sprite is drawn
        
        private float verticalVelocity = 0f;
        private float gravity = 900f;
        private float jumpStrength = -350f;


        public override Rectangle Bounds
        {
            get
            {
                if (Region == null) return Rectangle.Empty;
                int w = (int)(Region.Width * Scale.X);
                int h = (int)(Region.Height * Scale.Y);
                int left = (int)(Position.X - w / 2f);
                int top = (int)(Position.Y - h);
                return new Rectangle(left, top, w, h);
            }
        }

        public BigMarioSprite(GraphicsDevice graphicsDevice)
        {
            Texture2D texture = Texture2D.FromFile(graphicsDevice, "big-mario-final.png");

            _runFrames.Add(new TextureRegion(texture, 30 * 3, 0, FrameW, FrameH));
            _runFrames.Add(new TextureRegion(texture, 30 * 4, 0, FrameW, FrameH));
            _runFrames.Add(new TextureRegion(texture, 30 * 5, 0, FrameW, FrameH));

            _crouchFrame = new TextureRegion(texture, 0, 0, FrameW, FrameH);
            _jumpFrame = new TextureRegion(texture, 30 * 1, 0, FrameW, FrameH);
            _idleFrame = new TextureRegion(texture, 30 * 6, 0, FrameW, FrameH);

            _current = _idleFrame;
			Region = _current;
        }

        // Update with camera boundary
        public void Update(GameTime gameTime, float cameraLeftLimit)
        {
            var kb = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_groundPos == Vector2.Zero)
                _groundPos = Position;

            bool moving = false;

            float speed = _moveSpeed;
            if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift))
                speed *= _sprintMultiplier;

            // === CROUCH ===
            if (kb.IsKeyDown(Keys.Down) && !_isJumping)
            {
                _isCrouching = true;
                _current = _crouchFrame;
                _frameIndex = 0;
                _frameTimer = 0f;
            }
            else
            {
                _isCrouching = false;

                // === MOVE LEFT ===
                if (kb.IsKeyDown(Keys.Left))
                {
                    float newX = Position.X - speed * dt;

                    // prevent going left past camera or level start
                    if (newX >= cameraLeftLimit && newX >= 0)
                        Position = new Vector2(newX, Position.Y);
                        moving = true;

                    _effects = SpriteEffects.None;
                    AdvanceRun(dt);
                }
                // === MOVE RIGHT ===
                else if (kb.IsKeyDown(Keys.Right))
                {
                    Position = new Vector2(Position.X + speed * dt, Position.Y);
                    moving = true;
                    _effects = SpriteEffects.FlipHorizontally;
                    AdvanceRun(dt);
                }
                // === JUMP ===
                if (kb.IsKeyDown(Keys.Up))
                {
                    if (!_isJumping)
                    {
                        _isJumping = true;
                        verticalVelocity = jumpStrength;
                        SoundManager.PlayEffect("jumpBig");
                    }
                    //_current = _jumpFrame;
                }

                // ---- GRAVITY ----
                verticalVelocity += gravity * dt;
                Position = new Vector2(Position.X, Position.Y + verticalVelocity * dt);

                // ---- LANDING ----
                if (Position.Y >= _groundPos.Y)
                {
                    Position = new Vector2(Position.X, _groundPos.Y);
                    verticalVelocity = 0;
                    _isJumping = false;
                }

                // ---- ANIMATIONS ----
                if (_isJumping)
                {
                    _current = _jumpFrame;
                }
                else if (moving)
                {
                    AdvanceRun(dt);
                }
                else
                {
                    _current = _idleFrame;
                    _frameIndex = 0;
                    _frameTimer = 0f;
                }

            }
            Region = _current;
        }

        public override void Update(GameTime gameTime)
        {
            Update(gameTime, 0f);
        }

        private void AdvanceRun(float dt)
        {
            _frameTimer += dt;
            if (_frameTimer >= _frameTime)
            {
                _frameTimer -= _frameTime;
                _frameIndex = (_frameIndex + 1) % _runFrames.Count;
            }

            _current = _runFrames[_frameIndex];
        }

        public void Bounce(float pixels = 24f)
		{
			Position = new Vector2(Position.X, Position.Y - pixels);
		}


        public override void Draw(SpriteBatch spriteBatch, Vector2 _)
        {
            if (_current == null) return;

            var origin = new Vector2(_current.Width / 2f, _current.Height);

            // visually lower sprite while crouching without moving collision box
            float drawYOffset = _isCrouching ? CROUCH_DRAW_OFFSET : 0f;

            _current.Draw(
                spriteBatch,
                new Vector2(Position.X, Position.Y + drawYOffset),
                Color.White,
                0f,
                origin,
                Scale,
                _effects,
                0f
            );
        }
    }
}