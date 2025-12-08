using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonogameTest;

public abstract class StaticSprite : ISprite
{
    public TextureRegion Region { get; set; }

    public virtual Vector2 Position { get; set; } = Vector2.Zero;

    public virtual Rectangle Bounds
    {
        get
        {
            if (Region == null)
                return Rectangle.Empty;

            return new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                Region.Width,
                Region.Height
            );
        }
    }

    public abstract void Draw(SpriteBatch spriteBatch, Vector2 position);
    public abstract void Update(GameTime gameTime);
}