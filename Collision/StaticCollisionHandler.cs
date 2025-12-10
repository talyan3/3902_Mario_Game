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

        Vector2 pos, scale;
        if (marioAny is SmallMarioSprite sm)
        {
            pos = sm.Position;
            scale = sm.Scale;
        }
        else if (marioAny is BigMarioSprite bm)
        {
            pos = bm.Position;
            scale = bm.Scale;
        }
        else
        {
            return false; 
        }

        var region = marioAny.Region; // current frame
        int w = (int)(region.Width  * scale.X);
        int h = (int)(region.Height * scale.Y);
        int left = (int)(pos.X - w / 2f);
        int top  = (int)(pos.Y - h);
        var marioRect = new Rectangle(left, top, w, h);

        var detector = new DetectCollisions();
        var side = detector.GetCollision(marioRect, tileRect, out Point mtv);
        /*if(Game1.InputLocked && (side == typeCollision.Left || side == typeCollision.Right))
        {
            return false;
        }*/
        if (side == typeCollision.None) return false;

        var newPos = pos + mtv.ToVector2();
        if (marioAny is SmallMarioSprite smW) smW.Position = newPos;
        else if (marioAny is BigMarioSprite bmW) bmW.Position = newPos;

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