using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest
{
    public class Camera2D
    {
        private readonly Viewport _viewport;

        // Camera position in world space
        public Vector2 Position { get; private set; } = Vector2.Zero;

        // Smoothness of camera motion
        private float _smoothSpeed = 0.15f;

        // Level boundaries (change when LevelManager loads levels)
        private float _levelWidth = 2000f;
        private float _levelHeight = 720f;

        // Keep track of furthest scroll right
        private float _furthestRight = 0f;

        
        private float _horizontalOffsetRatio = 0.35f; // 0.35 = 35% from left side of screen

        public Camera2D(Viewport viewport)
        {
            _viewport = viewport;
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0f)) *
                   Matrix.CreateTranslation(new Vector3(_viewport.Width / 2f, _viewport.Height / 2f, 0f));
        }

        public void LookAt(Vector2 target)
        {
            // Base desired camera 
            float targetX = target.X - (_viewport.Width * _horizontalOffsetRatio);
            float targetY = 0f; // lock vertical movement for Mario-style levels

            // follow target horizontally
            Vector2 desiredPosition = new(targetX, targetY);
            Position = Vector2.Lerp(Position, desiredPosition, _smoothSpeed);

            // world bounds
            Position.X = MathHelper.Clamp(Position.X, 0, _levelWidth - _viewport.Width);
            Position.Y = MathHelper.Clamp(Position.Y, 0, _levelHeight - _viewport.Height);

            // Prevent camera from scrolling left after moving right
            if (Position.X > _furthestRight)
                _furthestRight = Position.X;
            else
                Position = new Vector2(_furthestRight, Position.Y);
        }

        public void Move(Vector2 delta) => Position += delta;

        public void Reset(Vector2 startPosition)
        {
            Position = startPosition;
            _furthestRight = startPosition.X;
        }
    }
}
