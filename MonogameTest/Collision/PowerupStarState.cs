using System;
using Microsoft.Xna.Framework;

namespace MonogameTest;

public class PowerupStarState : PowerupState
{
    public PowerupStarState(PowerupInstance p) : base(p) {}

    public override void Enter() {}

    public override void Update(GameTime gameTime)
    {
        _powerup.Position.Y += (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 5) * 0.5f;
    }
}