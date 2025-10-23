using Microsoft.Xna.Framework;

namespace MonogameTest
{
    public interface ICamera
    {
        Matrix GetViewMatrix();
        void LookAt(Vector2 target);
        void Move(Vector2 delta);
        void Reset(Vector2 startPosition);
    }
}

