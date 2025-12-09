using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.Screens
{
    public class TimeUpScreen : BaseScreen
    {
        private readonly SpriteFont _font;
        private readonly HUDScreen _hud;

        private double _timer = DefaultTimerSeconds;

        // Constants
        private const double DefaultTimerSeconds = 2.0;
        private const float TextOffsetX = 80f;

        public TimeUpScreen(Game1 game, ScreenManager manager, SpriteFont font, Texture2D coin)
            : base(game, manager)
        {
            _font = font;
            _hud = new HUDScreen(font, coin, manager);

            // Play warning sound
            SoundManager.Instance.StopSong();
            SoundManager.Instance.PlayEffect("warning");
        }

        public override void Update(GameTime gameTime)
        {
            // Countdown
            _timer -= gameTime.ElapsedGameTime.TotalSeconds;

            // Move to Game Over
            if (_timer <= 0)
                Manager.ChangeState(GameState.GameOver);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.GraphicsDevice.Clear(Color.Black);

            // HUD at top
            _hud.Draw(sb);

            // Center position
            float centerX = sb.GraphicsDevice.Viewport.Width / 2f;
            float centerY = sb.GraphicsDevice.Viewport.Height / 2f;

            // "TIME UP" text
            sb.DrawString(
                _font,
                "TIME UP",
                new Vector2(centerX - TextOffsetX, centerY),
                Color.White
            );
        }
    }
}