using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class HurryOverworldSound
    {
        private readonly SoundManager soundManager;

        public HurryOverworldSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadSong(game, "hurryOverworld", "Sounds/18-hurry-overworld-");
        }

        public void Play(bool loop = false)
        {
            soundManager.PlaySong("hurryOverworld", loop);
        }
    }
}
