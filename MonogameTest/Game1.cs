using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonogameTest;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public MarioManager MarioManager { get; set; } = new MarioManager();
    public CommandManager CommandManager { get; set; }
    private BlockManager blockManager; // ********

    private PowerupManager powerupManager; // ********

    KeyboardState previousState; // ********

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        CommandManager = new CommandManager(this, MarioManager);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D blocksTexture = Texture2D.FromFile(GraphicsDevice, "blocks-Final.png"); // *******
        blockManager = new BlockManager(blocksTexture); // **********
        Texture2D powerupTexture = Texture2D.FromFile(GraphicsDevice, "powerups.png"); // *******
        powerupManager = new PowerupManager(powerupTexture); // **********

        // TODO: use this.Content to load your game content here
        new SpriteCommand(GraphicsDevice, MarioManager).Execute();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        CommandManager.checkKeys();
        CommandManager.checkClicks();
        if (MarioManager.ActiveSprite != null) MarioManager.ActiveSprite.Update(gameTime);
        //Draw(gameTime);

        KeyboardState state = Keyboard.GetState(); // ********

        if (state.IsKeyDown(Keys.P) && !previousState.IsKeyDown(Keys.P)) // *******
        {
            blockManager.NextBlock();
            powerupManager.NextPowerup();
        }
        if (state.IsKeyDown(Keys.O) && !previousState.IsKeyDown(Keys.O))
        {
            blockManager.PreviousBlock();
            powerupManager.PreviousPowerup();
        }
        previousState = state; // *************

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        if (MarioManager.ActiveSprite != null)
        {
            int h = GraphicsDevice.Viewport.Height;
            int w = GraphicsDevice.Viewport.Width;
            MarioManager.ActiveSprite.Draw(_spriteBatch, new Vector2(w/2, h/2));
        }
        blockManager.Draw(_spriteBatch, new Vector2(100, 100)); // ********
        powerupManager.Draw(_spriteBatch, new Vector2(200, 100)); // ********

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
