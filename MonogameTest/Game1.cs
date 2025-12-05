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
using MonogameTest.Managers;

namespace MonogameTest;

public class Game1 : Game
{
    private GameConfig C => ConfigLoader.Config;

    // Graphics + Sprites
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private InputController _input = new InputController();/////

    // Mario
    private SmallMarioSprite _smallMario;
    private BigMarioSprite _bigMario;
	private StaticSprite _currentMario;
	private bool _isBig = false;
    private bool _bHeldLast = false;
    private bool _pHeldLast = false;
    // Level
    private Texture2D _tileset;
    private List<Tile> _mapTiles;
    private int TilesVisibleX => C.TilesVisibleX;
    private int TileSize => C.TileSize;
    KeyboardState previousState;
    private int ViewWidth => C.ViewWidth;

    // Camera 
    private ICamera camera;
    private int Scale => C.Scale;
    private float SpriteScale => C.SpriteScale;

    // Objects
    private List<Rectangle> _solidRects; 
    private Vector2 _spawnPoint;
    private BackgroundManager _backgroundManager;

    // UI Elements
    private SpriteFont myFont;
    public string score = "000000";
    private string coins = "00";
    private double time;
    private Texture2D coin;

    //THE EVER PROMISED STATE MACHINE 
    Animation idleAnim;
    Animation runAnim;
    Animation jumpAnim;
    AnimationPlayer animPlayer;
    Physics Phys;
    PlayerMario Mar; 

    public SoundManager SoundManager { get; private set; }

    // Screen + HUD
    private ScreenManager _screenManager;
    private HUDScreen _hud;

    private int ScaleMod => C.ScaleMod;

    private TitleScreen _titleScreen;
    private LevelIntroScreen _introScreen;
    private TimeUpScreen _timeUpScreen;
    private GameOverScreen _gameOverScreen;
    // Powerups + Flagpole
    Texture2D powerupsSheet;
    private PowerupFieldManager _powerupFieldManager;

    private Flagpole _flagpole;
    private Texture2D _flagTexture;
    private Rectangle _poleRect;

    private int cooldown;
    private HashSet<Tile> _usedQuestionBlocks = new HashSet<Tile>();
    private EnemyManager _enemyManager;

    private CollisionManager _collisionManager;

    private bool _isPaused = false;
    private bool _isMuted = false;

    public Game1()
    {
        ConfigLoader.Load();  // Load JSON before anything needs the values
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = ViewWidth * Scale; // 256 pixels
        _graphics.PreferredBackBufferHeight = ScaleMod * Scale;      // typical NES height
        _graphics.ApplyChanges();
        _screenManager = new ScreenManager();
        _enemyManager = new EnemyManager(TileSize);
        _powerupFieldManager = new PowerupFieldManager();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        cooldown = C.Cooldown;
        time = C.StartingTime;
        _spawnPoint = new Vector2(C.MarioStartX, C.MarioStartY);

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        SoundManager = SoundManager.Instance;

        SoundLoader.LoadAllSounds(this, SoundManager);
        SoundManager.PlaySong("mainTheme"); // auto start overworld theme

        _backgroundManager = new BackgroundManager(GraphicsDevice, Content);
        _backgroundManager.LoadContent();

        Assets.Load(Content,GraphicsDevice);
        idleAnim = new Animation(Assets.PlayerIdle, 30, 16, 1, 0.1f, 8);
        runAnim  = new Animation(Assets.PlayerRun,  30, 16, 3, 0.1f, 9);
        animPlayer = new AnimationPlayer();
        //jumpAnim = new Animation(Assets.PlayerJump, 16, 16, 2, 0.15f, 0);
        animPlayer.Play(idleAnim);
        Mar = new PlayerMario();
        Mar.LoadContent(Content, SoundManager, GraphicsDevice);

        _enemyManager.LoadContent(Content, GraphicsDevice, _spriteBatch);

        powerupsSheet = Content.Load<Texture2D>("Sprites/powerups");
        _powerupFieldManager.LoadContent(Content, GraphicsDevice, _spriteBatch);
        _powerupFieldManager.SetPowerupSheet(powerupsSheet);

        // Uncomment this to see One-up functionality
        //powerups.Add(PowerupFactory.Create(PowerupType.GreenMushroom, powerupsSheet, new Vector2(16*12, 16*11)));
        
        _smallMario = new SmallMarioSprite(GraphicsDevice); //Added
        _bigMario = new BigMarioSprite(GraphicsDevice);

        _smallMario.SoundManager = SoundManager; // if SmallMario also needs sounds
        _bigMario.SoundManager = SoundManager;   // use the game's SoundManager
        
        // Start Mario somewhere reasonable in world coordinates (e.g., ground level)
        var pos = new Vector2(TileSize * 5, TileSize * 13);

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

        // After loading your map tiles:
        _solidRects = new List<Rectangle>(_mapTiles.Count);
        foreach (var tile in _mapTiles)
            _solidRects.Add(tile.Bounds); 

        //loads the camera on mario
        camera = new CameraManager(GraphicsDevice.Viewport);
        camera.Reset(_currentMario.Position); // start centered on Mario
        camera.LookAt(_currentMario.Position); // immediately focus on him

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

        _collisionManager = new CollisionManager(
            _smallMario,
            _bigMario,
            _currentMario,
            _mapTiles,
            _enemyManager,
            _powerupFieldManager,
            _screenManager,
            SoundManager,
            _flagpole,
            _spawnPoint,
            TileSize,
            () => ToggleMarioSize(), // <-- pass the callback
            (int points) => { score = (int.Parse(score) + points).ToString("000000"); }, // score
            () => { coins = (int.Parse(coins) + 1).ToString("00"); }, // coin
            () => { score = "000000"; coins = "00"; time = 400;}
        );

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

        KeyboardState state = Keyboard.GetState();
        animPlayer.Update(gameTime);
        previousState = state; 

        _input.Update();

        // ----------------------------------
        // === TOGGLE PAUSE (P key) ===
        // ----------------------------------
        bool pDown = state.IsKeyDown(Keys.P);
        if (pDown && !_pHeldLast)
        {
            _isPaused = !_isPaused;

            SoundManager.Instance.PlayEffect("pause");

            if (_isPaused)
                SoundManager.Instance.PauseSong();
            else
                SoundManager.Instance.ResumeSong();
        }
        _pHeldLast = pDown;

        // === FREEZE GAMEPLAY WHEN PAUSED ===
        if (_isPaused)
        {
            previousState = state;
            return; // stop all gameplay logic
        }

        bool bDown = state.IsKeyDown(Keys.B);
		if (bDown && !_bHeldLast)
			ToggleMarioSize();
		_bHeldLast = bDown;

        if (state.IsKeyDown(Keys.R))
        {
            ResetLevel();
        }

        _currentMario.Update(gameTime);
        _collisionManager.Update(
            gameTime,
            _currentMario,
            _isBig,
            (CameraManager)camera
        );
        Vector2 marioPos = _currentMario is SmallMarioSprite sm ? sm.Position :
                   _currentMario is BigMarioSprite bm ? bm.Position :
                   Vector2.Zero;
        camera.LookAt(marioPos);

        _enemyManager.Update(gameTime, _mapTiles);

        if (Keyboard.GetState().IsKeyDown(Keys.M))
        {
            _isMuted = !_isMuted;
            SoundManager.Instance.ToggleMute();
        }
        previousState = state;
        var enemies = _enemyManager.GetLiveEnemies();
        time -= 0.02;
        base.Update(gameTime);
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

    private void ResetScoreAndCoins()
    {
        score = "000000";
        coins = "0";
        time = 400;
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
        pos.Y = TileSize * 13;

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
        _enemyManager.Draw(_spriteBatch);

        //powerups
        _powerupFieldManager.Draw(_spriteBatch);

        _flagpole.Draw(); 

        _spriteBatch.End();

        // HUD, sort of has to be like this since many things are incremented in game1
        _spriteBatch.Begin();
        _spriteBatch.DrawString(myFont, "MARIO", new Vector2(90, 15), Color.White);
        _spriteBatch.DrawString(myFont, score, new Vector2(90, 55), Color.White);
        _spriteBatch.DrawString(myFont, "x" + coins, new Vector2(375, 55), Color.White);
        _spriteBatch.DrawString(myFont, "WORLD", new Vector2(550, 15), Color.White);
        _spriteBatch.DrawString(myFont, "1-1", new Vector2(580, 55), Color.White);
        _spriteBatch.DrawString(myFont, "TIME", new Vector2(800, 15), Color.White);
        _spriteBatch.DrawString(myFont, ((int)time).ToString(), new Vector2(825, 55), Color.White);
        _spriteBatch.Draw(
            coin,                 
            new Vector2(330, 45),  
            null,                 
            Color.White,           
            0f,        
            Vector2.Zero,          
            3f,                    
            SpriteEffects.None,    
            0f                     
        );
        // === PAUSE OVERLAY ===
        if (_isPaused)
        {
            _spriteBatch.DrawString(
                myFont,
                "PAUSE",
                new Vector2(450, 300),
                Color.White
            );
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
