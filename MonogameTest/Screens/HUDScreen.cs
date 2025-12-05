using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.Screens
{
    public class HUDScreen
    {
        private readonly SpriteFont _font;
        private readonly Texture2D _coinTexture;
        private readonly ScreenManager _screenManager;

        // HUD positions
        private static readonly Vector2 MarioLabelPos = new Vector2(90f, 15f);
        private static readonly Vector2 ScorePos = new Vector2(90f, 55f);

        private static readonly Vector2 CoinIconPos = new Vector2(330f, 45f);
        private static readonly Vector2 CoinTextPos = new Vector2(375f, 55f);
        private const float CoinIconScale = 3f;

        private static readonly Vector2 WorldLabelPos = new Vector2(550f, 15f);
        private static readonly Vector2 WorldValuePos = new Vector2(580f, 55f);

        private static readonly Vector2 TimeLabelPos = new Vector2(800f, 15f);
        private static readonly Vector2 TimeValuePos = new Vector2(825f, 55f);

        public HUDScreen(SpriteFont font, Texture2D coinTexture, ScreenManager manager)
        {
            _font = font;
            _coinTexture = coinTexture;
            _screenManager = manager;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Text values
            string coins = _screenManager.Coins.ToString("00");
            string time = ((int)_screenManager.Time).ToString();
            string world = _screenManager.World + "-" + _screenManager.Level;
            string score = _screenManager.Score.ToString("000000");

            // MARIO + SCORE
            spriteBatch.DrawString(_font, "MARIO", MarioLabelPos, Color.White);
            spriteBatch.DrawString(_font, score, ScorePos, Color.White);

            // COIN ICON + COUNT
            spriteBatch.Draw(_coinTexture, CoinIconPos, null, Color.White,
                0f, Vector2.Zero, CoinIconScale, SpriteEffects.None, 0f);

            spriteBatch.DrawString(_font, "x" + coins, CoinTextPos, Color.White);

            // WORLD
            spriteBatch.DrawString(_font, "WORLD", WorldLabelPos, Color.White);
            spriteBatch.DrawString(_font, world, WorldValuePos, Color.White);

            // TIME
            spriteBatch.DrawString(_font, "TIME", TimeLabelPos, Color.White);
            spriteBatch.DrawString(_font, time, TimeValuePos, Color.White);
        }
    }
}