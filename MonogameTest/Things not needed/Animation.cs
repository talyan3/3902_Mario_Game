using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public class Animation
{
    private Texture2D idleTexture;
    private int v1;
    private int v2;
    private int v3;
    private float v4;
    private bool v5;

    // Code from MonoGame tutorial
    public List<TextureRegion> Frames { get; set; }
    public TimeSpan Delay { get; set; }
    public Animation()
    {
        Frames = new List<TextureRegion>();
        Delay = TimeSpan.FromMilliseconds(100);
    }

    public Animation(List<TextureRegion> frames)
    {
        Frames = frames;
        Delay = TimeSpan.FromMilliseconds(100);
    }

    public Animation(List<TextureRegion> frames, TimeSpan delay)
    {
        Frames = frames;
        Delay = delay;
    }

    public Animation(Texture2D idleTexture, int v1, int v2, int v3, float v4, bool v5)
    {
        this.idleTexture = idleTexture;
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;
        this.v4 = v4;
        this.v5 = v5;
    }
}