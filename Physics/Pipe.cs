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