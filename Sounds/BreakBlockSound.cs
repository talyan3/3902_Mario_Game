using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class BreakBlockSound
    {
        private readonly SoundManager soundManager;

        public BreakBlockSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadEffect(game, "breakBlock", "Sounds/smb_breakblock");
        }

        public void Play()
        {
            soundManager.PlayEffect("breakBlock");
        }
    }
}
