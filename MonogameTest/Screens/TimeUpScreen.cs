using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.Screens
{
    public class TimeUpScreen : BaseScreen
    {
        private readonly SpriteFont _font;
        private readonly HUDScreen _hud;
        private double _timer = 2.0; 

        public TimeUpScreen(Game1 game, ScreenManager manager, SpriteFont font, Texture2D coin)
            : base(game, manager)
        {
            _font = font;
            _hud = new HUDScreen(font, coin, manager);
        }

        public override void Update(GameTime gameTime)
        {
            _timer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer <= 0)
            {
                Manager.ChangeState(GameState.GameOver);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.GraphicsDevice.Clear(Color.Black);

            // HUD
            _hud.Draw(sb);

            // "TIME UP" 
            sb.DrawString(_font, "TIME UP",
                new Vector2(sb.GraphicsDevice.Viewport.Width / 2f - 80,
                            sb.GraphicsDevice.Viewport.Height / 2f),
                Color.White);
        }
    }
}
