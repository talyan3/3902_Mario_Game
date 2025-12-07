using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest
{
    public class PlayerMario
    {
        public Physics Physics;
        public AnimationPlayer animPlayer;
        public PlayerPhysics PhysicsP;

        public Animation animIdle;
        public Animation animRun;
        public Animation animJump;

        // Star Mario animation
        public Animation animStar;

        public PlayerState currState;
        public bool FacingRight = true;
        public SoundManager SM;
        public KeyboardController Input;

        private float scale = 1f;

        public bool VictoryLock { get; private set; } = false;

        public PlayerMario()
        {
            Physics = new Physics();
            Physics.position = new Vector2(-100, 100);

            PhysicsP = new PlayerPhysics();
            animPlayer = new AnimationPlayer();
            Input = new KeyboardController();
        }

        public void LoadContent(ContentManager content, SoundManager soundManager, GraphicsDevice graphicsDevice)
        {
            SM = soundManager;

            animIdle = new Animation(
                Texture2D.FromFile(graphicsDevice, "small-mario-final.png"),
                30, 16, 1, 0.15f, 8
            );

            animRun = new Animation(
                Texture2D.FromFile(graphicsDevice, "small-mario-final.png"),
                30, 16, 3, 0.10f, 9
            );

            animJump = new Animation(
                Texture2D.FromFile(graphicsDevice, "small-mario-final.png"),
                30, 16, 1, 0.20f, 13
            );

            // Star animation
            animStar = new Animation(
                Texture2D.FromFile(graphicsDevice, "mario_star_bkgrd_remvd.png"),
                30, 16, 4, 0.08f, 0
            );

            ChangeState(new IdleState(this));
        }

        public void LockForVictory()
        {
            VictoryLock = true;
            Physics.velocity = Vector2.Zero;
            PhysicsP.velocity = Vector2.Zero;
        }

        public void ForceWalkRight(float dt)
        {
            PhysicsP.ApplyHorizontalInput(1, dt);
            Physics.velocity.X = PhysicsP.velocity.X;
        }

        public void ChangeState(PlayerState newState)
        {
            currState?.Exit();
            currState = newState;
            currState.Enter();
        }

        public void Update(GameTime gameTime, Rectangle platformRect)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!VictoryLock)
            {
                Input.Update();
                int moveDir = Input.GetMoveDirection();

                PhysicsP.ApplyHorizontalInput(moveDir, dt);
                Physics.velocity.X = PhysicsP.velocity.X;

                if (Input.jumpPressed && Physics.isGrounded)
                {
                    Physics.velocity.Y = PhysicsP.jumpStrength;
                    Physics.isGrounded = false;
                    ChangeState(new JumpState(this));
                }
            }

            Physics.Update(gameTime);

            currState.Update(gameTime);
            animPlayer.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animPlayer.Draw(
                spriteBatch,
                Physics.position,
                FacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                scale
            );
        }
    }
}
