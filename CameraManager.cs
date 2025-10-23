using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest
{
    public class CameraManager : ICamera
    {
        private readonly Viewport _viewport;

        private Vector2 _position = Vector2.Zero;
        public Vector2 Position
        {
            get => _position;
            private set => _position = value;
        }

        private float _smoothSpeed = 0.15f;  // how smoothly camera follows target
        private float _levelWidth = TiledMapLoader.MapWidth * 16f;
        private float _levelHeight = TiledMapLoader.MapHeight * 16f;


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
      // NES: camera follows Mario, but keeps him slightly to the left of screen center
     float desiredX = target.X - (_viewport.Width * _horizontalOffsetRatio);

        // Smoothly interpolate to that position
      _position = Vector2.Lerp(_position, new Vector2(desiredX, 0), _smoothSpeed);

      // Clamp camera inside level bounds (never scroll left)
      _position.X = MathHelper.Clamp(_position.X, 0, _levelWidth - _viewport.Width);
      _position.Y = 0;

      // Track furthest right (prevents back-scrolling)
      if (_position.X > _furthestRight)
          _furthestRight = _position.X;
       else
          _position.X = _furthestRight;
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
