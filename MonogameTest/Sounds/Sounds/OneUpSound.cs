using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class OneUpSound
    {
        private readonly SoundManager soundManager;

        public OneUpSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadEffect(game, "oneUp", "Audio/smb_1-up");
        }

        public void Play()
        {
            soundManager.PlayEffect("oneUp");
        }
    }
}
