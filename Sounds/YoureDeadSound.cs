using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class YoureDeadSound
    {
        private readonly SoundManager soundManager;

        public YoureDeadSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadSong(game, "youreDead", "Sounds/08-you-re-dead");
        }

        public void Play(bool loop = false)
        {
            soundManager.PlaySong("youreDead", loop);
        }
    }
}
