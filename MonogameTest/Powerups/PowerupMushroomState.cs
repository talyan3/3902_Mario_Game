using Microsoft.Xna.Framework;

namespace MonogameTest;

public class PowerupMushroomState : PowerupState
{
    private float _speed = 20f;

    public PowerupMushroomState(PowerupInstance p) : base(p) {}

    public override void Enter() { }

    public override void Update(GameTime gameTime)
    {
        _powerup.Position.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}