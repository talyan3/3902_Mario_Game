using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class KickSound
    {
        private readonly SoundManager soundManager;

        public KickSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadEffect(game, "kick", "Audio/smb_kick");
        }

        public void Play()
        {
            soundManager.PlayEffect("kick");
        }
    }
}
