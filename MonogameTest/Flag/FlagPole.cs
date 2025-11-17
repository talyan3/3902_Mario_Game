using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameTest.Sounds;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest
{
    public class FlagPole
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

            _flagRect = new Rectangle(poleRect.Right - flagTexture.Width, poleRect.Top, flagTexture.Width, flagTexture.Height);

            _mario = mario;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_mario is BigMarioSprite bm)
            {
                    marioBounds = bm.Bounds;
            }
            else if (_mario is SmallMarioSprite sm)
            {
                marioBounds = sm.Bounds;
            }

                //check collision with pole
                Point mtv;
                var collisionType = _collision.GetCollision(marioBounds, _poleRect);
                if (collisionType == typeCollision.Left || collisionType == typeCollision.Right)
                {
                    _isSliding = true;
                }

            if (_isSliding)
            {

                Vector2 marioPos = (_mario is BigMarioSprite bm2) ? bm2.Position :
                                   (_mario is SmallMarioSprite sm2) ? sm2.Position :
                                   Vector2.Zero;

                marioPos.X = _poleRect.Center.X;

                marioPos.Y += _slideSpeed * dt;

                float poleBottomY = _poleRect.Bottom;

                if (marioPos.Y > poleBottomY)
                {
                    marioPos.Y = poleBottomY;
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
