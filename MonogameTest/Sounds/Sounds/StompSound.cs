using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class StompSound
    {
        private readonly SoundManager soundManager;

        public StompSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadEffect(game, "stomp", "Audio/smb_stomp");
        }

        public void Play()
        {
            soundManager.PlayEffect("stomp");
        }
    }
}
