using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class JumpSuperSound
    {
        private readonly SoundManager soundManager;

        public JumpSuperSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadEffect(game, "jumpSuper", "Audio/smb_jump-super");
        }

        public void Play()
        {
            soundManager.PlayEffect("jumpSuper");
        }
    }
}
