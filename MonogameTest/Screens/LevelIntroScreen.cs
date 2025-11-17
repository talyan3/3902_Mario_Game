using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.Screens
{
    public class LevelIntroScreen : BaseScreen
    {
        private SpriteFont _font;
        private Texture2D _coinTex;
        private double timer = 2.0;

        public LevelIntroScreen(Game1 game, ScreenManager manager,
                                SpriteFont font, Texture2D coin)
            : base(game, manager)
        {
            _font = font;
            _coinTex = coin;
        }

        public override void Update(GameTime gameTime)
        {
            timer -= gameTime.ElapsedGameTime.TotalSeconds;

            if (timer <= 0)
            {
                //SoundManager.Instance.PlaySong("mainTheme");
                Manager.ChangeState(GameState.Playing);
                timer = 2.0;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.GraphicsDevice.Clear(Color.Black);

            

            string world = $"{Manager.World}-{Manager.Level}";
            string coins = Manager.Coins.ToString("00");
            string time = ((int)Manager.Time).ToString();

            sb.DrawString(_font, "MARIO", new Vector2(90, 15), Color.White);
            sb.DrawString(_font, Manager.Score.ToString("000000"), new Vector2(90, 55), Color.White);

            sb.Draw(_coinTex, new Vector2(330, 45), null, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            sb.DrawString(_font, "x" + coins, new Vector2(375, 55), Color.White);

            sb.DrawString(_font, "WORLD", new Vector2(550, 15), Color.White);
            sb.DrawString(_font, world, new Vector2(580, 55), Color.White);

            sb.DrawString(_font, "TIME", new Vector2(800, 15), Color.White);
            sb.DrawString(_font, time, new Vector2(825, 55), Color.White);

            sb.DrawString(_font, $"WORLD {Manager.World}-{Manager.Level}",
                  new Vector2(360, 350), Color.White);

        

            sb.DrawString(_font, "MARIO", new Vector2(360, 520), Color.White);

            sb.DrawString(_font, $"x0{Manager.Lives}",
                  new Vector2(560, 520), Color.White);
        }
    }
}