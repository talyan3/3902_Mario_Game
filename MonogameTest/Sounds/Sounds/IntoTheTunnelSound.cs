using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class IntoTheTunnelSound
    {
        private readonly SoundManager soundManager;

        public IntoTheTunnelSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadSong(game, "intoTheTunnel", "Audio/11-into-the-tunnel");
        }

        public void Play(bool loop = false)
        {
            soundManager.PlaySong("intoTheTunnel", loop);
        }
    }
}
