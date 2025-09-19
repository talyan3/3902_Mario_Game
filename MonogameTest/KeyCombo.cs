using System;
using Microsoft.Xna.Framework.Input;

namespace MonogameTest;

public enum KeyAction
{
    PRESS,
    HOLD,
    RELEASE
}

public class KeyCombo
{
    private Keys key;
    private KeyAction action;
    public KeyCombo(Keys key, KeyAction action)
    {
        this.key = key;
        this.action = action;
    }

    public Keys getKey()
    {
        return key;
    }

    public KeyAction getAction()
    {
        return action;
    }

    public override bool Equals(object obj)
    {
        if (obj is KeyCombo)
        {
            KeyCombo compare = (KeyCombo)obj;
            if (compare.getKey().Equals(key) && compare.getAction().Equals(action)) return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        int res = 31;
        return res*37 + ((int)key) + ((int)action);
    }
}