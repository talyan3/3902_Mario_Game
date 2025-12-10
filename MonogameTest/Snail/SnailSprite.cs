using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.Snail
{
    public class SnailSprite
    {
        private Texture2D _frame1;
        private Texture2D _frame2;

        private float _timer;
        private float _animationSpeed = 0.25f; // smoother, faster animation
        private int _currentFrame = 0;

        private SpriteEffects _flip = SpriteEffects.None;

        public Texture2D CurrentTexture => 
         (_currentFrame == 0 ? _frame1 : _frame2);


        public SnailSprite(Texture2D frame1, Texture2D frame2)
        {
            _frame1 = frame1;
            _frame2 = frame2;
        }

        public void Update(GameTime gameTime, Vector2 snailPos, Vector2 marioPos)
        {
            // Flip based on chase direction
            if (marioPos.X < snailPos.X)
                _flip = SpriteEffects.FlipHorizontally;
            else
                _flip = SpriteEffects.None;

            // Smooth animation
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= _animationSpeed)
            {
                _timer = 0;
                _currentFrame = (_currentFrame + 1) % 2; 
            }
        }

        public void Draw(SpriteBatch sb, Vector2 position)
        {
            Texture2D tex = (_currentFrame == 0) ? _frame1 : _frame2;

            // Bottom-center origin so snail sits on ground like Mario
            Vector2 origin = new Vector2(tex.Width / 2f, tex.Height);

            sb.Draw(
                tex,
                position,
                null,
                Color.White,
                0f,
                origin,
                1f,
                _flip,
                0f
            );
        }

    }
}
