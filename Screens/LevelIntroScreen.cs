using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.Screens
{
    public class LevelIntroScreen : BaseScreen
    {
        private readonly SpriteFont _font;
        private readonly Texture2D _coinTex;

        // Timer before gameplay starts
        private double _timer = DefaultIntroTime;

        // Constants
        private const double DefaultIntroTime = 2.0;
        private const float CoinScale = 3f;

        // HUD positions
        private static readonly Vector2 MarioLabelPos = new Vector2(90f, 15f);
        private static readonly Vector2 ScorePos = new Vector2(90f, 55f);

        private static readonly Vector2 CoinIconPos = new Vector2(330f, 45f);
        private static readonly Vector2 CoinTextPos = new Vector2(375f, 55f);

        private static readonly Vector2 WorldLabelPos = new Vector2(550f, 15f);
        private static readonly Vector2 WorldValuePos = new Vector2(580f, 55f);

        private static readonly Vector2 TimeLabelPos = new Vector2(800f, 15f);
        private static readonly Vector2 TimeValuePos = new Vector2(825f, 55f);

        // Large intro text positions
        private static readonly Vector2 WorldIntroPos = new Vector2(360f, 350f);
        private static readonly Vector2 MarioIntroPos = new Vector2(360f, 520f);
        private static readonly Vector2 LivesIntroPos = new Vector2(560f, 520f);

        public LevelIntroScreen(Game1 game, ScreenManager manager,
                                SpriteFont font, Texture2D coin)
            : base(game, manager)
        {
            _font = font;
            _coinTex = coin;
        }

        public override void Update(GameTime gameTime)
        {
            // Countdown
            _timer -= gameTime.ElapsedGameTime.TotalSeconds;

            // Start level
            if (_timer <= 0)
            {
                SoundManager.Instance.StopSong();
                if (!Game1.ChristmasMode)
                {
                    SoundManager.Instance.PlaySong("mainTheme");
                }
                else
                {
                    SoundManager.Instance.PlaySong("mainXmas");
                }

                Manager.ChangeState(GameState.Playing);
                _timer = DefaultIntroTime;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.GraphicsDevice.Clear(Color.Black);

            string world = $"{Manager.World}-{Manager.Level}";
            string coins = Manager.Coins.ToString("00");
            string time = ((int)Manager.Time).ToString();
            string score = Manager.Score.ToString("000000");

            // HUD top-left
            sb.DrawString(_font, "MARIO", MarioLabelPos, Color.White);
            sb.DrawString(_font, score, ScorePos, Color.White);

            // Coins
            sb.Draw(_coinTex, CoinIconPos, null, Color.White, 0f,
                Vector2.Zero, CoinScale, SpriteEffects.None, 0f);

            sb.DrawString(_font, "x" + coins, CoinTextPos, Color.White);

            // World + level
            sb.DrawString(_font, "WORLD", WorldLabelPos, Color.White);
            sb.DrawString(_font, world, WorldValuePos, Color.White);

            // Time
            sb.DrawString(_font, "TIME", TimeLabelPos, Color.White);
            sb.DrawString(_font, time, TimeValuePos, Color.White);

            // Large center intro text
            if(Game1.pipeMode == false)
                sb.DrawString(_font, $"WORLD {world}", WorldIntroPos, Color.White);
            else
                sb.DrawString(_font, "WORLD 1-???", WorldIntroPos, Color.White);


            sb.DrawString(_font, "MARIO", MarioIntroPos, Color.White);
            sb.DrawString(_font, $"x0{Manager.Lives}", LivesIntroPos, Color.White);
        }
    }
}