using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class PlayerMario
{
    public PhysicsTest Physics;
    public AnimationPlayer animPlayer;
    public PlayerPhysics PhysicsP = new PlayerPhysics();

    public Animation animIdle;
    public Animation animRun;
    public Animation animJump;

    public InputController Input;
    public PlayerState currState;

    public bool isFacingRight = true;

    private float moveAcceleration = 1000f;
    private float maxMoveSpeed = 400f;
    private float groundFriction = 800f;
    private float airFriction = 100f;
    private float jumpStrength = -300f;

    private float scale = 0.03f;

    public bool FacingRight = true;
    public SoundManager SM;
    public PlayerMario()
    {
        Physics = new PhysicsTest();
        Physics.position = new Vector2(100, 100);

        animPlayer = new AnimationPlayer();
        Input = new InputController();
    }

    public void LoadContent(ContentManager content, SoundManager soundManager)
    {
        SM = soundManager;
        animIdle = new Animation(content.Load<Texture2D>("Sprites/Entity/small-mario-final"),30,16,1, 0.15f, 8);
        animRun  = new Animation(content.Load<Texture2D>("Sprites/Entity/small-mario-final"), 30,16,3,0.10f,9);
        animJump = new Animation(content.Load<Texture2D>("Sprites/Entity/small-mario-final"),30,16,1,0.20f,13);

        ChangeState(new IdleState(this));
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

        // ----- INPUT -----
        Input.Update();
        int moveDir = Input.GetMoveDirection();   // -1, 0, or +1

        if (moveDir != 0)
        {
            FacingRight = moveDir > 0;
        }
        // Horizontal movement (input only sets velocity)
        PhysicsP.ApplyHorizontalInput(moveDir, dt);

        if (PhysicsP.TryJump(Input.jumpPressed))
        {
            ChangeState(new JumpState(this));
        }
        // Physics update
        Physics.Update(gameTime);

        // PLATFORM COLLISION
        Rectangle rect = new Rectangle(
            (int)Physics.position.X,
            (int)Physics.position.Y,
            (int)(animPlayer.CurrentAnimation.FWidth * scale),
            (int)(animPlayer.CurrentAnimation.FHeight * scale)
        );

        if (rect.Intersects(platformRect) && Physics.velocity.Y >= 0)
        {
            Physics.position.Y = platformRect.Top - rect.Height;
            Physics.velocity.Y = 0;
            Physics.isGrounded = true;
        }

        // Update state machine
        currState.Update(gameTime);

        // Update facing direction
        if (Physics.velocity.X > 0) FacingRight = true;
        if (Physics.velocity.X < 0) FacingRight = false;

        // Update animation
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
