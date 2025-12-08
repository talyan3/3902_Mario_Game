using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class HurrySound
    {
        private readonly SoundManager soundManager;

        public HurrySound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadSong(game, "hurry", "Sounds/13-hurry");
        }

        public void Play(bool loop = false)
        {
            soundManager.PlaySong("hurry", loop);
        }
    }
}
