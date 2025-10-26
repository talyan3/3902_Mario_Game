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

        if (dRect.Width == 0 || dRect.Height == 0)
            dRect = new Rectangle(100, 400, 32, 32);
        if (sRect.Width == 0 || sRect.Height == 0)
            sRect = new Rectangle(0, 0, 30, 24);
    }
    
     public Vector2 Position
    {
        get => new Vector2(dRect.X, dRect.Y);
        set => dRect = new Rectangle((int)value.X, (int)value.Y, dRect.Width == 0 ? 32 : dRect.Width, dRect.Height == 0 ? 32 : dRect.Height);
    }
    public Rectangle Bounds => dRect;
    public Rectangle Region => sRect;
    public Vector2 Scale => Vector2.One;
    
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        _spriteBatch.Draw(sprite,dRect,sRect,Color.White);//
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
