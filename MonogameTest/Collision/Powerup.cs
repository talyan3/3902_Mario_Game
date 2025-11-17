using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public abstract class Powerup
{
    public Vector2 Position;
    public bool IsAlive = true;

    public abstract Rectangle Bounds { get; }
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);
}