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

namespace MonogameTest;

public class Game1 : Game
{
    // Graphics + Sprites
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private InputController _input = new InputController();/////
    public Texture2D goombaSprite;
    public Texture2D koopaSprite;
    public ISprite goom;
    public ISprite koop;
    private List<moveGoom> goombas = new List<moveGoom>();

    // Mario
    private SmallMarioSprite _smallMario;
    private BigMarioSprite _bigMario;
	private StaticSprite _currentMario;
	private bool _isBig = false;
    private bool _bHeldLast = false;

    // Level
    private Texture2D _tileset;
    private List<Tile> _mapTiles;
    const int TilesVisibleX = 16;
    const int TileSize = 16;
    KeyboardState previousState;
    const int ViewWidth = TilesVisibleX * TileSize;

    // Camera 
    private ICamera camera;
    const int scale = 4;
    const float SpriteScale = 0.30f;

    // Objects
    private List<Rectangle> _solidRects; 
    private Vector2 _spawnPoint;
    private BackgroundManager _backgroundManager;
    private List<object> _enemies = new List<object>();

    // UI Elements
    private SpriteFont myFont;
    public string score = "000000";
    private string coins = "00";
    private double time = 360;
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

    private TitleScreen _titleScreen;
    private LevelIntroScreen _introScreen;
    private TimeUpScreen _timeUpScreen;
    private GameOverScreen _gameOverScreen;
    // Powerups + Flagpole
    Texture2D powerupsSheet;
    List<PowerupInstance> powerups = new List<PowerupInstance>();
    private Flagpole _flagpole;
    private Texture2D _flagTexture;
    private Rectangle _poleRect;

    private int cooldown = 80;
    private HashSet<Tile> _usedQuestionBlocks = new HashSet<Tile>();
     //MAGIC:
    private static readonly GameNumbers GameNumbers = NumberLoad.Numbers.GameNum;
    private static readonly CameraMan UINumbers = NumberLoad.Numbers.CameraMan;
    private static readonly PlayerAnimation PlayerAnimation = NumberLoad.Numbers.PlayerAnimations;

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
        koop = new moveKoop(koopaSprite, _spriteBatch);

        // Load goombas
        foreach (var posTile in EnemyPositions.Goombas)
        {
            var g = new moveGoom(goombaSprite, _spriteBatch);
            g.Position = posTile * TileSize;
            goombas.Add(g);
        }

        (koop as moveKoop).Position = new Vector2(16 * 106, 16 * 12); // 106 tiles over, ground level

        powerupsSheet = Content.Load<Texture2D>("Sprites/powerups");

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

        KeyboardState state = Keyboard.GetState();
        animPlayer.Update(gameTime);
        previousState = state; 

        _input.Update();

        bool bDown = state.IsKeyDown(Keys.B);
		if (bDown && !_bHeldLast)
			ToggleMarioSize();
		_bHeldLast = bDown;

        if (state.IsKeyDown(Keys.R))
        {
            ResetLevel();
        }

        _currentMario.Update(gameTime);
        Vector2 marioPos = _currentMario is SmallMarioSprite sm ? sm.Position :
                   _currentMario is BigMarioSprite bm ? bm.Position :
                   Vector2.Zero;
        camera.LookAt(marioPos);

        if (StaticCollisionHandler.HandleMany(_currentMario, _mapTiles, out var res, out var hitTile))
        {
            if (hitTile.TileName == "Question" &&
                res.Side == typeCollision.Bottom &&
                !_usedQuestionBlocks.Contains(hitTile))
            {
                SoundManager.Instance.PlayEffect("bump");
                _usedQuestionBlocks.Add(hitTile);

                // Spawn a coin just above the block
                Vector2 spawnPos = hitTile.Position;
                spawnPos.Y -= hitTile.Bounds.Height;

                if (marioPos.X < TileSize * 22 && marioPos.X > 16 * 19)
                {
                    powerups.Add(
                    PowerupFactory.Create(
                        PowerupType.Mushroom,
                        powerupsSheet,
                        spawnPos
                    )
                );
                SoundManager.Instance.PlayEffect("powerUpAppears");
                } else
                {
                   powerups.Add(
                    PowerupFactory.Create(
                        PowerupType.Coin,
                        powerupsSheet,
                        spawnPos
                    )
                ); 
                SoundManager.Instance.PlayEffect("coin");
                coins = (int.Parse(coins) + 1).ToString("00");
                score = (int.Parse(score) + 100).ToString("000000");
                }

            }
        }

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
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
