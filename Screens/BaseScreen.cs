using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.Screens
{
    public abstract class BaseScreen
    {
        public static Texture2D Pixel;
        protected ScreenManager Manager { get; }
        protected Game1 Game { get; }

        protected BaseScreen(Game1 game, ScreenManager manager)
        {
            Game = game;
            Manager = manager;

            // Create 1x1 pixel for overlay rendering
            if (Pixel == null)
            {
                Pixel = new Texture2D(game.GraphicsDevice, 1, 1);
                Pixel.SetData(new[] { Color.White });
            }
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}