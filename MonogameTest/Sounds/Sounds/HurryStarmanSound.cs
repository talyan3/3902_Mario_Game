using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class HurryStarmanSound
    {
        private readonly SoundManager soundManager;

        public HurryStarmanSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadSong(game, "hurryStarman", "Audio/17-hurry-starman-");
        }

        public void Play(bool loop = false)
        {
            soundManager.PlaySong("hurryStarman", loop);
        }
    }
}
