using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest
{
    class moveKoop : ISprite
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D sprite;

        private Rectangle sRect;
        private Rectangle dRect;

        private float elapsed;
        private const float delay = 150f;

        private const float WalkSpeed = 30f;
        private const float ShellSpeed = 160f;

        private int frames;
        private float deltaTime;

        public bool IsAlive { get; set; } = true;

        // -1 = left, +1 = right
        public double direction = -1;

        // ============================
        // SPRITE FRAME CONSTANTS
        // ============================
        private const int WALK_FRAME_1 = 2;  // normal ground koopa
        private const int WALK_FRAME_2 = 3;
        private const int SHELL_FRAME  = 9;  //  correct NON-winged shell frame

        // ============================
        // KOOPA STATE MACHINE
        // ============================
        private enum KoopaState
        {
            Walking,
            ShellIdle,
            ShellMoving
        }

        private KoopaState state = KoopaState.Walking;

        // ============================
        // CONSTRUCTOR
        // ============================
        public moveKoop(Texture2D texture, SpriteBatch spriteBatch)
        {
            sprite = texture;
            _spriteBatch = spriteBatch;

            dRect = new Rectangle(0, 0, 32, 24);

            frames = WALK_FRAME_1;
            sRect = new Rectangle(frames * 30, 0, 30, 24); //  correct starting sprite
        }

        // ============================
        // PROPERTIES
        // ============================
        public Vector2 Position
        {
            get => new Vector2(dRect.X, dRect.Y);
            set => dRect = new Rectangle((int)value.X, (int)value.Y, dRect.Width, dRect.Height);
        }

        public Rectangle Bounds => IsAlive ? dRect : Rectangle.Empty;
        public Rectangle Region => sRect;
        public Vector2 Scale => Vector2.One;

        // ============================
        // STATE QUERIES
        // ============================
        public bool IsShell => state != KoopaState.Walking;
        public bool IsShellIdle => state == KoopaState.ShellIdle;
        public bool IsShellMoving => state == KoopaState.ShellMoving;
        public bool IsWalking => state == KoopaState.Walking;

        // ============================
        // STATE TRANSITIONS
        // ============================
        public void EnterShell()
        {
            state = KoopaState.ShellIdle;
            direction = 0;

            // FORCE correct shell sprite
            sRect = new Rectangle(SHELL_FRAME * 30, 0, 30, 24);
        }

        public void Kick(int dir)
        {
            state = KoopaState.ShellMoving;
            direction = Math.Sign(dir);
            if (direction == 0) direction = 1;
        }

        public void KickShell(Vector2 marioPos)
        {
            int dir = marioPos.X < Position.X ? 1 : -1;
            Kick(dir);
        }

        public void ReverseDirection()
        {
            direction *= -1;
        }
        public void ResetState()
        {
            IsAlive = true;

            // Reset state machine
            state = KoopaState.Walking;
            direction = -1;

            // Reset animation
            frames = WALK_FRAME_1;
            sRect = new Rectangle(frames * 30, 0, 30, 24);

            elapsed = 0f;
        }

        // ============================
        // UPDATE
        // ============================
        public void Update(GameTime gameTime)
        {
            if (!IsAlive) return;

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (state)
            {
                case KoopaState.Walking:
                    Position += new Vector2((float)direction * WalkSpeed * deltaTime, 0);

                    elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (elapsed >= delay)
                    {
                        frames = (frames == WALK_FRAME_1) ? WALK_FRAME_2 : WALK_FRAME_1;
                        elapsed = 0f;
                        sRect = new Rectangle(frames * 30, 0, 30, 24);
                    }
                    break;

                case KoopaState.ShellMoving:
                    Position += new Vector2((float)direction * ShellSpeed * deltaTime, 0);

                    // LOCK shell sprite while moving
                    sRect = new Rectangle(SHELL_FRAME * 30, 0, 30, 24);
                    break;

                case KoopaState.ShellIdle:
                    // LOCK shell sprite while idle
                    sRect = new Rectangle(SHELL_FRAME * 30, 0, 30, 24);
                    break;
            }
        }

        // ============================
        // DRAW
        // ============================
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (!IsAlive) return;
            _spriteBatch.Draw(sprite, dRect, sRect, Color.White);
        }
    }
}