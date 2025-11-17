using System.ComponentModel;
using Microsoft.Xna.Framework.Input;

public class InputController
{
    public bool moveLeft { get; private set; }
    public bool moveRight { get; private set; }
    public bool jumpPressed { get; private set; }
    public bool jumpHeld { get; private set; }
    public bool crouch { get; private set; }

    private KeyboardState prevState;
    public void Update()
    {
        KeyboardState state = Keyboard.GetState();

        moveLeft = state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.F);
        moveRight = state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.H);
        crouch = state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.G);
        bool currentJump = state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.T);
        bool previousJump = prevState.IsKeyDown(Keys.Space) || prevState.IsKeyDown(Keys.T);
        jumpHeld = currentJump;
        jumpPressed = currentJump && !previousJump;
        prevState = state;
    }
    public int GetMoveDirection()
    {
        bool left = moveLeft;   
        bool right = moveRight; 

        if (left && !right) return -1;
        if (right && !left) return +1;
        return 0;
    }

}