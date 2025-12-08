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
        // ===========================
        // DEFAULT GAME VALUES
        // ===========================
        private const int DefaultLives = 3;
        private const double DefaultTime = 400;
        private const int MinTime = 0;

        public GameState CurrentState { get; private set; } = GameState.Title;

        // ===========================
        // HUD + GAME STATS
        // ===========================
        public int Coins { get; private set; } = 0;
        public int Score { get; private set; } = 0;
        public int Lives { get; set; } = DefaultLives;
        public double Time { get; private set; } = DefaultTime;

        //  THESE WERE MISSING (CAUSE OF YOUR ERROR)
        public string World { get; private set; } = "1";
        public string Level { get; private set; } = "1";

        //  TIME FREEZE SUPPORT (FLAGPOLE)
        public bool IsFrozen { get; private set; } = false;

        // ===========================
        // STATE CONTROL
        // ===========================
        public void ChangeState(GameState newState)
        {
            CurrentState = newState;
        }

        // ===========================
        // HUD MUTATORS
        // ===========================
        public void AddScore(int points) => Score += points;
        public void AddCoin() => Coins++;

        public void LoseLife()
        {
            Lives--;
            if (Lives <= 0)
            {
                CurrentState = GameState.GameOver;
                Lives = DefaultLives;
            }
        }

        public void SetWorld(string world, string level)
        {
            World = world;
            Level = level;
        }

        // ===========================
        // RESET CONTROL
        // ===========================
        public void ResetLevel()
        {
            Time = DefaultTime;
            Coins = 0;
            Score = 0;
            IsFrozen = false;
        }

        public void ResetAll()
        {
            ResetLevel();
            Lives = DefaultLives;
            World = "1";
            Level = "1";
            CurrentState = GameState.Title;
        }

        // ===========================
        // FLAGPOLE TIME FREEZE + BONUS
        // ===========================
        public void FreezeTime()
        {
            IsFrozen = true;
        }

        public void ConvertTimeToScore()
        {
            if (Time > 0)
            {
                Time -= 1;
                Score += 50;
            }
        }

        // ===========================
        // TIMER UPDATE
        // ===========================
        public void Update(GameTime gameTime)
        {
            if (CurrentState != GameState.Playing || IsFrozen)
                return;
            Time -= gameTime.ElapsedGameTime.TotalSeconds;

            if (Time <= MinTime)
            {
                Time = MinTime;
                CurrentState = GameState.TimeUp;
            }
        }
    }
}
