using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.Screens
{
    // Base class for all screens (title, HUD, intro, game over, etc.)
    public abstract class BaseScreen
    {
        // Reference to the screen manager
        protected ScreenManager Manager { get; }

        // Reference to the main game
        protected Game1 Game { get; }

        protected BaseScreen(Game1 game, ScreenManager manager)
        {
            Game = game;
            Manager = manager;
        }

        // Screen-specific update logic
        public abstract void Update(GameTime gameTime);

        // Screen-specific draw logic
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}