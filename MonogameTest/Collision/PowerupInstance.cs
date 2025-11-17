using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public class PowerupInstance : Powerup
{
    public PowerupType Type;
    public Texture2D Texture;
    public Rectangle SourceRect;
    private PowerupState _state;

    public override Rectangle Bounds =>
        new Rectangle((int)Position.X, (int)Position.Y, SourceRect.Width, SourceRect.Height);

    public PowerupInstance(PowerupType type, Texture2D tex, Rectangle source)
    {
        Type = type;
        Texture = tex;
        SourceRect = source;
    }

    public void SetState(PowerupState state)
    {
        _state = state;
        state.Enter();
    }

    public override void Update(GameTime gameTime)
    {
        _state?.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (IsAlive)
            spriteBatch.Draw(Texture, Position, SourceRect, Color.White);
    }
}