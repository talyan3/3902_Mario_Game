using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.Managers
{
    public class PowerupFieldManager
    {
        private readonly List<PowerupInstance> _powerups = new List<PowerupInstance>();

        private Texture2D _powerupSheet;
        private SpriteBatch _spriteBatch;

        public PowerupFieldManager() { }

        public void LoadContent(ContentManager content, GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;

        }

        /// Allows Game1 to provide the shared powerup sheet.
        public void SetPowerupSheet(Texture2D sheet)
        {
            _powerupSheet = sheet;
        }

        /// Spawns a new powerup at a world position.
        public void Spawn(PowerupType type, Vector2 position)
        {
            if (_powerupSheet == null) return;

            var p = PowerupFactory.Create(type, _powerupSheet, position);
            _powerups.Add(p);
        }

        /// Updates all powerups and returns the one Mario picked up, if any.
        public PowerupInstance Update(GameTime gameTime, StaticSprite mario)
        {
            PowerupInstance pickedUp = null;

            foreach (var p in _powerups)
            {
                p.Update(gameTime);

                if (p.IsAlive && PowerupCollisionHandler.CheckMarioPowerupCollision(mario, p))
                {
                    pickedUp = p;
                    p.IsAlive = false;
                }
            }

            // Cleanup dead powerups
            _powerups.RemoveAll(p => !p.IsAlive);

            return pickedUp;
        }

        /// Draw all active powerups.
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var p in _powerups)
                p.Draw(spriteBatch);
        }

        /// Allows Game1 to see the list if needed.
        public IReadOnlyList<PowerupInstance> GetPowerups() => _powerups;
    }
}
