using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest
{
    public class Tile
    {
        public Texture2D Texture { get; }
        public Rectangle SourceRect { get; }
        public Vector2 Position { get;}

        public bool IsSolid =>
    TileName == "Ground" ||
    TileName == "Brick" ||
    TileName == "Question" ||
    TileName == "PipeTopLeft" ||
    TileName == "PipeTopRight" ||
    TileName == "PipeBodyLeft" ||
    TileName == "PipeBodyRight" ||
    TileName == "Stair";

        public Rectangle Bounds =>
            new Rectangle((int)Position.X, (int)Position.Y, SourceRect.Width, SourceRect.Height);
        public bool IsActive { get; set; } = true;
        public int Gid { get; set;}        
        public string TileName { get; }
        public Tile(Texture2D texture, Rectangle sourceRect, Vector2 position)
        {
            Texture = texture;
            SourceRect = sourceRect;
            Position = position;
            Gid = 0;
            TileName = "Unlabeled";
        }
        public Tile(Texture2D texture, Rectangle sourceRect, Vector2 position, int gid, string tileName)
            : this(texture, sourceRect, position)
        {
            Gid = gid;
            TileName = tileName;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRect, Color.White);
        }
    }
}
