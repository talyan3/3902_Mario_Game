using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest
{
    public class Tile
    {
        public Texture2D Texture { get; }
        public Rectangle SourceRect { get; }
        public Vector2 Position { get; }

        public Rectangle Bounds =>
            new Rectangle((int)Position.X, (int)Position.Y, SourceRect.Width, SourceRect.Height);

        public Tile(Texture2D texture, Rectangle sourceRect, Vector2 position)
        {
            Texture = texture;
            SourceRect = sourceRect;
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRect, Color.White);
        }
    }
}
