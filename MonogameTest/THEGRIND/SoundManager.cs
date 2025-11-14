using System.Reflection.PortableExecutable;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

public class SoundManager
{
    private readonly ContentManager _content;

    public SoundEffect JumpSfx;
    public SoundEffect LandSfx;
    public SoundEffect WalkSfx;

    public SoundManager(ContentManager content)
    {
        _content = content;
    }

    //here to update when we do the sound effects
    public void LoadContent()
    {
        // JumpSfx = _content.Load<SoundEffect>("Audio/jump");
        // LandSfx = _content.Load<SoundEffect>("Audio/land");
        // WalkSfx = _content.Load<SoundEffect>("Audio/walk");
    }
}