using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.Snail
{
    public class Snail
    {
        private readonly SnailSprite _sprite;
        private readonly SnailBehavior _behavior;
        private readonly Texture2D _texture1;
        private readonly Texture2D _texture2;

        public Vector2 Position;
        public bool Active => Game1.ChristmasMode; // Only exists in Christmas mode

        public Snail(Texture2D t1, Texture2D t2, Vector2 startPosition)
        {
            _texture1 = t1;
            _texture2 = t2;

            Position = startPosition;
            _sprite = new SnailSprite(_texture1, _texture2);
            _behavior = new SnailBehavior();
        }

        public void Update(GameTime gameTime, Vector2 marioPos)
        {
            if (!Active) return;

            Position = _behavior.UpdateSnail(Position, marioPos, (float)gameTime.ElapsedGameTime.TotalSeconds);
            _sprite.Update(gameTime, Position, marioPos);
        }

        public void Draw(SpriteBatch sb)
        {
            if (!Active) return;
            _sprite.Draw(sb, Position);
        }
    }
}
