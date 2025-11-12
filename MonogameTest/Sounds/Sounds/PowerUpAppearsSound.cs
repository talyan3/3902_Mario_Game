using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class PowerUpAppearsSound
    {
        private readonly SoundManager soundManager;

        public PowerUpAppearsSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadEffect(game, "powerUpAppears", "Audio/smb_powerup_appears");
        }

        public void Play()
        {
            soundManager.PlayEffect("powerUpAppears");
        }
    }
}
