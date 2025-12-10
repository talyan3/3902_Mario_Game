using Microsoft.Xna.Framework;

namespace MonogameTest;

public class Pipe
{
    public Rectangle Bounds;
    public Vector2 Destination;
    
    public Pipe(Rectangle bounds, Vector2 destination)
    {
        Bounds = bounds;
        Destination = destination;
    }
    
    public bool CheckCollision(Rectangle playerBounds)
    {
        return Bounds.Intersects(playerBounds);
    }
}

//47 and 48 times 16 this is x position 752 and 768
//and a y position 8 times 16 `which is -128 for enter pipe

//teleport to x at 2384 and y -272