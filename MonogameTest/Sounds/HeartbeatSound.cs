using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public class HeartbeatSound
    {
        private readonly SoundManager soundManager;

        public HeartbeatSound(SoundManager manager)
        {
            soundManager = manager;
        }

        public void Load(Game game)
        {
            soundManager.LoadSong(game, "heartbeat", "Sound/Heartbeat");
        }

        public void Play(bool loop = true)
        {
            soundManager.PlaySong("heartbeat", loop);
        }
    }
}
