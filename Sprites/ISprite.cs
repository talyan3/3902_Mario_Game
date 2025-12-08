using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public interface ISprite
{
    public void Draw(SpriteBatch spriteBatch, Vector2 position);
    public void Update(GameTime gameTime);
    //bool IsAlive { get; set; }          // Used by enemies, mushrooms, etc.
    //Vector2 Position { get; set; }      // Used for movement and collision
}