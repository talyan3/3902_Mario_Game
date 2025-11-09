using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public class BackgroundElement
{
    public Texture2D Texture { get; }
    public Vector2 Position { get; }
    public Color Tint { get; }

    public BackgroundElement(Texture2D texture, Vector2 position, Color? tint = null)
    {
        Texture = texture;
        Position = position;
        Tint = tint ?? Color.White;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Tint);
    }
}
