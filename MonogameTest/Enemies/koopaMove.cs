using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

class moveKoop : ISprite
{
   private SpriteBatch _spriteBatch;
    private Texture2D sprite;

    private readonly KoopaNum Numbers = NumberLoad.Numbers.KoopaM;

    Rectangle sRect;
    Rectangle dRect;
    float elasped;
    float delay;
    private const float Speed = 0;
    int frames;
    int walkLeft;
    int walkRight;
    bool walkingR = true;
    public Vector2 Velocity { get; set; } = Vector2.Zero;
    public bool IsAlive { get; set; } = true;
    public double direction = -1; // -1 = left, +1 = right
    float deltaTime;
    int flag = 1;

    public moveKoop(Texture2D texture, SpriteBatch spriteBatch)
    {
        sprite = texture;
        _spriteBatch = spriteBatch;

        if (dRect.Width == 0 || dRect.Height == 0)
            dRect = new Rectangle(0, 0, Numbers.DestWidth, Numbers.DestHeight);
        if (sRect.Width == 0 || sRect.Height == 0)
            sRect = new Rectangle(0, 0, Numbers.SrcWidth, Numbers.SrcHeight);
    }

    public Vector2 Position
    {
        get => new Vector2(dRect.X, dRect.Y);
        set => dRect = new Rectangle((int)value.X, (int)value.Y, dRect.Width == 0 ? Numbers.DestWidth : dRect.Width, dRect.Height == 0 ? Numbers.DestHeight : dRect.Height);
    }
    public Rectangle Bounds => IsAlive ? dRect : Rectangle.Empty;   
    public Rectangle Region => sRect;
    public Vector2 Scale => Vector2.One;
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        if (!IsAlive) return;
        _spriteBatch.Draw(sprite, dRect, sRect, Color.White);//
    }
    
    // Public method to be called by the collision handler in Game1.Update
    public void ReverseDirection()
    {
        direction *= Numbers.Direction;
        flag *=  (int)Numbers.Flag;
    }

    public void Update(GameTime gameTime)
    {
        if (!IsAlive) return;
        delay = Numbers.AnimationDelay;
        Speed = Numbers.Speed;

        deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // 1. Apply Movement: Goomba moves continuously based on its current 'direction'
        Position += new Vector2(Math.Clamp((float)direction, Numbers.ClampMin, Numbers.ClampMax) * Speed * deltaTime, 0);

        // NOTE: Direction is now only reversed externally when collision with a tile occurs.

        // 2. Animation Logic (Time-delayed)
        elasped += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        if (elasped >= delay)
        {
            if (flag == 1)
            {
                // Alternate between animation frames (2 and 3) (moving left)
                frames = ((frames + 1) % Numbers.LeftFrames.Length) + Numbers.LeftFrames.Length;
            }
            if (flag == -1)
            {
                // Alternate between animation frames (4 and 5) (moving right)
                frames = ((frames + 1) % Numbers.RightFrames.Length) + Numbers.RightFrames[0];
            }
            
            elasped = 0f;

            // Update the source rectangle based on the current animation frame
            sRect = new Rectangle(frames * Numbers.SrcWidth, 0, Numbers.SrcWidth, Numbers.SrcHeight);
        }
    }
}