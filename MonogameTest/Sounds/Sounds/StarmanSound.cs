using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class StarmanSound
    {
        private readonly SoundManager soundManager;

        public StarmanSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadSong(game, "starman", "Audio/05-starman");
        }

        public void Play(bool loop = true)
        {
            soundManager.PlaySong("starman", loop);
        }
    }
}
