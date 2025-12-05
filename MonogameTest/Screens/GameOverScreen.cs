using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonogameTest.Screens
{
    public class GameOverScreen : BaseScreen
    {
        private readonly SpriteFont _font;
        private readonly HUDScreen _hud;

        // Timer until auto-return to title
        private double _timer = DefaultTimerSeconds;

        // Constants to avoid magic numbers
        private const double DefaultTimerSeconds = 4.0;
        private const float TextOffsetX = 120f;    // half text width for centering
        private const float TextOffsetY = 0f;      // vertical offset if needed

        public GameOverScreen(Game1 game, ScreenManager manager, SpriteFont font, Texture2D coin)
            : base(game, manager)
        {
            _font = font;
            _hud = new HUDScreen(font, coin, manager);

            // Play Game Over music
            SoundManager.Instance.StopSong();
            //SoundManager.Instance.PlaySong("gameOver2", loop: false);
        }

        public override void Update(GameTime gameTime)
        {
            // Countdown
            _timer -= gameTime.ElapsedGameTime.TotalSeconds;

            var k = Keyboard.GetState();
            bool skip = k.IsKeyDown(Keys.Enter);

            // When timer ends or player skips
            if (_timer <= 0 || skip)
            {
                Manager.ResetLevel();
                Manager.ChangeState(GameState.Title);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.GraphicsDevice.Clear(Color.Black);

            // HUD at top of screen
            _hud.Draw(sb);

            // Center point of screen
            float centerX = sb.GraphicsDevice.Viewport.Width / 2f;
            float centerY = sb.GraphicsDevice.Viewport.Height / 2f;

            // Draw Game Over text centered
            sb.DrawString(
                _font,
                "GAME OVER",
                new Vector2(centerX - TextOffsetX, centerY + TextOffsetY),
                Color.White
            );
        }
    }
}