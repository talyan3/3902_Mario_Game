using Microsoft.Xna.Framework;
using System;

public interface ICollidable
{
    Rectangle BoundingBox { get; }

    void HandleCollision(ICollidable other, typeCollision side);
}


