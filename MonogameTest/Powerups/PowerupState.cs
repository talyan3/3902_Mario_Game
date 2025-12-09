using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 
namespace MonogameTest;

public abstract class PowerupState
{
    protected PowerupInstance _powerup;

    public PowerupState(PowerupInstance p) => _powerup = p;

    public abstract void Enter();
    public abstract void Update(GameTime gameTime);
}