using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

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
    private SmallMarioSprite _smallMario;
    private BigMarioSprite _bigMario;
	private StaticSprite _currentMario;
	private bool _isBig = false;
    private bool _bHeldLast = false;
    private Texture2D _tileset;
    private List<Tile> _mapTiles;
    const int TilesVisibleX = 16;
    const int TileSize = 16;// can create level class
    KeyboardState previousState;
    const int ViewWidth = TilesVisibleX * TileSize; // 256
    private ICamera camera;
    const int scale = 4;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
         CommandManager = new CommandManager(this, MarioManager);
        _graphics.PreferredBackBufferWidth = ViewWidth * scale; // 256 pixels
        _graphics.PreferredBackBufferHeight = 240 * scale;      // typical NES height
        _graphics.ApplyChanges();
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
        goom = new moveGoom(goombaSprite, _spriteBatch);
        koop = new moveKoop(koopaSprite, _spriteBatch);

        // TODO: use this.Content to load your game content here
        new SpriteCommand(GraphicsDevice, MarioManager).Execute();
        
        _smallMario = new SmallMarioSprite(GraphicsDevice); //Added
        _bigMario = new BigMarioSprite(GraphicsDevice);
        
        // Start Mario somewhere reasonable in world coordinates (e.g., ground level)
        var pos = new Vector2(16 * 5, 16 * 13); // y = 13 tiles down instead of 20

        _smallMario.Position = pos;
        _bigMario.Position = pos;

        // start the game with small mario active
        _currentMario = _smallMario;
        
        // Load the tileset image directly
        using (FileStream fs = new FileStream("blocksV10.png", FileMode.Open))
        {
            _tileset = Texture2D.FromStream(GraphicsDevice, fs);
        }

        // Load the map JSON
        string mapPath = Path.Combine(Directory.GetCurrentDirectory(), "level1.json");

        // Load map tiles
        _mapTiles = TiledMapLoader.Load(mapPath, _tileset);

        //loads the camera on mario
        camera = new CameraManager(GraphicsDevice.Viewport);
        camera.Reset(_currentMario.Position); // start centered on Mario
        camera.LookAt(_currentMario.Position); // immediately focus on him

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        CommandManager.checkKeys();
        CommandManager.checkClicks();
        if (MarioManager.ActiveSprite != null) MarioManager.ActiveSprite.Update(gameTime);

        KeyboardState state = Keyboard.GetState(); 

        if (state.IsKeyDown(Keys.P) && !previousState.IsKeyDown(Keys.P)) 
        {
            blockManager.NextBlock();
            powerupManager.NextPowerup();
        }
        if (state.IsKeyDown(Keys.O) && !previousState.IsKeyDown(Keys.O))
        {
            blockManager.PreviousBlock();
            powerupManager.PreviousPowerup();
        }
        previousState = state; 

        bool bDown = state.IsKeyDown(Keys.B);
		if (bDown && !_bHeldLast)
			ToggleMarioSize();
		_bHeldLast = bDown;

        _currentMario.Update(gameTime);
        Vector2 marioPos = _currentMario is SmallMarioSprite sm ? sm.Position :
                   _currentMario is BigMarioSprite bm ? bm.Position :
                   Vector2.Zero;
        camera.LookAt(marioPos);

        if (MarioManager.ActiveSprite != null)
			MarioManager.ActiveSprite.Update(gameTime);

        goom.Update(gameTime);
        koop.Update(gameTime);

        base.Update(gameTime);
    }
    
    private void ToggleMarioSize()
	{
		_isBig = !_isBig;

		// keep mario in the same spot when switching forms
		Vector2 pos = (_currentMario is SmallMarioSprite sm ? sm.Position :
					   (_currentMario is BigMarioSprite bm ? bm.Position : Vector2.Zero));

		_currentMario = _isBig ? (StaticSprite)_bigMario : _smallMario;

		if (_currentMario is SmallMarioSprite sm2) sm2.Position = pos;
		if (_currentMario is BigMarioSprite bm2) bm2.Position = pos;
	}

    protected override void Draw(GameTime gameTime) 
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
        _currentMario.Draw(_spriteBatch, _currentMario.Position);

        // For each tile, draw it
        foreach (var tile in _mapTiles)
        {
            tile.Draw(_spriteBatch);
        }
        //_spriteBatch.DrawRectangle(new Rectangle(0, 0, 256, 240), Color.Red);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
