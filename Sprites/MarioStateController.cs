using Microsoft.Xna.Framework;

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


        // ====================================================
        // GROW (MUSHROOM / POWERUP)
        // ====================================================
        public void Grow()
        {
            if (IsBig && !IsFire) return;

            Vector2 pos = smallMario.Position;
            bigMario.Position = pos;
            currentMario = bigMario;

            IsBig = true;
            IsFire = false;
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
        // NEW: FIREBALL COOLDOWN TICK
        // ====================================================
        public void Tick(GameTime gameTime)
        {
            if (_fireCooldown > 0)
                _fireCooldown -= gameTime.ElapsedGameTime.TotalSeconds;
        }

        // ====================================================
        // NEW: Attempt to shoot fireball
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
                Vector2 pos = fireMario.Position;
                bigMario.Position = pos;

                currentMario = bigMario;
                IsBig = true;
                IsFire = false;
                return;
            }

            if (!IsBig) return;

            Vector2 pos2 = bigMario.Position;
            pos2.Y = tileSize * 13;

            smallMario.Position = pos2;
            currentMario = smallMario;
            IsBig = false;
        }

        // ====================================================
        // HARD RESET (DEATH / LEVEL RESET)
        // ====================================================
        public void ForceSmall(Vector2 spawnPoint)
        {
            IsBig = false;
            IsFire = false; 

            smallMario.Position = spawnPoint;
            bigMario.Position = spawnPoint;
            fireMario.Position = spawnPoint; 

            currentMario = smallMario;
        }

        
    }
}

