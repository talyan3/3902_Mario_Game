using Microsoft.Xna.Framework;
using MonogameTest.Sounds;

namespace MonogameTest
{
    public class MarioStateController
    {
        public bool IsBig { get; private set; }
        public bool IsFire { get; private set; }
        public readonly FireMarioSprite fireMario;

        public readonly SmallMarioSprite smallMario;
        public readonly BigMarioSprite bigMario;
        private StaticSprite currentMario;
        private readonly int tileSize;

        private double _fireCooldown = 0;
        public bool FireReady => _fireCooldown <= 0;

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
            FireMarioSprite fire,          
            StaticSprite startingMario,
            int tileSize)
        {
            smallMario = small;
            bigMario = big;
            fireMario = fire;           
            currentMario = startingMario;
            this.tileSize = tileSize;

            IsBig = startingMario == big;
            IsFire = startingMario == fire; 
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
        // GROW (MUSHROOM / POWERUP)
        // ====================================================
        public void Grow()
        {
            // Do not re-enter while flashing
            if ((IsBig && !IsFire) || isFlashing)
                return;

            SoundManager.Instance.PlayEffect("powerUp");

            Vector2 pos = smallMario.Position;
            pos.Y = tileSize * 13;   // might be a problem idk
            bigMario.Position = pos;

            // Start from SMALL logically,
            // Update() will toggle and keep IsBig in sync
            currentMario = smallMario;
            IsBig = false;
            IsFire = false; // Could be the problem

            targetBig = true;
            isFlashing = true;
            flashTimer = 0f;
            flashCount = 0;
        }
        // ====================================================
        // NEW: FIRE FLOWER → Fire Mario
        // ====================================================
        public void Fire()
        {
            if (IsFire) return;

            Vector2 pos = currentMario.Position;
            fireMario.Position = pos;

            currentMario = fireMario;
            IsBig = true;
            IsFire = true;
        }

        // ====================================================
        // FIREBALL COOLDOWN TICK
        // ====================================================
        public void Tick(GameTime gameTime)
        {
            if (_fireCooldown > 0)
                _fireCooldown -= gameTime.ElapsedGameTime.TotalSeconds;
        }

        // ====================================================
        // Attempt to shoot fireball
        // ====================================================
        public bool TryShoot()
        {
            if (!IsFire || !FireReady)
                return false;

            _fireCooldown = 0.35; // 350ms cooldown
            return true;
        }

        // ====================================================
        // SHRINK (HIT BY ENEMY)
        // ====================================================
        public void Shrink()
        {
            if (IsFire)
            {
                Vector2 posF = fireMario.Position;
                bigMario.Position = posF;

                currentMario = bigMario;
                IsBig = true;
                IsFire = false;
                return;
            }
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
        // HARD RESET (DEATH / LEVEL RESET)
        // ====================================================
        public void ForceSmall(Vector2 spawnPoint)
        {
            isFlashing = false;
            flashTimer = 0f;
            flashCount = 0;
            targetBig = false;

            IsBig = false;
            IsFire = false; 
            smallMario.Position = spawnPoint;
            bigMario.Position = spawnPoint;
            fireMario.Position = spawnPoint; 
            currentMario = smallMario;
        }
    }
}
