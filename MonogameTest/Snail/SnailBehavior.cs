using Microsoft.Xna.Framework;

namespace MonogameTest.Snail
{
    public class SnailBehavior
    {
        private const float Speed = 20f; // Slow but relentless

        public Vector2 UpdateSnail(Vector2 snailPos, Vector2 marioPos, float dt)
        {
            Vector2 dir = marioPos - snailPos;
            if (dir.LengthSquared() > 0.1f)
            {
                dir.Normalize();
                snailPos += dir * Speed * dt;
            }
            return snailPos;
        }
    }
}
