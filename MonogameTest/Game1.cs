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
    public Texture2D goombaSprite;
    public Texture2D koopaSprite;
    public ISprite goom;
    public ISprite koop;
    public Vector2 pos;

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
        goombaSprite = Content.Load<Texture2D>("Sprites/goomba-Final");
        koopaSprite = Content.Load<Texture2D>("Sprites/green-koopa");
        // TODO: use this.Content to load your game content here
        goom = new moveGoom(goombaSprite,_spriteBatch);
        koop = new moveKoop(koopaSprite,_spriteBatch);

        // TODO: use this.Content to load your game content here
        new SpriteCommand(GraphicsDevice, MarioManager).Execute();
    }

    protected override void Update(GameTime gameTime) // TODO - seperate class for keyboard input: Anika
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

        goom.Update(gameTime);
        koop.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) // Think about how to introduce several blocks beyond 1 to prevent drawing to game every time.
    // Get grid system class that loops over block calls from external file using enum to decide what is drawn on each tile.
    // Think about ways to make 'shortcuts' in code, grouping and simplifiying things where you can, especially for collision which is expensive
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
        blockManager.Draw(_spriteBatch, new Vector2(500, 100)); // ********
        powerupManager.Draw(_spriteBatch, new Vector2(575, 100)); // ********
        goom.Draw(_spriteBatch,pos);
        koop.Draw(_spriteBatch,pos);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
