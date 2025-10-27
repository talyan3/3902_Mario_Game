using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows.Input;
using Microsoft.Xna.Framework.Input;

namespace MonogameTest;

// Class used to store KeyCombo and their respective command(s)
public class KeyboardController : IController
{
    private Dictionary<KeyCombo, ICommand> keyMap;

    public KeyboardController()
    {
        keyMap = new Dictionary<KeyCombo, ICommand>(new KeyboardKeyComparator());
    }

    // Return true if successfully added, return false if there already exists a mapping for the key combo
    public bool addMapping(KeyCombo key, ICommand command)
    {
        if (!keyMap.ContainsKey(key))
        {
            keyMap.Add(key, command);
            return true;
        }
        return false;
    }

    // Return true if successfully removed, return false if there does not exist a mapping for the key combo
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
        ICommand toReturn = null;
        if (keyMap.ContainsKey(key))
        {
            toReturn = keyMap[key];
        }
        return toReturn;
    }

    // Checks the current state of the game and what keys are pressed, and executes any matching states (KeyCombo)
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