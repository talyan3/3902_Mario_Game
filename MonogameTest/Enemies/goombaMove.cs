using System;
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
    float delay = 150f;
    private const float Speed = 30f;
    int frames;
    int walkLeft = 1;
    int walkRight = 1;
    public Vector2 Velocity { get; set; } = Vector2.Zero;
    public bool IsAlive { get; set; } = true;
    public double direction = -1; // -1 = left, +1 = right
    float deltaTime;
    private Texture2D goombaDead;

    public moveGoom(Texture2D texture, SpriteBatch spriteBatch)
    {
        sprite = texture;
        _spriteBatch = spriteBatch;

        if (dRect.Width == 0 || dRect.Height == 0)
            dRect = new Rectangle(0, 0, 32, 16);
        if (sRect.Width == 0 || sRect.Height == 0)
            sRect = new Rectangle(0, 0, 32, 16);
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
        _spriteBatch.Draw(sprite, dRect, sRect, Color.White);//
    }

    // Public method to be called by the collision handler in Game1.Update
    public void ReverseDirection()
    {
        direction *= -2;
    }

    public void Update(GameTime gameTime)
    {
        if (!IsAlive) 
        {
           // goombaDead = Texture2D.FromFile(GraphicsDevice, "coin2.png");
            return;
        }
        deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // 1. Apply Movement: Goomba moves continuously based on its current 'direction'
        Position += new Vector2(Math.Clamp((float)direction, -2f, 2f) * Speed * deltaTime, 0);

        // NOTE: Direction is now only reversed externally when collision with a tile occurs.

        // 2. Animation Logic (Time-delayed)
        elasped += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        if (elasped >= delay)
        {
            // Alternate between animation frames (0 and 1)
            frames = (frames + 1) % 2;
            elasped = 0f;

            // Update the source rectangle based on the current animation frame
            sRect = new Rectangle(frames * 32, 0, 32, 16);
        }
    }
}