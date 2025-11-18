using System.Data;
using Microsoft.Xna.Framework;

namespace MonogameTest;
public class Pipe
{
    Rectangle teleportZone;
    Vector2 teleportTarget;
    public Pipe(Rectangle zone, Vector2 target)
    {
        teleportZone = zone;
        teleportTarget = target;
    }
    //Need to fix for pipes that are not vertical. 
    public bool CheckTeleportable(Rectangle playerBounds, InputController input)
    {
        return teleportZone.Intersects(playerBounds) && input.crouch;
    }

    public void Teleport(ref Vector2 playerPosition)
    {
        playerPosition = teleportTarget;
    }
}
