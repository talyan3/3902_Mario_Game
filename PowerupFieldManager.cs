using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.Managers
{
    /// <summary>
    /// Handles all active powerups in the level: spawn, update, collision, and draw.
    /// </summary>
    public class PowerupFieldManager
    {
        private readonly List<PowerupInstance> _powerups = new();

        private Texture2D _powerupSheet;
        private SpriteBatch _spriteBatch;

        public PowerupFieldManager() { }

        // =========================================================
        // LOAD / SETUP
        // =========================================================

        public void LoadContent(ContentManager content, GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        /// Allows Game1 to provide the shared powerup sheet.
        public void SetPowerupSheet(Texture2D sheet)
        {
            _powerupSheet = sheet;
        }

        // =========================================================
        // SPAWN
        // =========================================================

        /// Spawns a new powerup at a world position.
        public void Spawn(PowerupType type, Vector2 position)
        {
            if (_powerupSheet == null)
                return;

            var p = PowerupFactory.Create(type, _powerupSheet, position);
            _powerups.Add(p);
        }

        // =========================================================
        // UPDATE + COLLISION
        // =========================================================

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

            // Cleanup dead powerups AFTER iteration
            _powerups.RemoveAll(p => !p.IsAlive);

            return pickedUp;
        }

        // =========================================================
        // DRAW
        // =========================================================

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var p in _powerups)
                p.Draw(spriteBatch);
        }

        // =========================================================
        // DEBUG / ACCESS
        // =========================================================

        public IReadOnlyList<PowerupInstance> GetPowerups()
            => _powerups;
    }
}
