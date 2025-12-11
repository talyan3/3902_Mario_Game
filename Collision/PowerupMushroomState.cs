using Microsoft.Xna.Framework;

namespace MonogameTest
{
    public class PowerupMushroomState : PowerupState
    {
        private float _speed = 40f;
        private float _gravity = 900f;

        public PowerupMushroomState(PowerupInstance p) : base(p) {}

        public override void Enter() {}

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Apply gravity
            _powerup.Velocity.Y += _gravity * dt;

            // Horizontal movement
            if (_powerup.Velocity.X == 0)
                _powerup.Velocity.X = _speed;

            // Apply movement
            _powerup.Position += _powerup.Velocity * dt;

            // Mark as falling until land collision resets it
            _powerup.IsFalling = true;
        }
    }
}