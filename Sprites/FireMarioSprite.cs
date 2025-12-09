using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonogameTest
{
    public class FireMarioSprite : StaticSprite
    {
        public override Vector2 Position { get; set; }
        public Vector2 Scale { get; set; } = new Vector2(1f, 1f);

        private const int FrameW = 19;  
        private const int FrameH = 35;  

        private readonly List<TextureRegion> _runFrames = new();
        private TextureRegion _idleFrame, _jumpFrame, _current;

        private int _frameIndex;
        private float _frameTimer;
        private float _frameTime = 0.12f;

        public bool _isJumping = false;
        public float verticalVelocity = 0f;

        private float gravity = 900f;
        private float jumpStrength = -350f;



        private SpriteEffects _effects = SpriteEffects.None;

        public FireMarioSprite(GraphicsDevice graphics)
        {
            Texture2D texture = Texture2D.FromFile(graphics, "mario_star_bkgrd_remvd.PNG");

            _idleFrame = new TextureRegion(texture, 0, 0, FrameW, FrameH);
            _runFrames.Add(new TextureRegion(texture, 19, 0, FrameW, FrameH));
            _runFrames.Add(new TextureRegion(texture, 38, 0, FrameW, FrameH));
            _runFrames.Add(new TextureRegion(texture, 57, 0, FrameW, FrameH));

            _jumpFrame = new TextureRegion(texture, 76, 0, FrameW, FrameH);

            _current = _idleFrame;
            Region = _current;
        }

        public override Rectangle Bounds
        {
            get
            {
                int w = (int)(Region.Width * Scale.X);
                int h = (int)(Region.Height * Scale.Y);

                int left = (int)(Position.X - w / 2f);
                int top = (int)(Position.Y - h);

                return new Rectangle(left, top, w, h);
            }
        }

        public override void Update(GameTime gameTime)
        {
            Update(gameTime, 0f);
        }

        public void Update(GameTime gameTime, float cameraLeftLimit)
        {
            var kb = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            bool moving = false;
            float speed = 100f;

            if (kb.IsKeyDown(Keys.Left))
            {
                Position = new Vector2(Position.X - speed * dt, Position.Y);
                _effects = SpriteEffects.FlipHorizontally;
                moving = true;
            }

            if (kb.IsKeyDown(Keys.Right))
            {
                Position = new Vector2(Position.X + speed * dt, Position.Y);
                _effects = SpriteEffects.None;
                moving = true;
            }

            if (kb.IsKeyDown(Keys.Up) && !_isJumping)
            {
                _isJumping = true;
                verticalVelocity = jumpStrength;
            }

            // Gravity
            verticalVelocity += gravity * dt;
            Position = new Vector2(Position.X, Position.Y + verticalVelocity * dt);

            if (verticalVelocity > 0)
                _isJumping = true;

            // Animation
            if (_isJumping)
                _current = _jumpFrame;
            else if (moving)
                AdvanceRun(dt);
            else
                _current = _idleFrame;

            Region = _current;
        }

        public void Bounce(float pixels)
		{
			verticalVelocity = pixels;
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
            Vector2 origin = new Vector2(10, 35);
            _current.Draw(spriteBatch, Position, Color.White, 0f, origin, Scale, _effects, 0f);
        }
    }
}
