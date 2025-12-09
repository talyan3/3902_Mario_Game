using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class PowerupsCollisionHandler : ICollidable
{
    public Rectangle Bounds { get; set; }   // Required by ICollidable
    public bool IsActive { get; set; }      // Required by ICollidable
}