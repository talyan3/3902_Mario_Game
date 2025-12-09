using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class LevelCompleteSound
    {
        private readonly SoundManager soundManager;

        public LevelCompleteSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            if (!Game1.ChristmasMode)
            {
                soundManager.LoadSong(game, "levelComplete", "Sounds/06-level-complete");
            }
            else
            {
                soundManager.LoadSong(game, "levelComplete", "Sounds/XmasWin");
            }
            
        }

        public void Play(bool loop = false)
        {
            soundManager.PlaySong("levelComplete", loop);
        }
    }
}
