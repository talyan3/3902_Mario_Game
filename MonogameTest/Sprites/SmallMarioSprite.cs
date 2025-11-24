using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Particles.Modifiers;
using MonogameTest.Sounds;

namespace MonogameTest
{
    public class SmallMarioSprite : StaticSprite
    {
        public static readonly SmallMarioNumbers smallN = NumberLoad.Numbers.SmallMario;
        public override Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Scale { get; set; } = new Vector2(smallN.Scale, smallN.Scale);

        private float _moveSpeed = smallN.MoveSpeed;
        private float _sprintMultiplier = smallN.SprintMultiplier;
        private SpriteEffects _effects = SpriteEffects.None;

        //CONST TAKEN AWAY
        private int FrameW = smallN.FrameWidth;
        private int FrameH = smallN.FrameHeight;

        private readonly List<TextureRegion> _runFrames = new();
        private TextureRegion _idleFrame;
        private TextureRegion _jumpFrame;
        private TextureRegion _crouchFrame;
        private TextureRegion _current;

        private int _frameIndex = 0;
        private float _frameTimer = 0f;
        private float _frameTime = smallN.FrameTime;

        private bool _isJumping = false;
        private bool _isCrouching = false;
        private float _jumpOffset = smallN.JumpOffset;
        private Vector2 _groundPos;
        public SoundManager SoundManager { get; set; }

        public float pixels = smallN.BounceHeight;

        // physics
        private float verticalVelocity = 0f;
        private float gravity = smallN.Gravity;
        private float jumpStrength = smallN.JumpStrength;


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

        public void Bounce(pixels)
        {
            verticalVelocity = pixels;
        }

        public SmallMarioSprite(GraphicsDevice graphicsDevice)
        {
            Texture2D texture = Texture2D.FromFile(graphicsDevice, "small-mario-final.png");

            _runFrames.Add
                (new TextureRegion
                    (texture, smallN.SheetColumnWidth * smallN.RunFrames[0], 0, FrameW, FrameH));
            _runFrames.Add
                (new TextureRegion
                    (texture, smallN.SheetColumnWidth * smallN.RunFrames[1], 0, FrameW, FrameH));
            _runFrames.Add
                (new TextureRegion
                    (texture, smallN.SheetColumnWidth * smallN.RunFrames[2], 0, FrameW, FrameH));

            _crouchFrame = new TextureRegion(texture, 0, 0, FrameW, FrameH);
            _jumpFrame = new TextureRegion(texture, smallN.SheetColumnWidth * smallN.JumpFrame, 0, FrameW, FrameH);
            _idleFrame = new TextureRegion(texture, smallN.SheetColumnWidth * smallN.IdleFrame, 0, FrameW, FrameH);

            _current = _idleFrame;
            Region = _current;
        }

        // === MAIN UPDATE ===
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
                        SoundManager.PlayEffect("jumpSmall");
                    }
                    //_current = _jumpFrame;
                }

                // ---- GRAVITY ----
                verticalVelocity += gravity * dt;
                Position = new Vector2(Position.X, Position.Y + verticalVelocity * dt);

                // === IDLE ===
                //if (_isJumping)
                //{
                    //Position = new Vector2(Position.X, _groundPos.Y);
                    //_isJumping = false;
                //}

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

        // Default update (if no camera provided)
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

        public override void Draw(SpriteBatch spriteBatch, Vector2 _)
        {
            if (_current == null) return;

            var origin = new Vector2(_current.Width / 2f, _current.Height);
            _current.Draw(spriteBatch, Position, Color.White, 0f, origin, Scale, _effects, 0f);
        }
    }
}