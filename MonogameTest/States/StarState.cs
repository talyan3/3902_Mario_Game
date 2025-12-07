using Microsoft.Xna.Framework;

namespace MonogameTest
{
    public class StarState : PlayerState
    {
        private float starDuration = 8f;  
        private float flashTimer = 0f;   
        private float flashSpeed = 0.1f;  // NOTE: add as part of physics?
        private bool flashToggle = false;

        private float originalMaxSpeed;
        private float originalAcceleration;

        public StarState(PlayerMario player) : base(player) { }

        public override void Enter()
        {
            // change speed
            originalMaxSpeed = player.PhysicsP.maxMoveSpeed;
            originalAcceleration = player.PhysicsP.moveAcceleration;

            // change physics
            player.PhysicsP.maxMoveSpeed *= 1.8f;
            player.PhysicsP.moveAcceleration *= 1.8f;

            // change tint for flashing
            player.animPlayer.TintColor = Color.Yellow;

            player.animPlayer.Play(player.animIdle);
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            starDuration -= dt;

            // handle flashing effects
            flashTimer += dt;
            if (flashTimer >= flashSpeed)
            {
                flashTimer = 0f;
                flashToggle = !flashToggle;
                player.animPlayer.TintColor = flashToggle ? Color.Yellow : Color.White;
            }

            //NOTE: check if all this code is somewhere else and can be condensed?
            player.Input.Update();
            int moveDir = player.Input.GetMoveDirection();

            if (moveDir != 0)
                player.FacingRight = moveDir > 0;

            player.PhysicsP.ApplyHorizontalInput(moveDir, dt);
            player.Physics.velocity.X = player.PhysicsP.velocity.X;

            if (player.Input.jumpPressed && player.Physics.isGrounded)
            {
                player.ChangeState(new JumpState(player));
                return;
            }

            // end of star state
            if (starDuration <= 0)
            {
                player.ChangeState(new IdleState(player));
                return;
            }
        }

        //reset everything
        public override void Exit()
        {
            player.PhysicsP.maxMoveSpeed = originalMaxSpeed;
            player.PhysicsP.moveAcceleration = originalAcceleration;

            player.animPlayer.TintColor = Color.White;

        }
    }
}
