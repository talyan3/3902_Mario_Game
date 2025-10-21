using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MonogameTest
{
    public class Camera2D
    {
        private readonly Viewport _viewport;
        public Vector2 Position { get; private set; } = Vector2.Zero;

        public Camera2D(Microsoft.Xna.Framework.Graphics.Viewport viewport)
        {
            _viewport = viewport;
        }

        public Matrix GetViewMatrix()
        {
            // Translate world so that camera position appears centered
            return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0f));
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
