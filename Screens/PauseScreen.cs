using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonogameTest.Screens
{
    public class PauseScreen : BaseScreen
    {
        private readonly SpriteFont _font;
        private KeyboardState _oldState;

        public bool IsPaused { get; private set; }

        public PauseScreen(Game1 game, ScreenManager manager, SpriteFont font)
            : base(game, manager)
        {
            _font = font;
        }

        private void TogglePause()
        {
            IsPaused = !IsPaused;
            Game1.InputLocked = IsPaused;

            if (IsPaused)
                SoundManager.Instance.PauseSong();
            else
                SoundManager.Instance.ResumeSong();
        }


        public override void Update(GameTime gameTime)
        {
            var ks = Keyboard.GetState();

            // Edge-trigger press detection
            if (ks.IsKeyDown(Keys.P) && !_oldState.IsKeyDown(Keys.P))
                TogglePause();

            _oldState = ks;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsPaused)
                return;

            // Draw dark overlay
            spriteBatch.Draw(
                Pixel,
                new Rectangle(0, 0, Game1.ScreenWidth, Game1.ScreenHeight),
                Color.Black * 0.5f
            );

            // Draw PAUSED text centered
            const string text = "PAUSED";
            Vector2 size = _font.MeasureString(text);

            Vector2 pos = new(
                (Game1.ScreenWidth - size.X) / 2f,
                (Game1.ScreenHeight - size.Y) / 2f
            );

            spriteBatch.DrawString(_font, text, pos, Color.White);
        }
    }
}