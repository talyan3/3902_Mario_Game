using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

public static class Assets
{
    public static Texture2D PlayerIdle;
    public static Texture2D PlayerRun;
    public static SoundEffect JumpSound;

    public static void Load(ContentManager _content, GraphicsDevice graphicsDevice)
    {
        PlayerIdle = Texture2D.FromFile(graphicsDevice, "small-mario-final.png");
        //PlayerRun  = _content.Load<Texture2D>("Sprites/Run");
        //JumpSound  = _content.Load<SoundEffect>("Sounds/Jump");
    }
}