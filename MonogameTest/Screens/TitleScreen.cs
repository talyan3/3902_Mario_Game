using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonogameTest.Screens
{
    public class TitleScreen : BaseScreen
    {
        private readonly Texture2D _sheet;
        private readonly SpriteFont _font;
        private readonly Texture2D _coinTexture;

        // Clean crop of the title tile
        private readonly Rectangle _titleRect = new Rectangle(
            0,      // tile X
            12,     // 
            258,
            224
        );

        public TitleScreen(Game1 game, ScreenManager manager, Texture2D sheet, SpriteFont font, Texture2D coin)
            : base(game, manager)
        {
            _sheet = sheet;
            _font = font;
            _coinTexture = coin;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keys = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keys.IsKeyDown(Keys.Enter))
                StartGame();

            Rectangle option1 = new Rectangle(
                0,
                Game.GraphicsDevice.Viewport.Height - 220,
                Game.GraphicsDevice.Viewport.Width,
                50);

            if (option1.Contains(mouse.Position) &&
                mouse.LeftButton == ButtonState.Pressed)
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            Manager.ChangeState(GameState.LevelIntro);
        }

        private void DrawHUD(SpriteBatch sb)
        {
            int screenW = Game.GraphicsDevice.Viewport.Width;

            string world = $"{Manager.World}-{Manager.Level}";
            string coins = Manager.Coins.ToString("00");
            string time = ((int)Manager.Time).ToString();

            sb.DrawString(_font, "MARIO", new Vector2(90, 15), Color.White);
            sb.DrawString(_font, Manager.Score.ToString("000000"), new Vector2(90, 55), Color.White);

            sb.Draw(_coinTexture, new Vector2(330, 45), null, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            sb.DrawString(_font, "x" + coins, new Vector2(375, 55), Color.White);

            sb.DrawString(_font, "WORLD", new Vector2(550, 15), Color.White);
            sb.DrawString(_font, world, new Vector2(580, 55), Color.White);

            sb.DrawString(_font, "TIME", new Vector2(800, 15), Color.White);
            sb.DrawString(_font, time, new Vector2(825, 55), Color.White);
        }


        public override void Draw(SpriteBatch sb)
        {
            int screenW = Game.GraphicsDevice.Viewport.Width;
            int screenH = Game.GraphicsDevice.Viewport.Height;

    

            // edges
            float scaleX = screenW / 258f;
            float scaleY = screenH / 224f;
            float scale = MathHelper.Max(scaleX, scaleY);

            // Center background
            Vector2 bgPos = new Vector2(
                (screenW - 258 * scale) / 2f,
                (screenH - 212 * scale) / 2f
            );


            sb.Draw(_sheet, bgPos, _titleRect, Color.White, 0f, Vector2.Zero, scale,
                SpriteEffects.None, 0f);

            DrawHUD(sb);





        }

        private void DrawCentered(SpriteBatch sb, string text, double y, Color color, float scale)
        {
            Vector2 size = _font.MeasureString(text);
            float x = (Game.GraphicsDevice.Viewport.Width - size.X * scale) / 2;
            sb.DrawString(_font, text, new Vector2(x, (float)y), color, 0f,
                Vector2.Zero, scale, SpriteEffects.None, 0f);
        }


    }
}
