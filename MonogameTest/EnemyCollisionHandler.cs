using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonogameTest
{
    public struct EnemyCollisionResult
    {
        public typeCollision Side;   
        public Point MTV;            
        public Rectangle TileRect;
        public object TileRef;   
         public bool Grounded;       
        public bool BonkedHead;      
        public bool HitWall;    
        
    }

    public static class EnemyCollisionHandler
    {
        public static bool Handle(object enemyAny, Rectangle tileRect, out EnemyCollisionResult result)
        {
            result = default;


            Vector2 pos;
            Rectangle enemyRect;

            if (enemyAny is moveGoom goom)
            {
                pos = goom.Position;
                enemyRect = goom.Bounds;
            }
            else if (enemyAny is moveKoop koop)
            {
                pos = koop.Position;
                enemyRect = koop.Bounds;
            }
            else
            {
                return false;
            }

            var detector = new DetectCollisions();
            var side = detector.GetCollision(enemyRect, tileRect, out Point mtv);
            if (side == typeCollision.None) return false;

            var newPos = pos + mtv.ToVector2();
            if (enemyAny is moveGoom goomW) goomW.Position = newPos;
            else if (enemyAny is moveKoop koopW) koopW.Position = newPos;

            result.Side     = side;
            result.MTV      = mtv;
            result.TileRect = tileRect;
            result.Grounded   = (side == typeCollision.Top    && mtv.Y < 0);
            result.BonkedHead = (side == typeCollision.Bottom && mtv.Y > 0);
            result.HitWall    = (side == typeCollision.Left || side == typeCollision.Right);
            return true;
        }

        public static bool HandleMany(object enemyAny, IEnumerable<Rectangle> solidTiles, out EnemyCollisionResult result)
        {
            foreach (var rect in solidTiles)
            {
                if (Handle(enemyAny, rect, out result))
                    return true;
            }
            result = default;
            return false;
        }

        public static bool HandleMany(
            object enemyAny,
            IEnumerable<Tile> tiles,
            out EnemyCollisionResult result,
            out Tile hitTile)
        {
            foreach (var t in tiles)
            {
                if (Handle(enemyAny, t.Bounds, out var tmp))
                {
                    tmp.TileRef  = t;
                    tmp.TileRect = t.Bounds;
                    result  = tmp;
                    hitTile = t;
                    return true;
                }
            }
            result  = default;
            hitTile = default!;
            return false;
        }


    }
}
