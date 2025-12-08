using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest
{
    public class PowerupManager
    {
        private Texture2D texture;
        private int currentIndex = 0;
        private const int PowerupSize = 16;
        private const int TotalPowerups = 5; // number of powerups in the spritesheet

        public PowerupManager(Texture2D texture)
        {
            this.texture = texture;
        }

        public void NextPowerup()
        {
            currentIndex = (currentIndex + 1) % TotalPowerups;
        }

        public void PreviousPowerup()
        {
            currentIndex = (currentIndex - 1 + TotalPowerups) % TotalPowerups;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            // source rectangle for current block
            Rectangle source = new Rectangle(currentIndex * PowerupSize, 0, PowerupSize, PowerupSize);

            spriteBatch.Draw(texture, position, source, Color.White, 
                             0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
        }
    }
}

//powerups.Add(PowerupFactory.Create(PowerupType.Mushroom, powerupsSheet, new Vector2(16*8, 16*11)));
        //powerups.Add(PowerupFactory.Create(PowerupType.Coin, powerupsSheet, new Vector2(16*9, 16*11)));
        //powerups.Add(PowerupFactory.Create(PowerupType.Star, powerupsSheet, new Vector2(16*10, 16*11)));
        //powerups.Add(PowerupFactory.Create(PowerupType.GreenMushroom, powerupsSheet, new Vector2(16*12, 16*11)));
