using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonogameTest;

public class CommandManager
{
    public KeyboardController KeyboardController { get; } = new KeyboardController();
    public MouseController MouseController { get; } = new MouseController();
    private DisplayMode DisplayMode;
    private GraphicsDevice GraphicsDevice;
    private MarioManager MarioManager;
    private Game Game;

    // Mappings declaration
    private KeyCombo _0KeyCombo = new KeyCombo(Keys.D0, KeyAction.PRESS);
    private KeyCombo _1KeyCombo = new KeyCombo(Keys.D1, KeyAction.PRESS);
    private KeyCombo _2KeyCombo = new KeyCombo(Keys.D2, KeyAction.PRESS);
    private KeyCombo _3KeyCombo = new KeyCombo(Keys.D3, KeyAction.PRESS);
    private KeyCombo _4KeyCombo = new KeyCombo(Keys.D4, KeyAction.PRESS);
    private KeyCombo num0KeyCombo = new KeyCombo(Keys.NumPad0, KeyAction.PRESS);
    private KeyCombo num1KeyCombo = new KeyCombo(Keys.NumPad1, KeyAction.PRESS);
    private KeyCombo num2KeyCombo = new KeyCombo(Keys.NumPad2, KeyAction.PRESS);
    private KeyCombo num3KeyCombo = new KeyCombo(Keys.NumPad3, KeyAction.PRESS);
    private KeyCombo num4KeyCombo = new KeyCombo(Keys.NumPad4, KeyAction.PRESS);
    


    private MouseCombo _1QuadCombo;
    private MouseCombo _2QuadCombo;
    private MouseCombo _3QuadCombo;
    private MouseCombo _4QuadCombo;
    private MouseCombo _quitCombo = new MouseCombo(MouseAction.RIGHT_CLICK);





    public CommandManager(Game game, MarioManager marioManager)
    {
        KeyboardController = new KeyboardController();
        MouseController = new MouseController();
        GraphicsDevice = game.GraphicsDevice;
        DisplayMode = GraphicsDevice.DisplayMode;
        MarioManager = marioManager;
        populateMouseCombos();
        populateControllers();
    }

    private void populateMouseCombos()
    {
        int h = GraphicsDevice.Viewport.Height;
        int w = GraphicsDevice.Viewport.Width;
        Location screenCenter = new Location(w / 2, h / 2);
        Location topMiddleScreen = new Location(w / 2, 0);
        Location leftMiddleScreen = new Location(0, h / 2);
        Location rightMiddleScreen = new Location(w, h / 2);
        Location bottomMiddleScreen = new Location(w / 2, h);
        Location bottomRightScreen = new Location(w, h);

        _1QuadCombo = new MouseCombo(MouseAction.LEFT_CLICK, new Location(), screenCenter);
        _2QuadCombo = new MouseCombo(MouseAction.LEFT_CLICK, topMiddleScreen, rightMiddleScreen);
        _3QuadCombo = new MouseCombo(MouseAction.LEFT_CLICK, leftMiddleScreen, bottomMiddleScreen);
        _4QuadCombo = new MouseCombo(MouseAction.LEFT_CLICK, screenCenter, bottomRightScreen);
    }

    private void populateControllers()
    {
        SpriteBatch SpriteBatch = new SpriteBatch(GraphicsDevice);
        KeyboardController.addMapping(_0KeyCombo, new QuitCommand(Game));
        KeyboardController.addMapping(_1KeyCombo, new SpriteCommand(GraphicsDevice, MarioManager));
        KeyboardController.addMapping(_2KeyCombo, new SpriteCommand(2, GraphicsDevice, MarioManager));
        KeyboardController.addMapping(_3KeyCombo, new SpriteCommand(3, GraphicsDevice, MarioManager));
        KeyboardController.addMapping(_4KeyCombo, new SpriteCommand(4, GraphicsDevice, MarioManager));

        KeyboardController.addMapping(num0KeyCombo, new QuitCommand(Game));
        KeyboardController.addMapping(num1KeyCombo, new SpriteCommand(GraphicsDevice, MarioManager));
        KeyboardController.addMapping(num2KeyCombo, new SpriteCommand(2, GraphicsDevice, MarioManager));
        KeyboardController.addMapping(num3KeyCombo, new SpriteCommand(3, GraphicsDevice, MarioManager));
        KeyboardController.addMapping(num4KeyCombo, new SpriteCommand(4, GraphicsDevice, MarioManager));

        MouseController.addMapping(_quitCombo, new QuitCommand(Game));
        MouseController.addMapping(_1QuadCombo, new SpriteCommand(GraphicsDevice, MarioManager));
        MouseController.addMapping(_2QuadCombo, new SpriteCommand(2, GraphicsDevice, MarioManager));
        MouseController.addMapping(_3QuadCombo, new SpriteCommand(3, GraphicsDevice, MarioManager));
        MouseController.addMapping(_4QuadCombo, new SpriteCommand(4, GraphicsDevice, MarioManager));
    }

    public void checkKeys()
    {
        KeyboardController.checkKeys();
    }

    public void checkClicks()
    {
        MouseController.checkClicks();
    }

}