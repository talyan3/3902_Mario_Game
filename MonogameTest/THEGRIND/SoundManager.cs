using System.Reflection.PortableExecutable;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
namespace MonogameTest;
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
        JumpSfx = _content.Load<SoundEffect>("smb_jump-super");
        // LandSfx = _content.Load<SoundEffect>("Audio/land");
        // WalkSfx = _content.Load<SoundEffect>("Audio/walk");
    }
}