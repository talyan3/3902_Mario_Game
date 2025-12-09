using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace MonogameTest.Snail
{
    public class Snail
    {
        private readonly SnailSprite _sprite;
        private readonly SnailBehavior _behavior;
        private readonly Texture2D _texture1;
        private readonly Texture2D _texture2;

        public Vector2 Position;
        public bool Active => Game1.ChristmasMode; // Only exists during Christmas mode

        // Heartbeat behavior settings
        private float _heartbeatMaxDistance = 600f; // beyond this = silence
        private float _heartbeatMinDistance = 80f;  // very close = max volume
        private float _heartbeatMaxVolume = 1.0f;   // loudest
        private float _heartbeatMinVolume = 0.0f;   // silent

        private SoundEffectInstance _heartbeatInstance;

        public Snail(Texture2D t1, Texture2D t2, Vector2 startPosition)
        {
            _texture1 = t1;
            _texture2 = t2;

            Position = startPosition;
            _sprite = new SnailSprite(_texture1, _texture2);
            _behavior = new SnailBehavior();

            // Create heartbeat instance (looping)
            _heartbeatInstance = SoundManager.Instance.CreateEffectInstance("heartbeat", looped: true);
            if (_heartbeatInstance != null)
            {
                _heartbeatInstance.Volume = 0f; // start silent
                _heartbeatInstance.Play();
            }
        }

        public void Update(GameTime gameTime, Vector2 marioPos)
        {
            // If not in Christmas mode → silence heartbeat + do nothing
            if (!Active)
            {
                if (_heartbeatInstance != null)
                    _heartbeatInstance.Volume = 0f;

                return;
            }

            // Move the snail
            Position = _behavior.UpdateSnail(
                Position,
                marioPos,
                (float)gameTime.ElapsedGameTime.TotalSeconds
            );

            // Update animation
            _sprite.Update(gameTime, Position, marioPos);

            // Adjust heartbeat loudness
            UpdateHeartbeatVolume(marioPos);
        }

        public void Draw(SpriteBatch sb)
        {
            if (!Active) return;
            _sprite.Draw(sb, Position);
        }

        private void UpdateHeartbeatVolume(Vector2 marioPos)
        {
            if (_heartbeatInstance == null)
                return;

            float distance = Vector2.Distance(Position, marioPos);

            float volume;

            if (distance >= _heartbeatMaxDistance)
            {
                volume = _heartbeatMinVolume;
            }
            else if (distance <= _heartbeatMinDistance)
            {
                volume = _heartbeatMaxVolume;
            }
            else
            {
                float t = (distance - _heartbeatMinDistance) /
                          (_heartbeatMaxDistance - _heartbeatMinDistance);

                volume = MathHelper.Lerp(_heartbeatMaxVolume, _heartbeatMinVolume, t);
            }

            _heartbeatInstance.Volume = MathHelper.Clamp(volume, 0f, 1f);
        }
    }
}
