using Microsoft.Xna.Framework;

public class PlayerPhysics
{
    public Vector2 velocity;
    public bool isGrounded;
    private float moveAcceleration = 1000f;
    private float maxMoveSpeed = 400f;
    private float groundFriction = 800f;
    private float airFriction = 100f;
    public float jumpStrength = -300f;
    
    public void ApplyHorizontalInput(int moveDir, float dt)
    {
        // Acceleration
        velocity.X += moveDir * moveAcceleration * dt;

        // Clamp speed
        velocity.X = MathHelper.Clamp(velocity.X, -maxMoveSpeed, maxMoveSpeed);

        // Friction
        float friction = isGrounded ? groundFriction : airFriction;

        if (moveDir == 0)
        {
            if (velocity.X > 0)
            {
                velocity.X -= friction * dt;
                if (velocity.X < 0) velocity.X = 0;
            }
            else if (velocity.X < 0)
            {
                velocity.X += friction * dt;
                if (velocity.X > 0) velocity.X = 0;
            }
        }
    }
    public bool TryJump(bool jumpPressed)
    {
        if (jumpPressed && isGrounded)
        {
            velocity.Y = jumpStrength;
            isGrounded = false;
            return true; // signal that a jump occurred
        }
        return false;
    }
}
