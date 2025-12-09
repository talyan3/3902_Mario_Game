using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonogameTest;

public static class PowerupFactory
{
    public static PowerupInstance Create(PowerupType type, Texture2D sheet, Vector2 pos)
    {
        Rectangle source = type switch
        {
            PowerupType.Mushroom   => new Rectangle(0, 0, 16, 16),   // RED mushroom
            PowerupType.GreenMushroom => new Rectangle(16, 0, 16, 16), // if you add 1UP later
            PowerupType.FireFlower => new Rectangle(32, 0, 16, 16),
            PowerupType.Star       => new Rectangle(48, 0, 16, 16),
            PowerupType.Coin       => new Rectangle(64, 0, 16, 16),
            _ => new Rectangle(0, 0, 16, 16)
        };

        var p = new PowerupInstance(type, sheet, source);
        p.Position = pos;

        p.SetState(type switch
        {
            PowerupType.Mushroom     => new PowerupMushroomState(p),
            PowerupType.FireFlower   => new PowerupFireFlowerState(p),
            PowerupType.Star         => new PowerupStarState(p),
            PowerupType.Coin         => new PowerupCoinState(p),
            _ => new PowerupMushroomState(p)
        });

        return p;
    }
}