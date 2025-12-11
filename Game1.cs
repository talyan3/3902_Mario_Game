using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameTest.Managers;
using MonogameTest.Screens;
using MonogameTest.Sounds;
using System.IO.Pipes;

namespace MonogameTest
{
    public class Game1 : Game
    {
        // ===========================
        // CORE ENGINE OBJECTS
        // ===========================
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private KeyboardController _input;
        private ScreenManager _screenManager;
        private PauseScreen _pauseScreen;
        private SoundManager _sound;

        // ===========================
        // WORLD / LEVEL
        // ===========================
        private List<Tile> _mapTiles;
        private Vector2 _spawnPoint;
        private ICamera _camera;
        private BackgroundManager _backgroundManager;

        // ===========================
        // MARIO
        // ===========================
        private SmallMarioSprite _smallMario;
        private BigMarioSprite _bigMario;
        private MarioStateController _marioState;
        private FireMarioSprite _fireMario;

        // ===========================
        // GAMEPLAY SYSTEMS
        // ===========================
        private EnemyManager _enemyManager;
        private PowerupFieldManager _powerupManager;
        private CollisionManager _collisionManager;
        private FireballManager _fireballs;

        // ===========================
        // UI / SCREENS
        // ===========================
        private SpriteFont _font;
        private Texture2D _coin;
        private HUDScreen _hud;
        private Texture2D _fireballTexture;
        private TitleScreen _titleScreen;
        private LevelIntroScreen _introScreen;
        private TimeUpScreen _timeUpScreen;
        private GameOverScreen _gameOverScreen;
        private Flagpole _flagpole;
        private float _deathTimer = 0f;

        // Flags
        public static bool InputLocked { get; set; }
        public static bool DebugGodMode { get; set; }


        // Christmas Mode
        public static bool ChristmasMode { get; set; }
        private bool _xWasDown;
        private bool _showChristmasText = false;
        private float _christmasTimer = 0f;

        // View Info
        public static int ScreenWidth { get; private set; }
        public static int ScreenHeight { get; private set; }

        // ===========================
        // CONFIG SHORTCUTS
        // ===========================
        private GameConfig C => ConfigLoader.Config;
        //private int TileSize => C.TileSize;
        private int ViewWidth => C.ViewWidth;
        private int Scale => C.Scale;
        private int ScaleMod => C.ScaleMod;

        List<Pipe> pipes;
        Texture2D whiteRectangle;
        public static bool pipeMode = false;
        private DashComponent dash;
        public Game1()
        {
            ConfigLoader.Load();
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        // =========================================================
        // INITIALIZE
        // =========================================================
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = ViewWidth * Scale;
            _graphics.PreferredBackBufferHeight = ScaleMod * Scale;
            _graphics.ApplyChanges();

            ScreenWidth = _graphics.PreferredBackBufferWidth;
            ScreenHeight = _graphics.PreferredBackBufferHeight;

            _input = new KeyboardController();
            _screenManager = ScreenManager.Instance;

            _enemyManager = new EnemyManager(C.TileSize); // THIS IS SO GOOD, USE THIS!!!
            _powerupManager = new PowerupFieldManager();

            pipes = new List<Pipe>();
            pipes.Add(new Pipe(
                new Rectangle(752-16, 128, 32, 16), // pipe location/size
                new Vector2(_spawnPoint.X, _spawnPoint.Y)            // Destination
            ));
            dash = new DashComponent();

            base.Initialize();
        }

        // =========================================================
        // LOAD CONTENT
        // =========================================================
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // ---------- SOUND ----------
            _sound = SoundManager.Instance;
            SoundLoader.LoadAllSounds(this, _sound);
            if (!ChristmasMode)
                SoundManager.Instance.PlaySong("mainTheme");
            else
                SoundManager.Instance.PlaySong("mainXmas");

            // ---------- BACKGROUND ----------
            _backgroundManager = new BackgroundManager(GraphicsDevice, Content, ChristmasMode);
            _backgroundManager.LoadContent();

            // ---------- MARIO ----------
            _smallMario = new SmallMarioSprite(GraphicsDevice) { SoundManager = _sound };
            _bigMario = new BigMarioSprite(GraphicsDevice) { SoundManager = _sound };
            _fireMario = new FireMarioSprite(GraphicsDevice) {SoundManager = _sound};

            _spawnPoint = new Vector2(C.TileSize * 5, C.TileSize * 13);

            whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            _smallMario.Position = _spawnPoint;
            _bigMario.Position = _spawnPoint;
            _fireMario.Position = _spawnPoint;

            _marioState = new MarioStateController(_smallMario, _bigMario, _fireMario, _smallMario, C.TileSize);

            // -------- FIREBALL --------
            _fireballTexture = Texture2D.FromFile(GraphicsDevice, "fireball.png");
            _fireballs = new FireballManager();
            _fireballs.Load(_fireballTexture);

            // ---------- MAP ----------
            using var fs = new FileStream("blocksV10.png", FileMode.Open);
            var tileset = Texture2D.FromStream(GraphicsDevice, fs);
            if (pipeMode == false)
                _mapTiles = TiledMapLoader.Load(Path.Combine(Directory.GetCurrentDirectory(), "level1.json"), tileset);
            else 
                _mapTiles = TiledMapLoader.Load(Path.Combine(Directory.GetCurrentDirectory(), "customLevel.json"), tileset);
            _powerupManager.SetTiles(_mapTiles);

            // ---------- CAMERA ----------
            _camera = new CameraManager(GraphicsDevice.Viewport);
            _camera.Reset(_spawnPoint);

            // ---------- POWERUPS ----------
            var powerupsSheet = Content.Load<Texture2D>("Sprites/powerups");
            _powerupManager.LoadContent(Content, GraphicsDevice, _spriteBatch);
            _powerupManager.SetPowerupSheet(powerupsSheet);

            // ---------- ENEMIES ----------
            _enemyManager.LoadContent(Content, GraphicsDevice, _spriteBatch);

            // ---------- HUD / SCREENS ----------
            _font = Content.Load<SpriteFont>("marioFont");
            _coin = Texture2D.FromFile(GraphicsDevice, "coin2.png");

            _hud = new HUDScreen(_font, _coin, _screenManager);

            _titleScreen = new TitleScreen(this, _screenManager, Texture2D.FromFile(GraphicsDevice, "titlescreen.png"), _font, _coin);
            _introScreen = new LevelIntroScreen(this, _screenManager, _font, _coin);
            _timeUpScreen = new TimeUpScreen(this, _screenManager, _font, _coin);
            _gameOverScreen = new GameOverScreen(this, _screenManager, _font, _coin);

            _pauseScreen = new PauseScreen(this, _screenManager, _font);

            // ---------- FLAGPOLE ----------
            var flagTexture = Texture2D.FromFile(GraphicsDevice, "flag.png");
            var poleRect = new Rectangle(C.TileSize * 198, C.TileSize * 3, 5, 160);
            _flagpole = new Flagpole(
                _spriteBatch,
                flagTexture,
                poleRect,
                _smallMario,
                _bigMario,
                _marioState.CurrentMario,
                _marioState,
                _screenManager,
                _spawnPoint,
                _camera,
                _hud
            );

            // ---------- COLLISION ----------
            _collisionManager = new CollisionManager(
                _marioState,
                _mapTiles,
                _enemyManager,
                _powerupManager,
                _screenManager,
                _sound,
                _flagpole,
                _spawnPoint,
                C.TileSize,
                _smallMario,
                _bigMario,
                _camera
            );
        }

        // =========================================================
        // UPDATE
        // =========================================================
        protected override void Update(GameTime gameTime)
        {
            // =============== DEATH TIMER HANDLING ===============
            if (PendingDeathTimer > 0f)
            {
                PendingDeathTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Still waiting → skip gameplay
                if (PendingDeathTimer > 0f)
                    return;

                // Timer finished → now reset
                ResetManager.SoftReset(
                    _smallMario,
                    _bigMario,
                    _marioState,
                    _spawnPoint,
                    _camera,
                    _enemyManager
                );

                _screenManager.ChangeState(GameState.LevelIntro);
                Game1.InputLocked = false;
            }
            // Pause logic first
            _pauseScreen.Update(gameTime);
            if (_pauseScreen.IsPaused)
                return;

            var pad = GamePad.GetState(PlayerIndex.One);
            var kb = Keyboard.GetState();

            if (pad.Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape))
                Exit();
            
            bool xDown = kb.IsKeyDown(Keys.X);
            if (xDown && !_xWasDown && !ChristmasMode)
            {
                ChristmasMode = true;
                _showChristmasText = true;
                _sound.StopSong();
                _enemyManager.ActivateSnail(_marioState.CurrentMario.Position);
                SoundManager.Instance.PlayEffect("jingle");
                if (SoundManager.Instance.IsSongPlaying())
                {
                    SoundManager.Instance.StopSong();
                    SoundManager.Instance.PlaySong("mainXmas");
                }
                _christmasTimer = 8f;
            }
            _xWasDown = xDown;

            _screenManager.Update(gameTime);

            switch (_screenManager.CurrentState)
            {
                case GameState.Title: _titleScreen.Update(gameTime); return;
                case GameState.LevelIntro: _introScreen.Update(gameTime); return;
                case GameState.TimeUp: _timeUpScreen.Update(gameTime); return;
                case GameState.GameOver: _gameOverScreen.Update(gameTime); return;
            }
            //TODO:PIPE TELEPORTATION
            foreach (var entrance in pipes)
            {
                if (entrance.CheckCollision(_smallMario.Bounds))
                {
                    Console.WriteLine("Teleporting Mario!");
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        pipeMode = true;
                        HardReload();
                    }
                    break;
                }
            }
            if (_showChristmasText)
            {
                _christmasTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_christmasTimer <= 0)
                    _showChristmasText = false;
            }

            if (!InputLocked)
            {
               _input.Update(); 
            }

            DebugGodMode = kb.IsKeyDown(Keys.D);

            //_marioState.CurrentMario.Update(gameTime);
            _marioState.Update(gameTime);
            if(!InputLocked)
            {
                if (_marioState.CurrentMario == _smallMario)
                {
                    _smallMario.Update(gameTime, _camera.LeftEdge);
                }
                if (_marioState.CurrentMario == _bigMario)
                {
                    _bigMario.Update(gameTime, _camera.LeftEdge);
                }
                if (_marioState.CurrentMario == _fireMario)
                {
                    _fireMario.Update(gameTime, _camera.LeftEdge);
                } 
            }
            dash.Update(gameTime, _marioState.CurrentMario.Position);
            _marioState.CurrentMario.Position = dash.position;
            _marioState.Tick(gameTime);

            if (kb.IsKeyDown(Keys.Z))
            {
            if (_marioState.TryShoot())
                {
                bool facingRight = true; 

                 Vector2 fbStart = _marioState.CurrentMario.Position + new Vector2(0, -10);
                _fireballs.Shoot(fbStart, facingRight);
                }
            }
            _fireballs.Update(gameTime, _mapTiles, _enemyManager.GetLiveEnemies());

            _collisionManager.Update(gameTime, _marioState.CurrentMario, (CameraManager)_camera);

            _camera.LookAt(_marioState.CurrentMario.Position);
            _enemyManager.Update(gameTime, _mapTiles,_marioState.CurrentMario.Position);

            // Sound mute toggle
            if (kb.IsKeyDown(Keys.M))
                _sound.ToggleMute();
            
            //reset level
            if (kb.IsKeyDown(Keys.R))
                ResetManager.SoftReset(
                    _smallMario,
                    _bigMario,
                    _marioState,
                    _spawnPoint,
                    _camera,
                    _enemyManager
                );

            base.Update(gameTime);
        }

        // =========================================================
        // DRAW
        // =========================================================
        protected override void Draw(GameTime gameTime)
        {
            if (pipeMode == false)
                GraphicsDevice.Clear(new Color(92, 148, 252));
            else 
                GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            switch (_screenManager.CurrentState)
            {
                case GameState.Title: _titleScreen.Draw(_spriteBatch); _spriteBatch.End(); return;
                case GameState.LevelIntro: _introScreen.Draw(_spriteBatch); _spriteBatch.End(); return;
                case GameState.TimeUp: _timeUpScreen.Draw(_spriteBatch); _spriteBatch.End(); return;
                case GameState.GameOver: _gameOverScreen.Draw(_spriteBatch); _spriteBatch.End(); return;
            }
            _spriteBatch.End();

            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());

            if (pipeMode == false)
                _backgroundManager.Draw(_spriteBatch, 0f);

            foreach (var tile in _mapTiles)
                tile.Draw(_spriteBatch);

            _marioState.CurrentMario.Draw(_spriteBatch, _marioState.CurrentMario.Position);
            _flagpole.Draw();

            _enemyManager.Draw(_spriteBatch);
            _powerupManager.Draw(_spriteBatch);
            
            _fireballs.Draw(_spriteBatch);

            _spriteBatch.End();

            _spriteBatch.Begin();
            _hud.Draw(_spriteBatch);
            if (_showChristmasText)
            {
                _spriteBatch.DrawString(
                    _font,
                    "CHRISTMAS MODE",
                    new Vector2(320, ScreenHeight/2),
                    Color.Red
                );
            }
            _pauseScreen.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        // ===============================================
        // HARD RELOAD (FULL GAME RESET) — Option 1
        // ===============================================
        public void HardReload()
        {
            // Turn off Christmas mode, unlock controls
            ChristmasMode = false;
            InputLocked = false;

            // Stop all audio
            SoundManager.Instance.StopSong();

            // Clear UI & screen states
            _screenManager = ScreenManager.Instance;
            _pauseScreen = new PauseScreen(this, _screenManager, _font);

            // Recreate core systems
            _enemyManager = new EnemyManager(C.TileSize);
            _powerupManager = new PowerupFieldManager();

            // Reset Mario to small + start position
            _smallMario.Position = _spawnPoint;
            _bigMario.Position = _spawnPoint;
            _marioState.ForceSmall(_spawnPoint);

            // Reload EVERYTHING just like startup
            Content.Unload();
            LoadContent();

            // Reset screen flow to level intro
            _screenManager.ChangeState(GameState.LevelIntro);
        }

        public static float PendingDeathTimer = 0f;

        public static void StartDeathTimer()
        {
            PendingDeathTimer = 2f;
        }
    }
}
