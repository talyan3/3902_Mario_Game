using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonogameTest.Screens
{
    public class GameOverScreen : BaseScreen
    {
        private readonly SpriteFont _font;
        private readonly HUDScreen _hud;
        private double _timer = 4.0; 

        public GameOverScreen(Game1 game, ScreenManager manager, SpriteFont font, Texture2D coin)
            : base(game, manager)
        {
            _font = font;
            _hud = new HUDScreen(font, coin, manager);
        }

        public override void Update(GameTime gameTime)
        {
            _timer -= gameTime.ElapsedGameTime.TotalSeconds;

            var k = Keyboard.GetState();
            bool skip = k.IsKeyDown(Keys.Enter);
            

            if (_timer <= 0 || skip)
            {
    
                Manager.ResetLevel();
                Manager.ChangeState(GameState.Title);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.GraphicsDevice.Clear(Color.Black);

            // HUD at top
            _hud.Draw(sb);

            // "GAME OVER" 
            sb.DrawString(_font, "GAME OVER",
                new Vector2(sb.GraphicsDevice.Viewport.Width / 2f - 120,
                            sb.GraphicsDevice.Viewport.Height / 2f),
                Color.White);
        }
    }
}