using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class HurryUndergroundSound
    {
        private readonly SoundManager soundManager;

        public HurryUndergroundSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadSong(game, "hurryUnderground", "Sounds/14-hurry-underground-");
        }

        public void Play(bool loop = false)
        {
            soundManager.PlaySong("hurryUnderground", loop);
        }
    }
}
