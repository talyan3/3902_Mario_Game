using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 
namespace MonogameTest;

public class PowerupFireFlowerState : PowerupState
{
    public PowerupFireFlowerState(PowerupInstance p) : base(p) {}

    public override void Enter() {}

    public override void Update(GameTime gameTime)
    {
        // flower does not move
    }
}