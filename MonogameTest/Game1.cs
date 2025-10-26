using System;
using System.IO;
using System.Collections.Generic;
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
    private SmallMarioSprite _smallMario;
    private BigMarioSprite _bigMario;
	private StaticSprite _currentMario;
	private bool _isBig = false;
    private bool _bHeldLast = false;
    private Texture2D _tileset;
    private List<Tile> _mapTiles;
    const int TilesVisibleX = 16;
    const int TileSize = 16;
    const int ViewWidth = TilesVisibleX * TileSize; // 256
    private Camera2D camera;

    private List<Rectangle> _solidRects; //Added

    const int scale = 5;

    KeyboardState previousState; // ********

    private Vector2 _spawnPoint; //Added

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        CommandManager = new CommandManager(this, MarioManager);
        _graphics.PreferredBackBufferWidth = 800; // 256 pixels
        _graphics.PreferredBackBufferHeight = 600;      // typical NES height
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
        
        var vp = GraphicsDevice.Viewport;//Added
        var pos = new Vector2(vp.Width * 0.5f, vp.Height * 0.35f);

		_smallMario.Position = pos;
        _bigMario.Position = pos;
        
        _spawnPoint = pos; //JAdded

        // start the game with small mario active
        _currentMario = _smallMario;
        
        // Load the tileset image directly from disk (same folder as .cs files)
        using (FileStream fs = new FileStream("blocksV2.png", FileMode.Open))
        {
            _tileset = Texture2D.FromStream(GraphicsDevice, fs);
        }

        // Load the map JSON (same folder)
        string mapPath = Path.Combine(Directory.GetCurrentDirectory(), "level1.json");

        // tilesPerRow = (tilesetWidth / tileWidth)
        _mapTiles = TiledMapLoader.Load(mapPath, _tileset, tilesPerRow: 11);

        /**** JAdded  ****/
        // After loading your map tiles:
    _solidRects = new List<Rectangle>(_mapTiles.Count);
    foreach (var tile in _mapTiles)
        _solidRects.Add(tile.Bounds); 
    /****  JEnd Added  ****/

        camera = new Camera2D(GraphicsDevice.Viewport);
        camera.LookAt(Vector2.Zero);
    }

    protected override void Update(GameTime gameTime) // TODO - seperate class for keyboard input: Anika
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        CommandManager.checkKeys();
        CommandManager.checkClicks();
        if (MarioManager.ActiveSprite != null) MarioManager.ActiveSprite.Update(gameTime);

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

        bool bDown = state.IsKeyDown(Keys.B);
		if (bDown && !_bHeldLast)
			ToggleMarioSize();
		_bHeldLast = bDown;

        _currentMario.Update(gameTime);

        // JAdded === Static ground/wall collision handling ===
       if (StaticCollisionHandler.HandleMany(_currentMario, _mapTiles, out var res, out var hitTile))
{
    Console.WriteLine($"Mario hit {hitTile.TileName} (gid={hitTile.Gid}) at {hitTile.Position} | Side={res.Side} | MTV={res.MTV}");
}

        if (MarioManager.ActiveSprite != null)
			MarioManager.ActiveSprite.Update(gameTime);

        goom.Update(gameTime);
        koop.Update(gameTime);

// Added this
        if (goom is moveGoom g)
        {
            if (EnemyCollisionHandler.HandleMany(g, _mapTiles, out var gRes, out var gTile))
            {
                if (gRes.HitWall)   Console.WriteLine($"Goomba hit wall at {gRes.TileRect.Location}");
                if (gRes.Grounded)  Console.WriteLine("Goomba grounded");
                if (gRes.BonkedHead)Console.WriteLine("Goomba bonked head");

                Console.WriteLine(
                    $"[Collision] Enemy=Goomba  Side={gRes.Side}  MTV={gRes.MTV}  TilePixel={gRes.TileRect.Location}");

            }
        }

        if (koop is moveKoop k)
        {
            if (EnemyCollisionHandler.HandleMany(k, _mapTiles, out var kRes, out var kTile))
            {
                if (kRes.HitWall) Console.WriteLine($"Koopa hit wall at {kRes.TileRect.Location}");
                if (kRes.Grounded) Console.WriteLine("Koopa grounded");
                if (kRes.BonkedHead) Console.WriteLine("Koopabonked head");
                Console.WriteLine(
                    $"[Collision] Enemy=Koopa   Side={kRes.Side}  MTV={kRes.MTV}  TilePixel={kRes.TileRect.Location}");

            }
        }
    

    
    var enemies = new List<object>();
    if (goom is moveGoom g2 && g2.IsAlive) enemies.Add(g2);
    if (koop is moveKoop k2 && k2.IsAlive) enemies.Add(k2);

    if (_isBig)
    {
        EnemyCollisionHandler.HandleMarioEnemyCollision(
            _bigMario,
            enemies,
            onBigHit: () =>
            {
                _isBig = false;
                _smallMario.Position = _bigMario.Position;
                var deltaFeet = _bigMario.Bounds.Bottom - _smallMario.Bounds.Bottom;
                _smallMario.Position = new Vector2(_smallMario.Position.X, _smallMario.Position.Y + deltaFeet);
                _currentMario = _smallMario;
            });
    }
    else
    {
        EnemyCollisionHandler.HandleMarioEnemyCollision(
            _smallMario,
            enemies,
            restart: () =>
            {
                _currentMario = _smallMario;
                _smallMario.Position = _spawnPoint;

                if (goom is moveGoom gg) gg.IsAlive = true;
                if (koop is moveKoop kk) kk.IsAlive = true;
            });
    
}
    

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

    protected override void Draw(GameTime gameTime) // Think about how to introduce several blocks beyond 1 to prevent drawing to game every time.
    // Get grid system class that loops over block calls from external file using enum to decide what is drawn on each tile.
    // Think about ways to make 'shortcuts' in code, grouping and simplifiying things where you can, especially for collision which is expensive
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
        _currentMario.Draw(_spriteBatch, Vector2.Zero);
        
        //if (MarioManager.ActiveSprite != null)
        //{
            //int h = GraphicsDevice.Viewport.Height;
            //int w = GraphicsDevice.Viewport.Width;
            //MarioManager.ActiveSprite.Draw(_spriteBatch, new Vector2(w / 2, h / 2));
        //}
        //blockManager.Draw(_spriteBatch, new Vector2(500, 100)); // ********
        //powerupManager.Draw(_spriteBatch, new Vector2(575, 100)); // ********
        //goom.Draw(_spriteBatch, pos);
        //koop.Draw(_spriteBatch, pos);

        foreach (var tile in _mapTiles)
        {
            tile.Draw(_spriteBatch);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

