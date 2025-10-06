using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

class moveKoop : ISprite
{
   private SpriteBatch _spriteBatch;
    private Texture2D sprite;

    Rectangle sRect;
    Rectangle dRect;
    float elasped;
    float delay = 175f;
    int frames;
    int walkLeft = 1;
    int walkRight = 1;
    bool walkingR = true;

    public moveKoop(Texture2D texture, SpriteBatch spriteBatch)
    {
        sprite = texture;
        _spriteBatch = spriteBatch;
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        _spriteBatch.Begin();//
        _spriteBatch.Draw(sprite,dRect,sRect,Color.White);//
        _spriteBatch.End();//
    }


    public void Update(GameTime gameTime)
    {
        elasped += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        if (elasped >= delay)
        {
            if (frames >= 1)
            {
                frames = 0;
            }
            else
            {
                frames++;
            }
            elasped = 0;
            if (walkRight < 10)
            {
                walkRight++;
                dRect = new Rectangle(100 + (10 * walkRight), 400, 32, 32);
                walkingR = true;
            }
            else if (walkLeft < 10 && walkRight >= 10)
            {
                walkLeft++;
                dRect = new Rectangle(200 - (10 * walkLeft), 400, 32, 32);
                walkingR = false;
            }
            else
            {
                walkLeft = 1;
                walkRight = 1;
            }
        }
        if (walkingR)
        {
            sRect = new Rectangle((4 + frames) * 30, 0, 30, 24);
        }
        else
        { 
            sRect = new Rectangle((2 + frames) * 30, 0, 30, 24);
        }
    }
}