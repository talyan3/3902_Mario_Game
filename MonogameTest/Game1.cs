using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameTest.Managers;
using MonogameTest.Screens;
using MonogameTest.Sounds;

namespace MonogameTest
{
    public class Game1 : Game
    {
        // Graphics / Core
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Input and Screen Flow
        private KeyboardController _input;
        private ScreenManager _screenManager;
        private PauseScreen _pauseScreen;

        // Sound
        private SoundManager _sound;

        // World Data
        private List<Tile> _mapTiles;
        private Vector2 _spawnPoint;
        private ICamera _camera;
        private BackgroundManager _backgroundManager;

        // Player
        private SmallMarioSprite _smallMario;
        private BigMarioSprite _bigMario;
        private MarioStateController _marioState;

        // Gameplay Systems
        private EnemyManager _enemyManager;
        private PowerupFieldManager _powerupManager;
        private CollisionManager _collisionManager;

        // UI / Screens
        private SpriteFont _font;
        private Texture2D _coin;
        private HUDScreen _hud;
        private TitleScreen _titleScreen;
        private LevelIntroScreen _introScreen;
        private TimeUpScreen _timeUpScreen;
        private GameOverScreen _gameOverScreen;
        private Flagpole _flagpole;
        private float _deathTimer = 0f;


        // Config and View Info
        private GameConfig Config => ConfigLoader.Config;
        public static int ScreenWidth { get; private set; }
        public static int ScreenHeight { get; private set; }

        // Flags
        public static bool InputLocked { get; set; }
        public static bool DebugGodMode { get; set; }

        // Christmas Mode
        public static bool ChristmasMode { get; set; }
        private bool _xWasDown;
        private bool _showChristmasText = false;
        private float _christmasTimer = 0f;



        public Game1()
        {
            ConfigLoader.Load();
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = Config.ViewWidth * Config.Scale;
            _graphics.PreferredBackBufferHeight = Config.ScaleMod * Config.Scale;
            _graphics.ApplyChanges();

            ScreenWidth = _graphics.PreferredBackBufferWidth;
            ScreenHeight = _graphics.PreferredBackBufferHeight;

            _input = new KeyboardController();
            _screenManager = new ScreenManager();

            _enemyManager = new EnemyManager(Config.TileSize);
            _powerupManager = new PowerupFieldManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Sound
            _sound = SoundManager.Instance;
            SoundLoader.LoadAllSounds(this, _sound);
            if (!ChristmasMode)
            {
                SoundManager.Instance.PlaySong("mainTheme");
            }
            else
            {
                SoundManager.Instance.PlaySong("mainXmas");
            }
            
            // Background
            _backgroundManager = new BackgroundManager(GraphicsDevice, Content);
            _backgroundManager.LoadContent();

            // Player setup
            _smallMario = new SmallMarioSprite(GraphicsDevice) { SoundManager = _sound };
            _bigMario = new BigMarioSprite(GraphicsDevice)   { SoundManager = _sound };

            _spawnPoint = new Vector2(Config.TileSize * 5, Config.TileSize * 13);
            _smallMario.Position = _spawnPoint;
            _bigMario.Position = _spawnPoint;

            _marioState = new MarioStateController(_smallMario, _bigMario, _smallMario, Config.TileSize);

            // Map
            using var fs = new FileStream("blocksV10.png", FileMode.Open);
            var tileset = Texture2D.FromStream(GraphicsDevice, fs);
            var levelPath = Path.Combine(Directory.GetCurrentDirectory(), "level1.json");
            _mapTiles = TiledMapLoader.Load(levelPath, tileset);

            // Camera
            _camera = new CameraManager(GraphicsDevice.Viewport);
            _camera.Reset(_spawnPoint);

            // Powerups
            var powerupSheet = Content.Load<Texture2D>("Sprites/powerups");
            _powerupManager.LoadContent(Content, GraphicsDevice, _spriteBatch);
            _powerupManager.SetPowerupSheet(powerupSheet);

            // Enemies
            _enemyManager.LoadContent(Content, GraphicsDevice, _spriteBatch);

            // Flagpole
            var flagTexture = Texture2D.FromFile(GraphicsDevice, "flag.png");
            var poleRect = new Rectangle(Config.TileSize * 198, Config.TileSize * 3, 5, 160);

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

            // Collision system
            _collisionManager = new CollisionManager(
            _marioState,
            _mapTiles,
            _enemyManager,
            _powerupManager,
            _screenManager,
            _sound,
            _flagpole,
            _spawnPoint,
            Config.TileSize,
            _smallMario,
            _bigMario,
            _camera
        );



            // UI Screens
            _font = Content.Load<SpriteFont>("marioFont");
            _coin = Texture2D.FromFile(GraphicsDevice, "coin2.png");

            _hud = new HUDScreen(_font, _coin, _screenManager);
            _titleScreen = new TitleScreen(this, _screenManager, Texture2D.FromFile(GraphicsDevice, "titlescreen.png"), _font, _coin);
            _introScreen = new LevelIntroScreen(this, _screenManager, _font, _coin);
            _timeUpScreen = new TimeUpScreen(this, _screenManager, _font, _coin);
            _gameOverScreen = new GameOverScreen(this, _screenManager, _font, _coin);

            // Pause
            _pauseScreen = new PauseScreen(this, _screenManager, _font);
        }

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


            // Non-gameplay screens
            _screenManager.Update(gameTime);
            switch (_screenManager.CurrentState)
            {
                case GameState.Title: _titleScreen.Update(gameTime); return;
                case GameState.LevelIntro: _introScreen.Update(gameTime); return;
                case GameState.TimeUp: _timeUpScreen.Update(gameTime); return;
                case GameState.GameOver: _gameOverScreen.Update(gameTime); return;
            }

            if (_showChristmasText)
            {
                _christmasTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_christmasTimer <= 0)
                    _showChristmasText = false;
            }


            if (!InputLocked)
                _input.Update();

            DebugGodMode = kb.IsKeyDown(Keys.D);

            // Mario and gameplay systems
            _marioState.Update(gameTime);
            if (!InputLocked)
                _marioState.CurrentMario.Update(gameTime);

            _collisionManager.Update(gameTime, _marioState.CurrentMario, (CameraManager)_camera);
            _enemyManager.Update(
                    gameTime,
                    _mapTiles,
                    _marioState.CurrentMario.Position
                );


            _camera.LookAt(_marioState.CurrentMario.Position);

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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(92, 148, 252));

            // Title and other menus
            _spriteBatch.Begin();
            switch (_screenManager.CurrentState)
            {
                case GameState.Title: _titleScreen.Draw(_spriteBatch); _spriteBatch.End(); return;
                case GameState.LevelIntro: _introScreen.Draw(_spriteBatch); _spriteBatch.End(); return;
                case GameState.TimeUp: _timeUpScreen.Draw(_spriteBatch); _spriteBatch.End(); return;
                case GameState.GameOver: _gameOverScreen.Draw(_spriteBatch); _spriteBatch.End(); return;
            }
            _spriteBatch.End();

            // Main game world
            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
            _backgroundManager.Draw(_spriteBatch, 0f);

            foreach (var tile in _mapTiles)
                tile.Draw(_spriteBatch);

            _marioState.CurrentMario.Draw(_spriteBatch, _marioState.CurrentMario.Position);
            _enemyManager.Draw(_spriteBatch);
            _powerupManager.Draw(_spriteBatch);
            _flagpole.Draw();
            _spriteBatch.End();

            // HUD and overlays
            _spriteBatch.Begin();
            _hud.Draw(_spriteBatch);
            if (_showChristmasText)
            {
                _spriteBatch.DrawString(
                    _font,
                    "CHRISTMAS MODE",
                    new Vector2(50, 50),
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
            _screenManager = new ScreenManager();
            _pauseScreen = new PauseScreen(this, _screenManager, _font);

            // Recreate core systems
            _enemyManager = new EnemyManager(Config.TileSize);
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
            PendingDeathTimer = 5.0f;
        }


    }

}
