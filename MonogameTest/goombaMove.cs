using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

class moveGoom : ISprite
{
    private SpriteBatch _spriteBatch;
    private Texture2D sprite;

    Rectangle sRect;
    Rectangle dRect;
    float elasped;
    //change the delay for different feels
    //looks kinda choppy atm
    //later plan to give every sprite that moves a pos value
    float delay = 150f;
    int frames;
    int walkLeft = 1;
    int walkRight = 1;
    //int currPosGoomX = 100;
    //32 is first walk right frame 
    //64 is neuatral frame
    //96 is walk right frame

    public moveGoom(Texture2D texture, SpriteBatch spriteBatch)
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
            //walking
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
            dRect = new Rectangle(100 + (10 * walkRight), 300, 32, 32);
        }
        else if (walkLeft < 10 && walkRight >= 10)
        {
            walkLeft++;
            dRect = new Rectangle(200 - (10 * walkLeft), 300, 32, 32);
        }
        else
        {
            walkLeft = 1;
            walkRight = 1;
        }
        }

   
    sRect = new Rectangle(frames * 32, 0, 32, 20);
       
    }
}