using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonogameTest;

public abstract class StaticSprite : ISprite
{

    public TextureRegion Region { get; set; }
    public abstract void Draw(SpriteBatch spriteBatch, Vector2 position);
    public abstract void Update(GameTime gameTime);
}