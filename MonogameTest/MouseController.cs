using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace MonogameTest;

// Class storing mapping of mouse actions and their respective command(s)
public class MouseController : IController
{
    private Dictionary<MouseCombo, ICommand> mouseMap;

    public MouseController()
    {
        mouseMap = new Dictionary<MouseCombo, ICommand>();
    }

    // Return true if successfully added, return false if there already exists a mapping for the key combo
    public bool addMapping(MouseCombo click, ICommand command)
    {
        if (!mouseMap.ContainsKey(click))
        {
            mouseMap.Add(click, command);
            return true;
        }
        return false;
    }

    // Return true if successfully removed, return false if there does not exist a mapping for the key combo
    public bool removeMapping(MouseCombo click)
    {
        if (mouseMap.ContainsKey(click))
        {
            mouseMap.Remove(click);
            return true;
        }
        return false;
    }

    public ICommand getCommand(MouseCombo click)
    {
        ICommand toReturn = null;
        if (mouseMap.ContainsKey(click))
        {
            toReturn = mouseMap[click];
        }
        return toReturn;
    }


    // Checks the current state of the mouse and executes any commands that the state matches
    public void checkClicks()
    {

        foreach (MouseCombo combo in mouseMap.Keys)
        {
            if (combo.stateMatchesCombo()) mouseMap[combo].Execute();
        }
    }

}