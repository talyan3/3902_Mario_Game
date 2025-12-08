using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class PauseSound
    {
        private readonly SoundManager soundManager;

        public PauseSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadEffect(game, "pause", "Sounds/smb_pause");
        }

        public void Play()
        {
            soundManager.PlayEffect("pause");
        }
    }
}
