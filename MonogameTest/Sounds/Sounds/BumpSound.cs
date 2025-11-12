using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class BumpSound
    {
        private readonly SoundManager soundManager;

        public BumpSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadEffect(game, "bump", "Audio/smb_bump");
        }

        public void Play()
        {
            soundManager.PlayEffect("bump");
        }
    }
}
