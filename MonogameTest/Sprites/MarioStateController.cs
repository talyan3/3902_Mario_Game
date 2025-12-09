using Microsoft.Xna.Framework;
using MonogameTest.Sounds;

namespace MonogameTest
{
    public class MarioStateController
    {
        public bool IsBig { get; private set; }

        private readonly SmallMarioSprite smallMario;
        private readonly BigMarioSprite bigMario;
        public StaticSprite currentMario;
        private readonly int tileSize;

        public StaticSprite CurrentMario => currentMario;

        // FLASH SYSTEM
        private bool isFlashing = false;
        private float flashTimer = 0f;
        private int flashCount = 0;
        private bool targetBig;

        private const float FLASH_INTERVAL = 0.08f;
        private const int MAX_FLASHES = 6;

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

        // MUST BE CALLED EVERY FRAME FROM Game1.Update
        public void Update(GameTime gameTime)
        {
            if (!isFlashing)
                return;

            flashTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (flashTimer < FLASH_INTERVAL)
                return;

            flashTimer = 0f;
            flashCount++;

            // Toggle sprite AND keep IsBig consistent
            if (currentMario == smallMario)
            {
                currentMario = bigMario;
                IsBig = true;
            }
            else
            {
                currentMario = smallMario;
                IsBig = false;
            }

            if (flashCount >= MAX_FLASHES)
            {
                isFlashing = false;
                flashCount = 0;

                // Force final state to target
                if (targetBig)
                {
                    currentMario = bigMario;
                    IsBig = true;
                }
                else
                {
                    currentMario = smallMario;
                    IsBig = false;
                }
            }
        }

        // ====================================================
        // GROW (SAFE FLASHING VERSION)
        // ====================================================
        public void Grow()
        {
            // Do not re-enter while flashing
            if (IsBig || isFlashing)
                return;

            SoundManager.Instance.PlayEffect("powerUp");

            Vector2 pos = smallMario.Position;
            pos.Y = tileSize * 13;
            bigMario.Position = pos;

            // Start from SMALL logically,
            // Update() will toggle and keep IsBig in sync
            currentMario = smallMario;
            IsBig = false;

            targetBig = true;
            isFlashing = true;
            flashTimer = 0f;
            flashCount = 0;
        }

        // ====================================================
        // SHRINK (HIT)
        // ====================================================
        public void Shrink()
        {
            if (!IsBig || isFlashing)
                return;

            SoundManager.Instance.PlaySong("intoTheTunnel", false);

            Vector2 pos = bigMario.Position;
            pos.Y = tileSize * 13;
            smallMario.Position = pos;

            // Start from BIG logically,
            // Update() will toggle and keep IsBig in sync
            currentMario = bigMario;
            IsBig = true;

            targetBig = false;
            isFlashing = true;
            flashTimer = 0f;
            flashCount = 0;
        }

        // ====================================================
        // HARD RESET
        // ====================================================
        public void ForceSmall(Vector2 spawnPoint)
        {
            isFlashing = false;
            flashTimer = 0f;
            flashCount = 0;
            targetBig = false;

            IsBig = false;
            smallMario.Position = spawnPoint;
            bigMario.Position = spawnPoint;
            currentMario = smallMario;
        }
    }
}
