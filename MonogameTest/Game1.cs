using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using System.Net;
using System.Runtime.Intrinsics.X86;
using MonogameTest.Sounds;
using MonogameTest.Screens;
using System.Net.Mime;

namespace MonogameTest;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private InputController _input = new InputController();/////
    //public CommandManager CommandManager { get; set; }
    public Texture2D goombaSprite;
    public Texture2D koopaSprite;
    public ISprite goom;
    public ISprite koop;
    private List<moveGoom> goombas = new List<moveGoom>();
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
    const float SpriteScale = 0.30f;
    Texture2D Hollow; // **$$$
    Texture2D platformTexture; // **$$$
    Rectangle platformRect; // **$$$
    private List<Rectangle> _solidRects; //Added
    private Vector2 _spawnPoint; //Added
    private BackgroundManager _backgroundManager;

    public Texture2D powerupTexture;
    public ISprite mushroom;
    private List<object> _enemies = new List<object>();

    private SpriteFont myFont;
    public string score = "000000";
    private string coins = "00";
    private double time = 360;
    private Texture2D coin;

    //THE EVER PROMISED STATE MACHINE 
    //... Testy
    Animation idleAnim;
    Animation runAnim;
    Animation jumpAnim;
    AnimationPlayer animPlayer;
    Physics Phys;
    PlayerMario Mar; ///////

    public SoundManager SoundManager { get; private set; }

    // Screen + HUD
    private ScreenManager _screenManager;
    private HUDScreen _hud;

    private TitleScreen _titleScreen;
    private LevelIntroScreen _introScreen;
    private TimeUpScreen _timeUpScreen;
    private GameOverScreen _gameOverScreen;
    //powerups
    Texture2D powerupsSheet;
    List<PowerupInstance> powerups = new List<PowerupInstance>();
    private Flagpole _flagpole;
    private Texture2D _flagTexture;
    private Rectangle _poleRect;

    private int cooldown = 80;

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
        _screenManager = new ScreenManager();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        SoundManager = SoundManager.Instance;

        SoundLoader.LoadAllSounds(this, SoundManager);
        SoundManager.PlaySong("mainTheme"); // auto start overworld theme

        _backgroundManager = new BackgroundManager(GraphicsDevice, Content);
        _backgroundManager.LoadContent();

        /////
        Assets.Load(Content,GraphicsDevice);
        idleAnim = new Animation(Assets.PlayerIdle, 30, 16, 1, 0.1f, 8);
        runAnim  = new Animation(Assets.PlayerRun,  30, 16, 3, 0.1f, 9);
        animPlayer = new AnimationPlayer();
        //jumpAnim = new Animation(Assets.PlayerJump, 16, 16, 2, 0.15f, 0);
        animPlayer.Play(idleAnim);
        Mar = new PlayerMario();
        Mar.LoadContent(Content, SoundManager, GraphicsDevice);

        goombaSprite = Content.Load<Texture2D>("Sprites/goomba-Final");
        koopaSprite = Content.Load<Texture2D>("Sprites/green-koopa");
        goom = new moveGoom(goombaSprite, _spriteBatch);
        koop = new moveKoop(koopaSprite, _spriteBatch);

        // Set initial positions for enemies (in world coordinates, same scale as tiles)
        Vector2[] goombaPositions = new Vector2[]
        {
            new Vector2(16 * 22, 16 * 12),
            new Vector2(16 * 39, 16 * 12),
            new Vector2(16 * 50, 16 * 12),
            new Vector2(16 * 52, 16 * 12),
            new Vector2(16 * 79, 16 * 4),
            new Vector2(16 * 81, 16 * 4),
            new Vector2(16 * 96, 16 * 12),
            new Vector2(16 * 98, 16 * 12),
            new Vector2(16 * 113, 16 * 12),
            new Vector2(16 * 115, 16 * 12),
            new Vector2(16 * 124, 16 * 12),
            new Vector2(16 * 126, 16 * 12),
            new Vector2(16 * 128, 16 * 12),
            new Vector2(16 * 130, 16 * 12),
            new Vector2(16 * 173, 16 * 12),
            new Vector2(16 * 175, 16 * 12)
        };
        // Load goombas
        foreach (var posG in goombaPositions)
        {
            var g = new moveGoom(goombaSprite, _spriteBatch);
            g.Position = posG;
            goombas.Add(g);
        }

        (koop as moveKoop).Position = new Vector2(16 * 106, 16 * 12); // 106 tiles over, ground level

        powerupsSheet = Content.Load<Texture2D>("Sprites/powerups");

        powerups.Add(PowerupFactory.Create(PowerupType.Mushroom, powerupsSheet, new Vector2(16*8, 16*11)));
        powerups.Add(PowerupFactory.Create(PowerupType.Coin, powerupsSheet, new Vector2(16*9, 16*11)));
        powerups.Add(PowerupFactory.Create(PowerupType.Star, powerupsSheet, new Vector2(16*10, 16*11)));
        powerups.Add(PowerupFactory.Create(PowerupType.GreenMushroom, powerupsSheet, new Vector2(16*12, 16*11)));
        
        _smallMario = new SmallMarioSprite(GraphicsDevice); //Added
        _bigMario = new BigMarioSprite(GraphicsDevice);

        _smallMario.SoundManager = SoundManager; // if SmallMario also needs sounds
        _bigMario.SoundManager = SoundManager;   // use the game's SoundManager
        
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
        Hollow = Texture2D.FromFile(GraphicsDevice, "mario-static.png"); // **$$$
        //Mar = new MarioPhysiscsTest(Hollow); // **$$$
        platformTexture = new Texture2D(GraphicsDevice, 1, 1); // **$$$
        //platformRect = new Rectangle(0, 209, 5000, 50); // **$$$

        myFont = Content.Load<SpriteFont>("marioFont");
        coin = Texture2D.FromFile(GraphicsDevice, "coin2.png");
        var titleTex = Texture2D.FromFile(GraphicsDevice, "titlescreen.png");

        _hud = new HUDScreen(myFont, coin, _screenManager);

        // Load screens
        _titleScreen = new TitleScreen(this, _screenManager, titleTex, myFont, coin);
        _introScreen = new LevelIntroScreen(this, _screenManager, myFont, coin);
        _timeUpScreen = new TimeUpScreen(this, _screenManager, myFont, coin);
        _gameOverScreen = new GameOverScreen(this, _screenManager, myFont, coin);

        _flagTexture = Texture2D.FromFile(GraphicsDevice, "flag.png");
        _poleRect = new Rectangle(TileSize * 198, TileSize * 3, 5, 160); // guessing numbers for testing
        _flagpole = new Flagpole(_spriteBatch, _flagTexture, _poleRect, _currentMario);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

            _screenManager.Update(gameTime);

        // Screen state handling
        switch (_screenManager.CurrentState)
        {
            case GameState.Title:
                _titleScreen.Update(gameTime);
                return;

            case GameState.LevelIntro:
                _introScreen.Update(gameTime);
                return;

            case GameState.TimeUp:
                _timeUpScreen.Update(gameTime);
                return;

            case GameState.GameOver:
                _gameOverScreen.Update(gameTime);
                return;

            case GameState.Playing:
                break;
        }

        //if (MarioManager.ActiveSprite != null) MarioManager.ActiveSprite.Update(gameTime);

        KeyboardState state = Keyboard.GetState();
        animPlayer.Update(gameTime);////////
        previousState = state; 

        _input.Update();/////

        bool bDown = state.IsKeyDown(Keys.B);
		if (bDown && !_bHeldLast)
			ToggleMarioSize();
		_bHeldLast = bDown;

        if (state.IsKeyDown(Keys.R))
        {
            _currentMario.Position = _spawnPoint;
            _smallMario.Position = _spawnPoint;
            _bigMario.Position = _spawnPoint;
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

        //Mar.Update(gameTime, state, platformRect); 
        foreach (var gg in goombas)
            if (gg.IsAlive)
                gg.Update(gameTime);
            
        koop.Update(gameTime);

        if (Keyboard.GetState().IsKeyDown(Keys.M))
        {
            SoundManager.Instance.ToggleMute();
        }

        foreach (var g in goombas)
        {
            if (EnemyCollisionHandler.HandleMany(g, _mapTiles, out var gRes, out var gTile))
            {
                if (gRes.HitWall)
                {
                    Console.WriteLine($"Goomba hit wall at {gRes.TileRect.Location}");
                    g.ReverseDirection();
                }
                if (gRes.Grounded) Console.WriteLine("Goomba grounded");
                if (gRes.BonkedHead) Console.WriteLine("Goomba bonked head");

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
        foreach (var g in goombas)
            if (g.IsAlive)
                enemies.Add(g);
        
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
                    _screenManager.Lives--;
                    if (_screenManager.Lives <= 0)
                        _screenManager.ChangeState(GameState.GameOver);
                    else
                    {
                        _screenManager.ChangeState(GameState.LevelIntro);
                    }
                });

        }
        time -= 0.02;
        //powerups
        foreach (var p in powerups)
        {
         p.Update(gameTime);

        if (PowerupCollisionHandler.CheckMarioPowerupCollision(_currentMario, p))
        {
        HandlePowerupPickup(p);
        }
    }
        _flagpole.Update(gameTime);
        base.Update(gameTime);
    }

    //powerups
    private void HandlePowerupPickup(PowerupInstance p)
    {
        p.IsAlive = false;

        switch (p.Type)
        {
            case PowerupType.Mushroom:
                if(_isBig == false)
                    ToggleMarioSize();
                score = (int.Parse(score) + 200).ToString("000000");
                break;

            case PowerupType.FireFlower:
                // give fire ability
                break;

            case PowerupType.Star:
                //invincibility
                break;

            case PowerupType.Coin:
                SoundManager.Instance.PlayEffect("coin");
                coins = (int.Parse(coins) + 1).ToString("00");
                score = (int.Parse(score) + 100).ToString("000000");
                break;
            case PowerupType.GreenMushroom:
                SoundManager.Instance.PlayEffect("oneUp");
                score = (int.Parse(score) + 200).ToString("000000");
                _screenManager.Lives += 1;
                break;
        }
    }

    private void ResetLevel()
    {
        _currentMario.Position = _spawnPoint;
        _smallMario.Position = _spawnPoint;
        _bigMario.Position = _spawnPoint;
        camera.Reset(_spawnPoint);
        camera.LookAt(_spawnPoint);
        _screenManager.ResetLevel();
    }
    
    private void ToggleMarioSize()
	{
        if (_currentMario == _smallMario)
        {
            SoundManager.Instance.PlayEffect("powerUp");
        } 
        else { 
            SoundManager.Instance.PlayEffect("warning");
            }
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
        _spriteBatch.Begin();
        switch (_screenManager.CurrentState)
            {
                case GameState.Title:
                    _titleScreen.Draw(_spriteBatch);
                    _spriteBatch.End();
                    return;

                case GameState.LevelIntro:
                    _introScreen.Draw(_spriteBatch);
                    _spriteBatch.End();
                    return;

                case GameState.TimeUp:
                    _timeUpScreen.Draw(_spriteBatch);
                    _spriteBatch.End();
                    return;

                case GameState.GameOver:
                    _gameOverScreen.Draw(_spriteBatch);
                    _spriteBatch.End();
                    _screenManager.Lives = 3;
                    //Exit();
                    return;

                case GameState.Playing:
                    break;
            }

        _spriteBatch.End();

        _spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
        _backgroundManager.Draw(_spriteBatch, cameraX: 0f); // or your camera’s X

        _currentMario.Draw(_spriteBatch, _currentMario.Position);

        // For each tile, draw it
        foreach (var tile in _mapTiles)
        {
            tile.Draw(_spriteBatch);
        }
        Mar.Draw(_spriteBatch); // ***$$$ maybe not mario
        //draw enemies (koop and goom)
        foreach (var g in goombas)
            if (g.IsAlive)
                g.Draw(_spriteBatch, g.Position);

        if(koop is moveKoop k && k.IsAlive)
        {
            k.Draw(_spriteBatch, k.Position);
        }

        //powerups
        foreach (var p in powerups)
            p.Draw(_spriteBatch);

        _flagpole.Draw(); 

        _spriteBatch.End();

        // HUD
        _spriteBatch.Begin();
        _spriteBatch.DrawString(myFont, "MARIO", new Vector2(90, 15), Color.White);
        _spriteBatch.DrawString(myFont, score, new Vector2(90, 55), Color.White);
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
