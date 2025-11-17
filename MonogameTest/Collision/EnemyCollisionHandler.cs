using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;

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
        private static EnemyCollision NumberSt => NumberLoad.Numbers.EnemyCollision;
        public static bool Handle(object enemyAny, Rectangle tileRect, out EnemyCollisionResult result)
        {
            result = default;

            Vector2 pos;
            Rectangle enemyRect;

            if (enemyAny is moveGoom goom)
            {
                pos = goom.Position;
                enemyRect = goom.Bounds;
                enemyRect.Inflate(NumberSt.EnemyInflateX, 0);   // Shrinks width by 8 total, height unchanged
            }
            else if (enemyAny is moveKoop koop)
            {
                pos = koop.Position;
                enemyRect = koop.Bounds;
                enemyRect.Inflate(NumberSt.EnemyInflateX, 0);   // Shrinks width by 8 total, height unchanged
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


            result.Side = side;
            result.MTV = mtv;
            result.TileRect = tileRect;
            result.Grounded = (side == typeCollision.Top && mtv.Y < 0);
            result.BonkedHead = (side == typeCollision.Bottom && mtv.Y > 0);
            result.HitWall = (side == typeCollision.Left || side == typeCollision.Right);
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
                    tmp.TileRef = t;
                    tmp.TileRect = t.Bounds;
                    result = tmp;
                    hitTile = t;
                    return true;
                }
            }
            result = default;
            hitTile = default!;
            return false;
        }

  
        private static Rectangle FeetRect(Rectangle r)           
            => new Rectangle(r.X, r.Bottom - NumberSt.FeetHeight, r.Width, NumberSt.FeetHeight);                 

        private static void HandleMarioVsEnemiesCore(                       
            Rectangle marioBounds,                                         
            Action bounce,                                                   
            Action onHit,                                                    
            IList<object> enemies)                                          
        {
            var feet = FeetRect(marioBounds);

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                var enemy = enemies[i];

                Rectangle enemyRect;
                bool alive;

                if (enemy is moveGoom goom)
                {
                    enemyRect = goom.Bounds;
                    alive = goom.IsAlive;
                    enemyRect.Inflate(NumberSt.StompInflateX, 0); 
                }
                else if (enemy is moveKoop koop)
                {
                    enemyRect = koop.Bounds;
                    alive = koop.IsAlive;
                    enemyRect.Inflate(NumberSt.StompInflateX, 0); 
                }
                else continue;

                if (!alive) continue;
                if (enemyRect == Rectangle.Empty) continue;
                if (!marioBounds.Intersects(enemyRect)) continue;

                
                bool stomp = feet.Intersects(enemyRect) &&
                             (marioBounds.Bottom <= enemyRect.Top + NumberSt.StompThreshold);

                if (stomp)
                {
                    if (enemy is moveGoom g)
                    {
                        g.IsAlive = false;
                    }
                    else if (enemy is moveKoop k) k.IsAlive = false;

                    bounce?.Invoke();
                    continue;
                }

                onHit?.Invoke();
            }
        }

        
        public static void HandleMarioEnemyCollision(                       
            SmallMarioSprite mario, IList<object> enemies, Action restart)   
        {
            HandleMarioVsEnemiesCore(
                marioBounds: mario.Bounds,
                bounce: () => mario.Bounce(NumberSt.SmallMarioBounce),
                onHit: restart,
                enemies: enemies
            );
        }

      
        public static void HandleMarioEnemyCollision(                       
            BigMarioSprite mario, IList<object> enemies, Action onBigHit)    
        {
            HandleMarioVsEnemiesCore(
                marioBounds: mario.Bounds,
                bounce: () => mario.Bounce(NumberSt.BigMarioBounce),
                onHit: onBigHit,   
                enemies: enemies
            );
        }
    }
}