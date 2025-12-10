using Microsoft.Xna.Framework;
using MonogameTest.Managers;

namespace MonogameTest.Snail
{
    public class SnailBehavior
    {
        private const float Speed = 20f; // Slow but relentless

        private float _groundLevel = 0f;

        public void setGround(Vector2 marioPos)
        {
            _groundLevel = marioPos.Y;
        }

        public Vector2 UpdateSnail(Vector2 snailPos, Vector2 marioPos, float dt, Snail snail)
        {
            float speed = 20f;

            // Move only horizontally toward Mario
            if (marioPos.X > snailPos.X)
                snailPos.X += speed * dt;
            else if (marioPos.X < snailPos.X)
                snailPos.X -= speed * dt;

            // snail can fly if mario is in the air
            if (marioPos.Y < _groundLevel)
            {
                if (snailPos.Y > marioPos.Y)
                    snailPos.Y -= speed * dt;   // move UP toward Mario
                else if (snailPos.Y < marioPos.Y)
                    snailPos.Y += speed * dt;   // move DOWN toward Mario

                snail.SetFlying(true);
            }
            else
            {
                if (snailPos.Y > _groundLevel)
                    snailPos.Y -= speed * dt;   // move UP toward ground
                else if (snailPos.Y < _groundLevel)
                    snailPos.Y += speed * dt;   // move DOWN toward ground
                else
                    snail.SetFlying(false);     // landed exactly on ground
            }

            return snailPos;
        }

    }
}