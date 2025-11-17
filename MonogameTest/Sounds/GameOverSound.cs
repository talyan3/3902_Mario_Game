using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class GameOverSound
    {
        private readonly SoundManager soundManager;

        public GameOverSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadSong(game, "gameOver", "Sounds/09-game-over");
        }

        public void Play(bool loop = false)
        {
            soundManager.PlaySong("gameOver", loop);
        }
    }
}
