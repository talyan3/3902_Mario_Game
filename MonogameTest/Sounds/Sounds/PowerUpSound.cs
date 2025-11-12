using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class PowerUpSound
    {
        private readonly SoundManager soundManager;

        public PowerUpSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadEffect(game, "powerUp", "Audio/smb_powerup");
        }

        public void Play()
        {
            soundManager.PlayEffect("powerUp");
        }
    }
}
