using Microsoft.Xna.Framework;
namespace MonogameTest;
public class Physics
{
    private static readonly PhysicsNum physicsNum = NumberLoad.Numbers.GPhysics;
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    public bool isGrounded;


    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Apply gravity
        if (!isGrounded)
            acceleration.Y = physicsNum.Gravity;
        else
            acceleration.Y = 0f;

        // Update velocity and position
        velocity += acceleration * dt;
        position += velocity * dt;

        // Simple ground collision example
        if (position.Y >= physicsNum.GroundY) 
        {
            position.Y = physicsNum.GroundY;
            velocity.Y = 0f;
            isGrounded = true;
        }
    }
}