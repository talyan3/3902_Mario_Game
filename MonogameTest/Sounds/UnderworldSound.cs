using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class UnderworldSound
    {
        private readonly SoundManager soundManager;

        public UnderworldSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadSong(game, "underworld", "Sounds/02-underworld");
        }

        public void Play(bool loop = true)
        {
            soundManager.PlaySong("underworld", loop);
        }
    }
}
