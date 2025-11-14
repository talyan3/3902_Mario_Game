using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;

namespace MonogameTest;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
   
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
    private MarioPhysiscsTest Mar; // **$$$
    Texture2D Hollow; // **$$$
    Texture2D platformTexture; // **$$$
    Rectangle platformRect; // **$$$
    private List<Rectangle> _solidRects; //Added
    private Vector2 _spawnPoint; //Added
    private BackgroundManager _backgroundManager;

    //ANIKA POWERUP COLLISION VARIABLES
    public Texture2D powerupTexture;
    public ISprite mushroom;

    //ANIKA ENEMIES COLLISION VARIABLES
    private List<object> _enemies = new List<object>();

    private SpriteFont myFont;
    private string coins = "00";

    private Song backgroundMusic;
    private double time = 360;

    private Texture2D coin;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = ViewWidth * scale; // 256 pixels
        _graphics.PreferredBackBufferHeight = 240 * scale;      // typical NES height
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _backgroundManager = new BackgroundManager(GraphicsDevice, Content);
        _backgroundManager.LoadContent();

        goombaSprite = Content.Load<Texture2D>("Sprites/goomba-Final");
        koopaSprite = Content.Load<Texture2D>("Sprites/green-koopa");
        goom = new moveGoom(goombaSprite, _spriteBatch);
        koop = new moveKoop(koopaSprite, _spriteBatch);

        // Set initial positions for enemies (in world coordinates, same scale as tiles)
        (goom as moveGoom).Position = new Vector2(16 * 20, 16 * 12); // 20 tiles over, ground level
        (koop as moveKoop).Position = new Vector2(16 * 25, 16 * 12); // 35 tiles over, ground level

        //load powerups
        powerupTexture = Content.Load<Texture2D>("Sprites/ItemSprite/powerups");
        //powerupTexture = Texture2D.FromFile(GraphicsDevice, "powerups.png");
        mushroom = new movePower(powerupTexture, _spriteBatch);
        (mushroom as movePower).Position = new Vector2(16 * 10, 16 * 11);

        
        _smallMario = new SmallMarioSprite(GraphicsDevice, Content); 
        _bigMario = new BigMarioSprite(GraphicsDevice, Content);
        
        // Start Mario somewhere reasonable in world coordinates (e.g., ground level)
        var pos = new Vector2(16 * 5, 16 * 13); // y = 13 tiles down instead of 20

        _smallMario.Position = pos;
        _bigMario.Position = pos;

        _spawnPoint = pos; //JAdded

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

        /**** JAdded  ****/
        // After loading your map tiles:
        _solidRects = new List<Rectangle>(_mapTiles.Count);
        foreach (var tile in _mapTiles)
            _solidRects.Add(tile.Bounds); 
        /****  JEnd Added  ****/

        //loads the camera on mario
        camera = new CameraManager(GraphicsDevice.Viewport);
        camera.Reset(_currentMario.Position); // start centered on Mario
        camera.LookAt(_currentMario.Position); // immediately focus on him

        //physics test $$$
        Hollow = Content.Load<Texture2D>("Sprites/Entity/marioStatic");
        //Hollow = Texture2D.FromFile(GraphicsDevice, "mario-static.png"); // **$$$
        Mar = new MarioPhysiscsTest(Hollow); // **$$$
        platformTexture = new Texture2D(GraphicsDevice, 1, 1); // **$$$
        platformRect = new Rectangle(0, 209, 5000, 50); // **$$$

        myFont = Content.Load<SpriteFont>("marioFont");

        backgroundMusic = Content.Load<Song>("01-main-theme-overworld");
        MediaPlayer.IsRepeating = true;   // loop the song
        MediaPlayer.Volume = 0.5f;        // volume (0.0 - 1.0)
        MediaPlayer.Play(backgroundMusic);
        coin = Content.Load<Texture2D>("Sprites/ItemSprite/coin2");
        //coin = Texture2D.FromFile(GraphicsDevice, "coin2.png");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();



        KeyboardState state = Keyboard.GetState();
        previousState = state; 

        bool bDown = state.IsKeyDown(Keys.B);
		if (bDown && !_bHeldLast)
			ToggleMarioSize();
		_bHeldLast = bDown;

        if (state.IsKeyDown(Keys.R))
        {
            // Reset Mario’s position to the original spawn point
            if (_currentMario is SmallMarioSprite)
                _currentMario.Position = _spawnPoint;
            else if (_currentMario is BigMarioSprite)
                _currentMario.Position = _spawnPoint;

            // Also reset both sprite forms so future switches start from same place
            _smallMario.Position = _spawnPoint;
            _bigMario.Position = _spawnPoint;

            // Reset the camera to follow Mario at that point
            camera.Reset(_spawnPoint);
            camera.LookAt(_spawnPoint);
        }

        _currentMario.Update(gameTime);
        Vector2 marioPos = _currentMario is SmallMarioSprite sm ? sm.Position :
                   _currentMario is BigMarioSprite bm ? bm.Position :
                   Vector2.Zero;
        camera.LookAt(marioPos);

        if (StaticCollisionHandler.HandleMany(_currentMario, _mapTiles, out var res, out var hitTile))
        {
            Console.WriteLine($"Mario hit {hitTile.TileName} (gid={hitTile.Gid}) at {hitTile.Position} | Side={res.Side} | MTV={res.MTV}");
        }

       
        Mar.Update(gameTime, state, platformRect); // ***$$$

        goom.Update(gameTime);
        koop.Update(gameTime);

        //update mushroom
        mushroom.Update(gameTime);

        if (goom is moveGoom g)
        {
            if (EnemyCollisionHandler.HandleMany(g, _mapTiles, out var gRes, out var gTile))
            {
                if (gRes.HitWall)
                {
                    Console.WriteLine($"Goomba hit wall at {gRes.TileRect.Location}");
                    g.ReverseDirection();
                }
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
                if (kRes.HitWall)
                {
                    Console.WriteLine($"Koopa hit wall at {kRes.TileRect.Location}");
                    k.ReverseDirection();
                }
                if (kRes.Grounded) Console.WriteLine("Koopa grounded");
                if (kRes.BonkedHead) Console.WriteLine("Koopa bonked head");
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
                    camera.Reset(_spawnPoint);
                    camera.LookAt(_spawnPoint);
                });

        }
        time -= 0.02;

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
        GraphicsDevice.Clear(new Color(92, 148, 252));

        // TODO: Add your drawing code here
        _spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());

        _backgroundManager.Draw(_spriteBatch, cameraX: 0f); // or your camera’s X

        _currentMario.Draw(_spriteBatch, _currentMario.Position);

        // For each tile, draw it
        foreach (var tile in _mapTiles)
        {
            tile.Draw(_spriteBatch);
        }
        //_spriteBatch.DrawRectangle(new Rectangle(0, 0, 256, 240), Color.Red);
        Mar.Draw(_spriteBatch); // ***$$$ maybe not mario
        //draw enemies (koop and goom)
        if (goom is moveGoom g && g.IsAlive)
        {
            g.Draw(_spriteBatch, g.Position);
        }

        if(koop is moveKoop k && k.IsAlive)
        {
            k.Draw(_spriteBatch, k.Position);
        }

        //draw mushroom
        if (mushroom != null && (mushroom as movePower).IsAlive)
        {
            mushroom.Draw(_spriteBatch, (mushroom as movePower).Position);
        }

        _spriteBatch.End();

        _spriteBatch.Begin();

        _spriteBatch.DrawString(myFont, "MARIO", new Vector2(90, 15), Color.White);
        _spriteBatch.DrawString(myFont, "000000", new Vector2(90, 55), Color.White);
        _spriteBatch.DrawString(myFont, "x" + coins, new Vector2(375, 55), Color.White);
        _spriteBatch.DrawString(myFont, "WORLD", new Vector2(550, 15), Color.White);
        _spriteBatch.DrawString(myFont, "1-1", new Vector2(580, 55), Color.White);
        _spriteBatch.DrawString(myFont, "TIME", new Vector2(800, 15), Color.White);
        _spriteBatch.DrawString(myFont, ((int)time).ToString(), new Vector2(825, 55), Color.White);
        _spriteBatch.Draw(
            coin,                  // Texture2D
            new Vector2(330, 45),  // Position (top-left)
            null,                  // Source rectangle (null = full texture)
            Color.White,           // Tint
            0f,                    // Rotation (none)
            Vector2.Zero,          // Origin (top-left corner)
            3f,                    // Scale (3x larger)
            SpriteEffects.None,    // No flipping
            0f                     // Layer depth
        );

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
