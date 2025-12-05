using Microsoft.Xna.Framework;

namespace MonogameTest.Screens
{
    public enum GameState
    {
        Title,
        LevelIntro,
        Playing,
        TimeUp,
        Death,
        GameOver
    }

    public class ScreenManager
    {
        // Default values
        private const int DefaultLives = 3;
        private const double DefaultTime = 400;
        private const int MinTime = 0;

        public GameState CurrentState { get; private set; } = GameState.Title;

        // HUD data
        public int Coins { get; set; } = 0;
        public int Score { get; set; } = 0;
        public int Lives { get; set; } = DefaultLives;
        public double Time { get; set; } = DefaultTime;
        public string World { get; set; } = "1";
        public string Level { get; set; } = "1";

        public void ChangeState(GameState newState)
        {
            CurrentState = newState;
        }

        public void ResetLevel()
        {
            // Reset level stats
            Time = DefaultTime;
            Coins = 0;
            Score = 0;
            // Lives handled separately in Game1
        }

        public void Update(GameTime gameTime)
        {
            // Timer only runs during gameplay
            if (CurrentState == GameState.Playing)
            {
                Time -= gameTime.ElapsedGameTime.TotalSeconds;

                if (Time <= MinTime)
                {
                    Time = MinTime;
                    CurrentState = GameState.TimeUp;
                }
            }
        }
    }
}