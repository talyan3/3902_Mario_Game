using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonogameTest;

public abstract class StaticSprite : ISprite
{

    public TextureRegion Region { get; set; }
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Scale { get; set; } = Vector2.One;

    public abstract void Draw(SpriteBatch spriteBatch, Vector2 position);
    public abstract void Update(GameTime gameTime);
}