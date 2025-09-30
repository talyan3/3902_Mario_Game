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
    float delay = 200f;
    int frames;

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
            if (frames >= 3)
            {
                frames = 0;
            }
            else
            {
                frames++;
            }
            elasped = 0;
        }
        int i = 1;
        while (i < 7)
        {
            dRect = new Rectangle(50 + frames , 100, 205, 165);
            sRect = new Rectangle(frames * 32, 0, 32, 20);
            i++;
        }
       
    }
}