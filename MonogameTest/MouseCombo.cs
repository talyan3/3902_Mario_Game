using Microsoft.Xna.Framework.Input;
using System.Windows.Input;
using Microsoft.Xna.Framework;

namespace MonogameTest;

public enum MouseAction
{
    LEFT_CLICK,
    RIGHT_CLICK,
    MIDDLE_CLICK,
    SCROLL,
    CURSOR_MOVE
}

// Combinations of mouse clicks / locations that can be used for certain commands. Includes a BoundingBox specifying a boxed location you can click in (optional), and a mouse action
public class MouseCombo
{
    public MouseAction Action { get; }
    public BoundingBox BoundingBox { get; }
    private bool IsBoundedByBox { get; }

    private MouseCombo() { }

    public MouseCombo(MouseAction action)
    {
        Action = action;
        IsBoundedByBox = false;
        BoundingBox = new BoundingBox();
    }

    public MouseCombo(MouseAction action, Location boundingBoxStart, Location boundingBoxEnd)
    {
        Action = action;
        IsBoundedByBox = true;
        BoundingBox = new BoundingBox(boundingBoxStart, boundingBoxEnd);
    }

    public MouseCombo(MouseAction action, BoundingBox boundingBox)
    {
        Action = action;
        IsBoundedByBox = true;
        BoundingBox = boundingBox;
    }

    // Checks if the mouse action falls within the BoundingBox if there is one, and if the click matches
    public bool stateMatchesCombo()
    {
        Point coursorLocation = Mouse.GetState().Position;
        ButtonState on = ButtonState.Pressed;
        Location loc = new Location(coursorLocation.X, coursorLocation.Y);
        bool toReturn = Mouse.GetState().LeftButton == on && Action == MouseAction.LEFT_CLICK;
        toReturn = toReturn || Mouse.GetState().RightButton == on && Action == MouseAction.RIGHT_CLICK;
        toReturn = toReturn || Mouse.GetState().MiddleButton == on && Action == MouseAction.MIDDLE_CLICK;
        return toReturn && (!IsBoundedByBox || BoundingBox.locationIsWithinBox(loc));
    }

    public override bool Equals(object obj)
    {
        if (obj is MouseCombo)
        {
            MouseCombo compare = (MouseCombo)obj;
            if (compare.Action.Equals(Action) && compare.BoundingBox.Equals(BoundingBox) && compare.IsBoundedByBox.Equals(IsBoundedByBox)) return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        int res = 23;
        return 37 * res + (int)Action + BoundingBox.GetHashCode() + (IsBoundedByBox ? 0 : 1);
    }
}