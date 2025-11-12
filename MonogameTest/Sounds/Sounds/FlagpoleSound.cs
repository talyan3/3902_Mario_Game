using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class FlagpoleSound
    {
        private readonly SoundManager soundManager;

        public FlagpoleSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadEffect(game, "flagpole", "Audio/smb_flagpole");
        }

        public void Play()
        {
            soundManager.PlayEffect("flagpole");
        }
    }
}
