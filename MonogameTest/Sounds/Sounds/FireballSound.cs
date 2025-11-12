using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class FireballSound
    {
        private readonly SoundManager soundManager;

        public FireballSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadEffect(game, "fireball", "Audio/smb_fireball");
        }

        public void Play()
        {
            soundManager.PlayEffect("fireball");
        }
    }
}
