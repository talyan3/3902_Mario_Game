using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest
{
    public class CameraManager : ICamera
    {
        private readonly Viewport _viewport;

        public Vector2 Position { get; private set; } = Vector2.Zero;

        private float _smoothSpeed = 0.15f;  // how smoothly camera follows target
        private float _levelWidth = 2000f;   // total level width (adjust once LevelManager is ready)
        private float _levelHeight = 720f;   // level height

        private float _furthestRight = 0f;   // store max scroll position for one-way behavior
        private float _horizontalOffsetRatio = 0.35f; 

        public CameraManager(Viewport viewport)
        {
            _viewport = viewport;
        }


        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0f)) *
                   Matrix.CreateTranslation(new Vector3(_viewport.Width / 2f, _viewport.Height / 2f, 0f));
        }

        // follows mario around screen
        public void LookAt(Vector2 target)
        {
            float targetX = target.X - (_viewport.Width * _horizontalOffsetRatio);
            float targetY = 0f; 

            Vector2 desiredPosition = new(targetX, targetY);
            Position = Vector2.Lerp(Position, desiredPosition, _smoothSpeed);

            // level bouncs
            Position.X = MathHelper.Clamp(Position.X, 0, _levelWidth - _viewport.Width);
            Position.Y = MathHelper.Clamp(Position.Y, 0, _levelHeight - _viewport.Height);

            // Prevent scrolling backward
            if (Position.X > _furthestRight)
                _furthestRight = Position.X;
            else
                Position = new Vector2(_furthestRight, Position.Y);
        }

        //moves camera if needed
        public void Move(Vector2 delta)
        {
            Position += delta;
        }

        //resets camera if needed
        public void Reset(Vector2 startPosition)
        {
            Position = startPosition;
            _furthestRight = startPosition.X;
        }
    }
}
