using Microsoft.Xna.Framework;
using MonogameTest.Sounds;

namespace MonogameTest
{
    public class MarioStateController
    {
        public bool IsBig { get; private set; }

        public readonly SmallMarioSprite smallMario;
        public readonly BigMarioSprite bigMario;
        private StaticSprite currentMario;
        private readonly int tileSize;

        public StaticSprite CurrentMario => currentMario;

        public MarioStateController(
            SmallMarioSprite small,
            BigMarioSprite big,
            StaticSprite startingMario,
            int tileSize)
        {
            smallMario = small;
            bigMario = big;
            currentMario = startingMario;
            this.tileSize = tileSize;

            IsBig = startingMario == bigMario;
        }

        // ====================================================
        // GROW (MUSHROOM / POWERUP)
        // ====================================================
        public void Grow()
        {
            if (IsBig) return;

            SoundManager.Instance.PlayEffect("powerUp");

            Vector2 pos = smallMario.Position;
            pos.Y = tileSize * 13;

            bigMario.Position = pos;
            currentMario = bigMario;
            IsBig = true;
        }

        // ====================================================
        // SHRINK (HIT BY ENEMY)
        // ====================================================
        public void Shrink()
        {
            if (!IsBig) return;

            SoundManager.Instance.PlayEffect("intoTheTunnel");

            Vector2 pos = bigMario.Position;
            pos.Y = tileSize * 13;

            smallMario.Position = pos;
            currentMario = smallMario;
            IsBig = false;
        }

        // ====================================================
        // HARD RESET (DEATH / LEVEL RESET)
        // ====================================================
        public void ForceSmall(Vector2 spawnPoint)
        {
            IsBig = false;
            smallMario.Position = spawnPoint;
            bigMario.Position = spawnPoint;
            currentMario = smallMario;
        }
    }
}
