using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 
namespace MonogameTest;

public class PowerupCoinState : PowerupState
{
    public PowerupCoinState(PowerupInstance p) : base(p) {}

    public override void Enter() {}

    public override void Update(GameTime gameTime)
    {
        // coin does nothing
    }
}