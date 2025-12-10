using Microsoft.Xna.Framework;

namespace MonogameTest.Snail
{
    public class SnailBehavior
    {
        private const float Speed = 20f; // Slow but relentless

        public Vector2 UpdateSnail(Vector2 snailPos, Vector2 marioPos, float dt)
        {
            float speed = 20f;

            // Move only horizontally toward Mario
            if (marioPos.X > snailPos.X)
                snailPos.X += speed * dt;
            else if (marioPos.X < snailPos.X)
                snailPos.X -= speed * dt;

            return snailPos;
        }

    }
}