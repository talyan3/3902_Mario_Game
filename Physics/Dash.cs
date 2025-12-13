using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace MonogameTest;

public class DashComponent
{
    // Dash properties
    public Vector2 position;
    public float DashDistance { get; set; } = 48f;
    public float Cooldown { get; set; } = 2f;
    public float CurrentCooldown { get; private set; } = 0f;
    public bool IsDashing { get; private set; } = false;
    public bool CanDash => CurrentCooldown <= 0;
    
    public DashComponent()
    {
        position = Vector2.Zero;
    }
    
    public void Update(GameTime gameTime, Vector2 currentPosition)
    {
        position = currentPosition;
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Update cooldown timer
        if (CurrentCooldown > 0)
        {
            CurrentCooldown -= deltaTime;
            if (CurrentCooldown < 0)
                CurrentCooldown = 0;
        }
        var kb = Keyboard.GetState();
        bool spacePressed = kb.IsKeyDown(Keys.Space);
        if (spacePressed && CanDash)
        {
            CurrentCooldown = Cooldown;
            IsDashing = true;
            if (isLeftDash(kb)){
            position.X -= DashDistance;
            }
            else
            {
                position.X += DashDistance;
            }
        }
    }
    


    public bool isLeftDash(KeyboardState keyboard)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            return true;
        }
        return false;
    }
}