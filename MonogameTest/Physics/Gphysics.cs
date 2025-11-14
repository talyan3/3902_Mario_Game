using Microsoft.Xna.Framework;
namespace MonogameTest;
public class PhysicsTest
{
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    public bool isGrounded;

    public bool isFacingRight = true;

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Apply gravity
        if (!isGrounded)
            acceleration.Y = 1000f;
        else
            acceleration.Y = 0f;

        // Update velocity and position
        velocity += acceleration * dt;
        position += velocity * dt;

        // Simple ground collision example
        if (position.Y >= 400f) 
        {
            position.Y = 400f;
            velocity.Y = 0f;
            isGrounded = true;
        }
    }
}