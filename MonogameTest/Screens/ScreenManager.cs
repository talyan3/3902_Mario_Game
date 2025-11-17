using Microsoft.Xna.Framework;

namespace MonogameTest.Screens
{
    public enum GameState
    {
        Title,
        LevelIntro,
        Playing,
        TimeUp,
        GameOver
    }

    public class ScreenManager
    {
        public GameState CurrentState { get; private set; } = GameState.Title;

        // HUD data
        public int Coins { get; set; } = 0;
        public int Score { get; set; } = 0;
        public int Lives { get; set; } = 3;
        public double Time { get; set; } = 400;
        public string World { get; set; } = "1";
        public string Level { get; set; } = "1";

        public void ChangeState(GameState newState)
        {
            CurrentState = newState;
        }

        public void ResetLevel()
        {
            Time = 400;
            Coins = 0;
            Score = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentState == GameState.Playing)
            {
                Time -= gameTime.ElapsedGameTime.TotalSeconds;
                if (Time <= 0)
                {
                    Time = 0;
                    //SoundManager.Instance.StopSong();
                    //SoundManager.Instance.PlaySong("gameOver");
                    CurrentState = GameState.TimeUp;
                }
            }
        }
    }
}