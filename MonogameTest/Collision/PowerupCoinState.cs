using Microsoft.Xna.Framework;

namespace MonogameTest;

public class PowerupCoinState : PowerupState
{
    private float _riseSpeed = 40f;   
    private float _lifeTime = 0.4f;   
    private float _elapsed = 0f;

    public PowerupCoinState(PowerupInstance p) : base(p) { }

    public override void Enter()
    {
        _elapsed = 0f;
    }

    public override void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _elapsed += dt;

       
        _powerup.Position.Y -= _riseSpeed * dt;

        
        if (_elapsed >= _lifeTime)
        {
            _powerup.IsAlive = false;
        }
    }
}
