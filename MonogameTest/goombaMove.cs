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
    public Vector2 Velocity { get; set; } = Vector2.Zero;   
    public bool IsAlive { get; set; } = true;

    public moveGoom(Texture2D texture, SpriteBatch spriteBatch)
    {
        sprite = texture;
        _spriteBatch = spriteBatch;

        if (dRect.Width == 0 || dRect.Height == 0)
            dRect = new Rectangle(100, 300, 32, 32);
        if (sRect.Width == 0 || sRect.Height == 0)
            sRect = new Rectangle(0, 0, 32, 20);
    }

    public Vector2 Position
    {
        get => new Vector2(dRect.X, dRect.Y);
        set => dRect = new Rectangle((int)value.X, (int)value.Y, dRect.Width == 0 ? 32 : dRect.Width, dRect.Height == 0 ? 32 : dRect.Height);
    }
    public Rectangle Bounds => IsAlive ? dRect : Rectangle.Empty;
    public Rectangle Region => sRect;             
    public Vector2 Scale => Vector2.One; 
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        if (!IsAlive) return;
        _spriteBatch.Draw(sprite,dRect,sRect,Color.White);//
    }


    public void Update(GameTime gameTime)
    {
        if (!IsAlive) return;
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