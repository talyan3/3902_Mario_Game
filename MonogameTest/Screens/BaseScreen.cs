using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.Screens
{
    public abstract class BaseScreen
    {
        protected ScreenManager Manager { get; }
        protected Game1 Game { get; }

        protected BaseScreen(Game1 game, ScreenManager manager)
        {
            Game = game;
            Manager = manager;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
