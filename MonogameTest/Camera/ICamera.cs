using Microsoft.Xna.Framework;

namespace MonogameTest
{
    public interface ICamera
    {
        Matrix GetViewMatrix();
        void LookAt(Vector2 target);
        void Reset(Vector2 startPosition);
        float LeftEdge { get; }
    }
}
