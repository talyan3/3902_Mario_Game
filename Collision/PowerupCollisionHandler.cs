using Microsoft.Xna.Framework;

namespace MonogameTest;

public static class PowerupCollisionHandler
{
    public static bool CheckMarioPowerupCollision(StaticSprite mario, PowerupInstance p)
    {
        return p.IsAlive && mario.Bounds.Intersects(p.Bounds);
    }
}