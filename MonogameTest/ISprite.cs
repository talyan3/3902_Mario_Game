using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public interface ISprite
{
    public Vector2 Position { get; set; }
    public Vector2 Scale { get; set; }
    public TextureRegion Region { get; set; }
    public void Draw(SpriteBatch spriteBatch, Vector2 position);
    public void Update(GameTime gameTime);
}