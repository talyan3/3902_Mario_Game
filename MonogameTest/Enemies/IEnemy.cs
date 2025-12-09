using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IEnemy : ICollidable
{
    Vector2 Position { get; set; }
    bool IsAlive { get; set; }

    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch, Vector2 position);

    void ReverseDirection();
}
