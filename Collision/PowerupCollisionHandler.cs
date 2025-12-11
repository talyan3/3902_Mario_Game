using Microsoft.Xna.Framework;

namespace MonogameTest
{
    public static class PowerupCollisionHandler
    {
        public static bool CheckMarioPowerupCollision(StaticSprite mario, PowerupInstance p)
        {
            return p.IsAlive && mario.Bounds.Intersects(p.Bounds);
        }

       
        //Tile Collision (Ground + Pipes + Bricks + Stairs + ? Blocks)
        public static void HandleTileCollision(PowerupInstance p, Tile tile)
        {
            if (!p.IsAlive) return;
            if (!tile.IsSolid) return;

            Rectangle pb = p.Bounds;
            Rectangle tb = tile.Bounds;

            if (!pb.Intersects(tb)) return;

            Vector2 pos = p.Position;

            // ---------- Vertical Collision ----------
            if (pb.Bottom > tb.Top && pb.Top < tb.Top && p.Velocity.Y > 0)
            {
                // Land on top of tile
                pos.Y = tb.Top - p.SourceRect.Height;
                p.Velocity.Y = 0;
                p.IsFalling = false;
                p.Position = pos;
                return;
            }

            // ---------- Horizontal Collision ----------
            if (pb.Right > tb.Left &&
                pb.Left < tb.Left &&
                p.Velocity.X > 0)
            {
                // Hit left side of tile → reverse direction
                pos.X = tb.Left - p.SourceRect.Width;
                p.Velocity.X *= -1;
            }
            else if (pb.Left < tb.Right &&
                     pb.Right > tb.Right &&
                     p.Velocity.X < 0)
            {
                // Hit right side of tile → reverse direction
                pos.X = tb.Right;
                p.Velocity.X *= -1;
            }

            p.Position = pos;
        }
    }
}