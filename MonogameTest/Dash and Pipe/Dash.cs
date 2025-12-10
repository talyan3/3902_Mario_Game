using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace MonogameTest;

public class DashComponent
{
    // Dash properties
    public Vector2 position;
    public float DashDistance { get; set; } = 100f;
    public float Cooldown { get; set; } = 0.8f;
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
            Console.WriteLine("DASH!");
            CurrentCooldown = Cooldown;
            IsDashing = true;
            if (isLeftDash(kb)){
                Console.WriteLine("DASH Left!");
            position.X -= DashDistance;
            }
            else
            {
                Console.WriteLine("DASH Right!");
                position.X += DashDistance;
            }
        }
    }
    
    // public bool TryDash(Vector2 direction)
    // {
    //     if (CanDash && direction != Vector2.Zero)
    //     {
    //         IsDashing = true;
    //         CurrentCooldown = Cooldown;
    //         return true;
    //     }
    //     return false;
    // }

    public bool isLeftDash(KeyboardState keyboard)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            return true;
        }
        return false;
    }
}

