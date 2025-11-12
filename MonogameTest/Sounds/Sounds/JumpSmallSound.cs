using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class JumpSmallSound
    {
        private readonly SoundManager soundManager;

        public JumpSmallSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadEffect(game, "jumpSmall", "Audio/smb_jumpsmall");
        }

        public void Play()
        {
            soundManager.PlayEffect("jumpSmall");
        }
    }
}
