using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameTest.Sounds;

namespace MonogameTest
{
    public class Flagpole
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _flagTexture;
        private Rectangle _poleRect;
        private Rectangle _flagRect;

        private ISprite _mario;
        private bool _isSliding = false;
        private float _slideSpeed = 60f;

        private DetectCollisions _collision = new DetectCollisions();

        public Flagpole(SpriteBatch spriteBatch, Texture2D flagTexture, Rectangle poleRect, ISprite mario)
        {
            _spriteBatch = spriteBatch;
            _flagTexture = flagTexture;
            _poleRect = poleRect;

            _flagRect = new Rectangle(
                poleRect.Right - flagTexture.Width,
                poleRect.Top,
                flagTexture.Width,
                flagTexture.Height);

            _mario = mario;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Rectangle marioBounds = Rectangle.Empty;

            if (_mario is BigMarioSprite bm)
                marioBounds = bm.Bounds;
            else if (_mario is SmallMarioSprite sm)
                marioBounds = sm.Bounds;

            // Detect collision with pole
            var collisionType = _collision.GetCollision(marioBounds, _poleRect, out Point mtv);

            if (collisionType == typeCollision.Left ||
                collisionType == typeCollision.Right)
            {
                _isSliding = true;
            }

            if (_isSliding)
            {
                Vector2 marioPos =
                    (_mario is BigMarioSprite bm2) ? bm2.Position :
                    (_mario is SmallMarioSprite sm2) ? sm2.Position :
                    Vector2.Zero;

                marioPos.X = _poleRect.Center.X;
                marioPos.Y += _slideSpeed * dt;

                float bottom = _poleRect.Bottom;
                if (marioPos.Y > bottom)
                {
                    marioPos.Y = bottom;
                    _isSliding = false;
                }

                if (_mario is BigMarioSprite bm3) bm3.Position = marioPos;
                if (_mario is SmallMarioSprite sm3) sm3.Position = marioPos;

                _flagRect.Y = (int)marioPos.Y - _flagTexture.Height;
            }
        }

        public void Draw()
        {
            _spriteBatch.Draw(_flagTexture, _flagRect, Color.White);
        }
    }
}
