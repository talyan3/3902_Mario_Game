using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class CoinSound
    {
        private readonly SoundManager soundManager;

        public CoinSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadEffect(game, "coin", "Audio/smb_coin");
        }

        public void Play()
        {
            soundManager.PlayEffect("coin");
        }
    }
}
