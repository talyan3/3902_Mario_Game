using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Xna.Framework.Input;

namespace MonogameTest;

// Unified Input + Command Controller
public class KeyboardController : IController
{
    // -----------------------------
    // COMMAND SYSTEM
    // -----------------------------
    private Dictionary<KeyCombo, ICommand> keyMap;

    // -----------------------------
    // REAL-TIME INPUT STATE (OLD FILE 2)
    // -----------------------------
    public bool moveLeft { get; private set; }
    public bool moveRight { get; private set; }
    public bool jumpPressed { get; private set; }
    public bool jumpHeld { get; private set; }
    public bool crouch { get; private set; }
    public bool InputLocked { get; private set; } = false;


    private KeyboardState prevState;

    public KeyboardController()
    {
        keyMap = new Dictionary<KeyCombo, ICommand>(new KeyboardKeyComparator());
    }

    // -----------------------------
    // COMMAND MAPPING SYSTEM
    // -----------------------------
    public bool addMapping(KeyCombo key, ICommand command)
    {
        if (!keyMap.ContainsKey(key))
        {
            keyMap.Add(key, command);
            return true;
        }
        return false;
    }


    public bool removeMapping(KeyCombo key)
    {
        if (keyMap.ContainsKey(key))
        {
            keyMap.Remove(key);
            return true;
        }
        return false;
    }

    public ICommand getCommand(KeyCombo key)
    {
        return keyMap.ContainsKey(key) ? keyMap[key] : null;
    }

    // -----------------------------
    // REAL-TIME INPUT UPDATE
    // -----------------------------
    public void Update()
    {
        if (Game1.InputLocked)
        {
            moveLeft = false;
            moveRight = false;
            crouch = false;
            jumpHeld = false;
            jumpPressed = false;
            return;
        }


        KeyboardState state = Keyboard.GetState();

        moveLeft  = state.IsKeyDown(Keys.Left)  || state.IsKeyDown(Keys.F);
        moveRight = state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.H);
        crouch    = state.IsKeyDown(Keys.Down)  || state.IsKeyDown(Keys.G);

        bool currentJump  = state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.T);
        bool previousJump = prevState.IsKeyDown(Keys.Space) || prevState.IsKeyDown(Keys.T);

        jumpHeld    = currentJump;
        jumpPressed = currentJump && !previousJump;

        prevState = state;
    }

    public void LockInput()
    {
        InputLocked = true;
        moveLeft = false;
        moveRight = false;
        jumpHeld = false;
        jumpPressed = false;
        crouch = false;
    }

    public void UnlockInput()
    {
        InputLocked = false;
    }


    public int GetMoveDirection()
    {
        if (moveLeft && !moveRight) return -1;
        if (moveRight && !moveLeft) return +1;
        return 0;
    }

        public void ForceRight(bool state)
    {
        moveRight = state;
        moveLeft = false;
    }


    // -----------------------------
    // COMMAND EXECUTION SYSTEM
    // -----------------------------
    public void checkKeys()
    {
        foreach (Keys key in Keyboard.GetState().GetPressedKeys())
        {
            KeyCombo combo = new KeyCombo(key, KeyAction.PRESS);
            if (keyMap.ContainsKey(combo))
            {
                keyMap[combo].Execute();
            }
        }
    }
}

public class KeyboardKeyComparator : EqualityComparer<KeyCombo>
{
    public override bool Equals(KeyCombo x, KeyCombo y)
    {
        return x.Equals(y);
    }

    public override int GetHashCode([DisallowNull] KeyCombo obj)
    {
        return obj.GetHashCode();
    }
}
