using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonogameTest.Sounds;
using MonogameTest;
using MonogameTest.Screens;
using MonogameTest.Managers;

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
        private static int stompCombo = 0;

        private static int GetStompScore()
        {
            stompCombo++;

            return stompCombo switch
            {
                1 => 100,
                2 => 200,
                3 => 400,
                4 => 800,
                5 => 1000,
                6 => 2000,
                7 => 4000,
                _ => 8000      // 8 or more
            };
        }
        public static void ResetStompCombo()
        {
            stompCombo = 0;
        }
        // ============================================
        // ENEMY VS TILES
        // ============================================

        public static bool Handle(object enemyAny, Rectangle tileRect, out EnemyCollisionResult result)
        {
            result = default;

            Vector2 pos;
            Rectangle enemyRect;

            if (enemyAny is moveGoom goom)
            {
                pos = goom.Position;
                enemyRect = goom.Bounds;
                enemyRect.Inflate(-5, 0);
            }
            else if (enemyAny is moveKoop koop)
            {
                pos = koop.Position;
                enemyRect = koop.Bounds;
                enemyRect.Inflate(-5, -0);
            }
            else
            {
                return false;
            }

            var detector = new DetectCollisions();
            var side = detector.GetCollision(enemyRect, tileRect, out Point mtv);
            if (side == typeCollision.None) return false;

            var newPos = pos + mtv.ToVector2();

            if (enemyAny is moveGoom g) g.Position = newPos;
            else if (enemyAny is moveKoop k) k.Position = newPos;

            result.Side = side;
            result.MTV = mtv;
            result.TileRect = tileRect;
            result.Grounded = (side == typeCollision.Top && mtv.Y < 0);
            result.BonkedHead = (side == typeCollision.Bottom && mtv.Y > 0);
            result.HitWall = (side == typeCollision.Left || side == typeCollision.Right);
            return true;
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

        // ============================================
        // SHELL VS GOOMBAS (INDEPENDENT OF MARIO)
        // ============================================

        private static void HandleShellVsGoombas(IList<object> enemies)
        {
            // Find a moving Koopa shell, if any
            moveKoop shell = null;
            foreach (var e in enemies)
            {
                if (e is moveKoop k && k.IsShellMoving && k.IsAlive)
                {
                    shell = k;
                    break;
                }
            }

            if (shell == null) return;

            var shellBounds = shell.Bounds;

            foreach (var e in enemies)
            {
                if (e is moveGoom g && g.IsAlive)
                {
                    var goomRect = g.Bounds;
                    goomRect.Inflate(-6, 0);

                    if (goomRect != Rectangle.Empty && shellBounds.Intersects(goomRect))
                    {
                        if (!Game1.ChristmasMode)
                        {
                            SoundManager.Instance.PlayEffect("stomp");
                        }
                        else
                        {
                            SoundManager.Instance.PlayEffect("xmasThud");
                        }
                        g.IsAlive = false;
                        ResetStompCombo();                            // Shell kills don’t chain
                        ScreenManager.Instance.AddScore(400); 
                    }
                }
            }
        }

        // ============================================
        // MARIO VS ENEMIES
        // ============================================

        private static Rectangle FeetRect(Rectangle r, int h = 4)
            => new Rectangle(r.X, r.Bottom - h, r.Width, h);

        private static void HandleMarioVsEnemiesCore(
            Rectangle marioBounds,
            float marioVelocity, 
            Action bounce,
            Action onHit,
            IList<object> enemies)
        {
            // First, resolve shell hitting Goombas
            HandleShellVsGoombas(enemies);

            var feet = FeetRect(marioBounds, 4);

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                var enemy = enemies[i];

                Rectangle enemyRect;
                bool alive;

                if (enemy is moveGoom goom)
                {
                    enemyRect = goom.Bounds;
                    alive = goom.IsAlive;
                    enemyRect.Inflate(-6, 0);
                }
                else if (enemy is Snail.Snail snail)
                {
                    enemyRect = snail.Bounds;
                    alive = true;  // snail is always alive 
                    enemyRect.Inflate(-6, 0); 
                }
                else if (enemy is moveKoop koop)
                {
                    enemyRect = koop.Bounds;
                    alive = koop.IsAlive;
                    enemyRect.Inflate(-6, 0);
                }
                else
                {
                    continue;
                }

                if (!alive) continue;
                if (enemyRect == Rectangle.Empty) continue;
                if (!marioBounds.Intersects(enemyRect)) continue;

                /*bool stomp = feet.Intersects(enemyRect) &&
                             (marioBounds.Bottom <= enemyRect.Top + 4);*/
                bool stomp = 
                    marioVelocity > 0 &&       // Mario must be falling
                    feet.Intersects(enemyRect);   // Feet must hit enemy


                // =========================
                // GOOMBA
                // =========================
                if (enemy is moveGoom g)
                {
                    if (stomp)
                    {
                        g.IsAlive = false;

                        // Award stomp combo points
                        int points = GetStompScore();
                        ScreenManager.Instance.AddScore(points);   // <-- We add this below

                        bounce?.Invoke();

                        // Play stomp sound
                        SoundManager.Instance.PlayEffect(Game1.ChristmasMode ? "xmasThud" : "stomp");

                        continue;
                    }

                    // Side hit by a live Goomba
                    ResetStompCombo();
                    onHit?.Invoke();
                }

                // =========================
                // KOOPA
                // =========================
                else if (enemy is moveKoop k)
                {
                    if (stomp)
                    {
                        if (!Game1.ChristmasMode)
                        {
                            SoundManager.Instance.PlayEffect("stomp");
                        }
                        else
                        {
                            SoundManager.Instance.PlayEffect("xmasThud");
                        }

                        // Award stomp score
                        int points = GetStompScore();
                        ScreenManager.Instance.AddScore(points);

                        if (k.IsWalking)
                        {
                            // First stomp -> go into shell
                            k.EnterShell();
                        }
                        else if (k.IsShellIdle)
                        {
                            // Stomp idle shell -> kick it
                            int dir = marioBounds.Center.X > enemyRect.Center.X ? -1 : 1;
                            if (!Game1.ChristmasMode)
                            {
                                SoundManager.Instance.PlayEffect("kick");
                            }
                            else
                            {
                                SoundManager.Instance.PlayEffect("xmasThud");
                            }
                            k.Kick(dir);
                        }
                        else if (k.IsShellMoving)
                        {
                            // Stomp moving shell -> stop it
                            k.EnterShell();
                        }

                        bounce?.Invoke();
                        continue;
                    }
                    ResetStompCombo();

                    // SIDE HIT LOGIC
                    if (k.IsShellMoving)
                    {
                        // Moving shell hitting Mario hurts him
                        onHit?.Invoke();
                    }
                    else if (k.IsShellIdle)
                    {
                        // Walk into idle shell -> kick it
                        int dir = marioBounds.Center.X > enemyRect.Center.X ? -1 : 1;
                        SoundManager.Instance.PlayEffect("kick");
                        k.Kick(dir);
                    }
                    else if (k.IsWalking)
                    {
                        // Walk into walking Koopa -> hurt Mario
                        onHit?.Invoke();
                    }
                }
            }
        }

        public static void HandleMarioEnemyCollision(
            SmallMarioSprite mario,
            IList<object> enemies,
            Action restart)
        {
            HandleMarioVsEnemiesCore(
                marioBounds: mario.Bounds,
                marioVelocity: mario.verticalVelocity,
                bounce: () => mario.Bounce(-250f),
                onHit: restart,
                enemies: enemies
            );
        }

        public static void HandleMarioEnemyCollision(
            BigMarioSprite mario,
            IList<object> enemies,
            Action onBigHit)
        {
            HandleMarioVsEnemiesCore(
                marioBounds: mario.Bounds,
                marioVelocity: mario.verticalVelocity,
                bounce: () => mario.Bounce(-250f),
                onHit: onBigHit,
                enemies: enemies
            );
        }
    }
}