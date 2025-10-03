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
    float delay = 200f;
    int frames;

    public moveKoop(Texture2D texture, SpriteBatch spriteBatch)
    {
        sprite = texture;
        _spriteBatch = spriteBatch;
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        //_spriteBatch.Begin();//
        _spriteBatch.Draw(sprite,dRect,sRect,Color.White);//
        //_spriteBatch.End();//
    }


    public void Update(GameTime gameTime)
    {
        elasped += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        if (elasped >= delay)
        {
            if (frames >= 2)
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
            dRect = new Rectangle(50 + i, 200, 205, 165);
            sRect = new Rectangle((4 + frames) * 30, 0, 30, 24);
            i++;
        }
    }
}