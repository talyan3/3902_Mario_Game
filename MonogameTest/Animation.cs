using System;
using System.Collections.Generic;

namespace MonogameTest;

public class Animation
{
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

    
}