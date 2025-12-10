using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.Screens
{
    public class HUDScreen
    {
        private readonly SpriteFont _font;
        private readonly Texture2D _coinTexture;
        private readonly ScreenManager _screenManager;

        public HUDScreen(SpriteFont font, Texture2D coinTexture, ScreenManager manager)
        {
            _font = font;
            _coinTexture = coinTexture;
            _screenManager = manager;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "MARIO", new Vector2(90, 15), Color.White);
            spriteBatch.DrawString(_font, _screenManager.Score.ToString("000000"), new Vector2(90, 55), Color.White);

            spriteBatch.Draw(_coinTexture, new Vector2(330, 45), null, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(_font, "x" + _screenManager.Coins.ToString("00"), new Vector2(375, 55), Color.White);

            spriteBatch.DrawString(_font, "WORLD", new Vector2(550,15), Color.White);
            if (Game1.pipeMode == false)
                spriteBatch.DrawString(_font, "1-1", new Vector2(580,55), Color.White);
            else
                spriteBatch.DrawString(_font, "1-???", new Vector2(570,55), Color.White);


            spriteBatch.DrawString(_font, "TIME", new Vector2(800, 15), Color.White);
            spriteBatch.DrawString(_font, ((int)_screenManager.Time).ToString(), new Vector2(825, 55), Color.White);
        }
    }
}
