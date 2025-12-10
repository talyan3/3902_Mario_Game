using Microsoft.Xna.Framework;
using System.Collections.Generic;
namespace MonogameTest;
public struct StaticCollisionResult
{
    public typeCollision Side;
    public Point MTV;
    public object TileRef;   
    public Rectangle TileRect;     
      
}

public static class StaticCollisionHandler
{
    // Handle a single ground/wall rectangle
        public static bool Handle(StaticSprite marioAny, Rectangle tileRect, out StaticCollisionResult result)
        {
            result = default;

            // --- Get current Mario position (for repositioning) ---
            Vector2 pos;
            if (marioAny is SmallMarioSprite sm)
            {
                pos = sm.Position;
            }
            else if (marioAny is BigMarioSprite bm)
            {
                pos = bm.Position;
            }
            else if (marioAny is FireMarioSprite fm)   
            {
                pos = fm.Position;
            }
            else
            {
                return false;
            }

            // --- USE BOUNDS instead of recomputing from Region ---
            Rectangle marioRect = marioAny.Bounds;
            if (marioRect == Rectangle.Empty)
                return false;

            var detector = new DetectCollisions();
            var side = detector.GetCollision(marioRect, tileRect, out Point mtv);
            if (side == typeCollision.None)
                return false;

            // --- Apply MTV to Mario's position ---
            Vector2 newPos = pos + mtv.ToVector2();

            if (marioAny is SmallMarioSprite smW)
                smW.Position = newPos;
            else if (marioAny is BigMarioSprite bmW)
                bmW.Position = newPos;
            else if (marioAny is FireMarioSprite fmW)   
                fmW.Position = newPos;

            result.Side = side;
            result.MTV = mtv;
            result.TileRect = tileRect;

            return true;
        }

    // iterate many tiles; stop on first collision handled
    public static bool HandleMany(StaticSprite marioAny, IEnumerable<Rectangle> solidTiles, out StaticCollisionResult result)
    {
        foreach (var rect in solidTiles)
            if (Handle(marioAny, rect, out result))
                return true;

        result = default;
        return false;
    }

    public static bool HandleMany(
    StaticSprite marioAny,
    IEnumerable<Tile> tiles,
    out StaticCollisionResult result,
    out Tile hitTile)
{
    foreach (var tile in tiles)
    {
        if (Handle(marioAny, tile.Bounds, out var tmp))
        {
            tmp.TileRef  = tile;
            tmp.TileRect = tile.Bounds;

            result  = tmp;
            hitTile = tile;
            return true;
        }
    }

    result  = default;
    hitTile = default!;
    return false;
}
}