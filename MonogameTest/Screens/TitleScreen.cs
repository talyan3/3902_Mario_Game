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

        // Title image crop
        private static readonly Rectangle TitleRect = new Rectangle(0, 12, 258, 224);

        // HUD positions
        private static readonly Vector2 MarioLabelPos = new Vector2(90f, 15f);
        private static readonly Vector2 ScorePos = new Vector2(90f, 55f);

        private static readonly Vector2 CoinIconPos = new Vector2(330f, 45f);
        private static readonly Vector2 CoinTextPos = new Vector2(375f, 55f);
        private const float CoinScale = 3f;

        private static readonly Vector2 WorldLabelPos = new Vector2(550f, 15f);
        private static readonly Vector2 WorldValuePos = new Vector2(580f, 55f);

        private static readonly Vector2 TimeLabelPos = new Vector2(800f, 15f);
        private static readonly Vector2 TimeValuePos = new Vector2(825f, 55f);

        // Click area height
        private const int ClickBoxHeight = 50;
        private const int BottomOffset = 220;

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

            // Press Enter to start
            if (keys.IsKeyDown(Keys.Enter))
            {
                StartGame();
            }

            // Click area for starting the game
            Rectangle clickArea = new Rectangle(
                0,
                Game.GraphicsDevice.Viewport.Height - BottomOffset,
                Game.GraphicsDevice.Viewport.Width,
                ClickBoxHeight
            );

            // Mouse click start
            if (clickArea.Contains(mouse.Position) &&
                mouse.LeftButton == ButtonState.Pressed)
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            // Stop any music before switching screens
            SoundManager.Instance.StopSong();

            // Title → LevelIntro → Playing
            Manager.ChangeState(GameState.LevelIntro);
        }

        private void DrawHUD(SpriteBatch sb)
        {
            string world = $"{Manager.World}-{Manager.Level}";
            string coins = Manager.Coins.ToString("00");
            string time = ((int)Manager.Time).ToString();
            string score = Manager.Score.ToString("000000");

            sb.DrawString(_font, "MARIO", MarioLabelPos, Color.White);
            sb.DrawString(_font, score, ScorePos, Color.White);

            sb.Draw(_coinTexture, CoinIconPos, null, Color.White,
                0f, Vector2.Zero, CoinScale, SpriteEffects.None, 0f);

            sb.DrawString(_font, "x" + coins, CoinTextPos, Color.White);

            sb.DrawString(_font, "WORLD", WorldLabelPos, Color.White);
            sb.DrawString(_font, world, WorldValuePos, Color.White);

            sb.DrawString(_font, "TIME", TimeLabelPos, Color.White);
            sb.DrawString(_font, time, TimeValuePos, Color.White);
        }

        public override void Draw(SpriteBatch sb)
        {
            int screenW = Game.GraphicsDevice.Viewport.Width;
            int screenH = Game.GraphicsDevice.Viewport.Height;

            // Scale title image to fill screen
            float scaleX = screenW / (float)TitleRect.Width;
            float scaleY = screenH / (float)TitleRect.Height;
            float scale = MathHelper.Max(scaleX, scaleY);

            // Center the background
            Vector2 pos = new Vector2(
                (screenW - TitleRect.Width * scale) / 2f,
                (screenH - TitleRect.Height * scale) / 2f
            );

            sb.Draw(_sheet, pos, TitleRect, Color.White,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            DrawHUD(sb);
        }

        private void DrawCentered(SpriteBatch sb, string text, double y, Color color, float scale)
        {
            Vector2 size = _font.MeasureString(text);
            float x = (Game.GraphicsDevice.Viewport.Width - size.X * scale) / 2f;

            sb.DrawString(
                _font,
                text,
                new Vector2(x, (float)y),
                color,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}