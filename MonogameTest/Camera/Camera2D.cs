using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// USELESS?
namespace MonogameTest
{
    public class Camera2D
    {
        private readonly Viewport _viewport;
        public float Scale { get; set; } = 3f;
        public Vector2 Position { get; private set; } = Vector2.Zero;

        public Camera2D(Microsoft.Xna.Framework.Graphics.Viewport viewport)
        {
            _viewport = viewport;
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-Position, 0f)) *
                Matrix.CreateScale(Scale) *
                Matrix.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0f));
        }

        public void LookAt(Vector2 target)
        {
            Position = target;
        }

        public void Move(Vector2 delta)
        {
            Position += delta;
        }
    }
}
