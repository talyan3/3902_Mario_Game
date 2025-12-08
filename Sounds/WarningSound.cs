using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class WarningSound
    {
        private readonly SoundManager soundManager;

        public WarningSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadEffect(game, "warning", "Sounds/smb_warning");
        }

        public void Play()
        {
            soundManager.PlayEffect("warning");
        }
    }
}
