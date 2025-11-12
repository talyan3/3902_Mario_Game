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
            soundManager.LoadSong(game, "levelComplete", "Audio/06-level-complete");
        }

        public void Play(bool loop = false)
        {
            soundManager.PlaySong("levelComplete", loop);
        }
    }
}
