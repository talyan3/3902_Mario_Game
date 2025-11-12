using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class MainThemeSound
    {
        private readonly SoundManager soundManager;

        public MainThemeSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadSong(game, "mainTheme", "Audio/01-main-theme-overworld");
        }

        public void Play(bool loop = true)
        {
            soundManager.PlaySong("mainTheme", loop);
        }
    }
}
