using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public class SpriteCommand : ICommand
{
    int key;
    public GraphicsDevice GraphicsDevice { get; set; }
    public MarioManager MarioManager { get; set; }

    private SpriteCommand() { }

    public SpriteCommand(GraphicsDevice graphicsDevice, MarioManager marioManager)
    {
        key = 1;
        GraphicsDevice = graphicsDevice;
        MarioManager = marioManager;
    }
    public SpriteCommand(int key, GraphicsDevice graphicsDevice, MarioManager marioManager)
    {
        this.key = key;
        GraphicsDevice = graphicsDevice;
        MarioManager = marioManager;
    }

    public void Execute()
    {
        ISprite sprite = getSprite(key);
        MarioManager.ActiveSprite = sprite;
    }

    private ISprite getSprite(int key)
    {
        Texture2D texture = Texture2D.FromFile(GraphicsDevice, "mario.png");
        ISprite toReturn;
        switch (key)
        {
            case 1:
            default:
                toReturn = new StaticMarioSprite(GraphicsDevice);
                break;
            case 2:
                toReturn = new AnimatedMarioSprite(new TextureRegion(texture, 100, 0, 28, 28), getMarioAnimation());
                break;
            case 3:
                toReturn = new StaticMovingMarioSprite(GraphicsDevice);
                break;
            case 4:
                toReturn = new AnimatedMovingMarioSprite(new TextureRegion(texture, 100, 0, 28, 28), getMarioAnimation());
                break;
        }
        return toReturn;
    }

    private Animation getMarioAnimation()
    {
        Texture2D texture = Texture2D.FromFile(GraphicsDevice, "mario.png");
        List<TextureRegion> frames = new List<TextureRegion>();
        for (int i = 5; i >2; i--)
        {
            frames.Add(new TextureRegion(texture, 30*i, 0, 28, 28));
        }
        return new Animation(frames);
    }
}
