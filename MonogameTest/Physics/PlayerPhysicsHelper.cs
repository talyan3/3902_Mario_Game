using Microsoft.Xna.Framework;
namespace MonogameTest;
public class PlayerPhysics
{
    public Vector2 velocity;
    public bool isGrounded;
    private float moveAcceleration = 1000f;
    private float maxMoveSpeed = 400f;
    private float groundFriction = 800f;
    private float airFriction = 100f;
    public float jumpStrength = -100f;
    
    public void ApplyHorizontalInput(int moveDir, float dt)
    {
        velocity.X += moveDir * moveAcceleration * dt;

        velocity.X = MathHelper.Clamp(velocity.X, -maxMoveSpeed, maxMoveSpeed);

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
        bool jumped = true;
        if (jumpPressed && isGrounded)
        {
            isGrounded = false;
        } else
        {
            jumped = false;
        }
        return jumped;
    }
}