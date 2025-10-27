using Microsoft.Xna.Framework;
using System;
public enum typeCollision
{
    None,
    Top,
    Bottom,
    Left,
    Right
}

public class DetectCollisions
{
    Rectangle pRect;
    Rectangle sRect;

    int xValue;
    int yValue;


    public typeCollision GetCollision(Rectangle pRect, Rectangle sRect, out Point mtv)
    {
        mtv = Point.Zero;

        if (pRect.Intersects(sRect))
        {
            xValue = pRect.X - sRect.X;
            yValue = pRect.Y - sRect.Y;

            //the length of the overlap
            int overlapLeft = pRect.Right - sRect.Left;
            int overlapRight = sRect.Right - pRect.Left;
            int overlapTop = pRect.Bottom - sRect.Top;
            int overlapBottom = sRect.Bottom - pRect.Top;

            int minHoriz = (overlapLeft < overlapRight) ? overlapLeft : overlapRight;
int minVert  = (overlapTop  < overlapBottom) ? overlapTop  : overlapBottom;

if (minHoriz < minVert) // horizontal collision
{
    if (overlapLeft < overlapRight)
    {
        mtv = new Point(-overlapLeft, 0);
        return typeCollision.Left;
    }
    else
    {
        mtv = new Point(overlapRight, 0);
        return typeCollision.Right;
    }
}
else // vertical collision
{
    if (overlapTop < overlapBottom)
    {
        mtv = new Point(0, -overlapTop);
        return typeCollision.Top;
    }
    else
    {
        mtv = new Point(0, overlapBottom);
        return typeCollision.Bottom;
    }
}

        }

        return typeCollision.None; //no collision
    }
}