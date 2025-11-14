using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Animation
{
    public Texture2D Texture { get; }
    public int FWidth { get; }
    public int FHeight { get; }
    public int FCount { get; }
    public float FSpeed { get; }    
    public int FStart { get; }

    public Animation(Texture2D texture, int frameWidth, int frameHeight, int frameCount, float frameSpeed, int frameStart)
    {
        Texture = texture;
        FWidth = frameWidth;
        FHeight = frameHeight;
        FCount = frameCount;
        FSpeed = frameSpeed;
        FStart = frameStart;
    }

    public Rectangle GetFrameRect(int frameIndex)
    {
        int currFrame = FStart + frameIndex;
        return new Rectangle(currFrame * FWidth, 0, FWidth, FHeight);
    }
}