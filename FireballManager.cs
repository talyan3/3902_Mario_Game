using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonogameTest
{
    public class FireballManager
    {
        private List<Fireball> _fireballs = new();
        private Texture2D _texture;

        public void Load(Texture2D tex)
        {
            _texture = tex;
        }

        public void Shoot(Vector2 pos, bool right)
        {
            var src = new Rectangle(0, 0, 16, 16);
            _fireballs.Add(new Fireball(_texture, src, pos, right));
        }

        public void Update(GameTime time, List<Tile> tiles, List<object> enemies)
        {
            foreach (var f in _fireballs)
            {
                f.Update(time, tiles);

                if (!f.IsAlive) continue;

                foreach (var e in enemies)
                {
                    Rectangle er =
                        e is moveGoom g ? g.Bounds :
                        e is moveKoop k ? k.Bounds :
                        Rectangle.Empty;

                    if (er != Rectangle.Empty && f.Bounds.Intersects(er))
                    {
                        if (e is moveGoom g2) g2.IsAlive = false;
                        if (e is moveKoop k2) k2.IsAlive = false;

                        f.IsAlive = false;
                        break;
                    }
                }
            }

            _fireballs.RemoveAll(f => !f.IsAlive);
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (var f in _fireballs)
                f.Draw(sb);
        }
    }
}
