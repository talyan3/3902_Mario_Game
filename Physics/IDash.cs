using Microsoft.Xna.Framework;
public interface IDash
{
    Vector2 Position { get; set; }
    Rectangle BoundingBox { get; }
}