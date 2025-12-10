using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonogameTest
{
    public class Fireball
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public bool IsAlive = true;

        private Texture2D _texture;
        private Rectangle _src;

        private const float Gravity = 600f;
        private const float BounceVelocity = -250f;

        public Rectangle Bounds =>
            new Rectangle((int)Position.X, (int)Position.Y, 16, 16);

        public Fireball(Texture2D t, Rectangle s, Vector2 pos, bool right)
        {
            _texture = t;
            _src = s;
            Position = pos;
            Velocity = new Vector2(right ? 250f : -250f, -50f);
        }

        public void Update(GameTime time, List<Tile> tiles)
        {
            if (!IsAlive) return;

            float dt = (float)time.ElapsedGameTime.TotalSeconds;

            Velocity.Y += Gravity * dt;
            Position += Velocity * dt;

            foreach (var t in tiles)
            {
                if (!Bounds.Intersects(t.Bounds)) continue;

                if (Position.Y < t.Bounds.Top)
                {
                    Velocity.Y = BounceVelocity;
                    Position = new Vector2(Position.X, t.Bounds.Top - 16);
                    return;
                }

                IsAlive = false;
                return;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (IsAlive)
                sb.Draw(_texture, Position, _src, Color.White);
        }
    }
}