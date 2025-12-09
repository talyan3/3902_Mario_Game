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

        // ===========================
        // GAMEPLAY SYSTEMS
        // ===========================
        private EnemyManager _enemyManager;
        private PowerupFieldManager _powerupManager;
        private CollisionManager _collisionManager;

        // ===========================
        // UI / SCREENS
        // ===========================
        private SpriteFont _font;
        private Texture2D _coin;
        private HUDScreen _hud;

        private TitleScreen _titleScreen;
        private LevelIntroScreen _introScreen;
        private TimeUpScreen _timeUpScreen;
        private GameOverScreen _gameOverScreen;

        private Flagpole _flagpole;

        public static bool InputLocked { get; set; }
        public static bool DebugGodMode { get; set; }
        public static bool ChristmasMode { get; set; }
        private bool _cWasDown;
        public static int ScreenWidth { get; private set; }
        public static int ScreenHeight { get; private set; }

        // ===========================
        // CONFIG SHORTCUTS
        // ===========================
        private GameConfig C => ConfigLoader.Config;
        private int TileSize => C.TileSize;
        private int ViewWidth => C.ViewWidth;
        private int Scale => C.Scale;
        private int ScaleMod => C.ScaleMod;

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
            _screenManager = new ScreenManager();

            _enemyManager = new EnemyManager(TileSize);
            _powerupManager = new PowerupFieldManager();

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
            _sound.PlaySong("mainTheme");


            // ---------- BACKGROUND ----------
            _backgroundManager = new BackgroundManager(GraphicsDevice, Content);
            _backgroundManager.LoadContent();

            // ---------- MARIO ----------
            _smallMario = new SmallMarioSprite(GraphicsDevice) { SoundManager = _sound };
            _bigMario = new BigMarioSprite(GraphicsDevice) { SoundManager = _sound };

            _spawnPoint = new Vector2(TileSize * 5, TileSize * 13);
            _smallMario.Position = _spawnPoint;
            _bigMario.Position = _spawnPoint;

            _marioState = new MarioStateController(_smallMario, _bigMario, _smallMario, TileSize);

            // ---------- MAP ----------
            using var fs = new FileStream("blocksV10.png", FileMode.Open);
            var tileset = Texture2D.FromStream(GraphicsDevice, fs);
            _mapTiles = TiledMapLoader.Load(Path.Combine(Directory.GetCurrentDirectory(), "level1.json"), tileset);

            // ---------- CAMERA ----------
            _camera = new CameraManager(GraphicsDevice.Viewport);
            _camera.Reset(_spawnPoint);

            // ---------- POWERUPS ----------
            var powerupsSheet = Content.Load<Texture2D>("Sprites/powerups");
            _powerupManager.LoadContent(Content, GraphicsDevice, _spriteBatch);
            _powerupManager.SetPowerupSheet(powerupsSheet);

            // ---------- ENEMIES ----------
            _enemyManager.LoadContent(Content, GraphicsDevice, _spriteBatch);

            // ---------- FLAGPOLE ----------
            var flagTexture = Texture2D.FromFile(GraphicsDevice, "flag.png");
            var poleRect = new Rectangle(TileSize * 198, TileSize * 3, 5, 160);
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
                points => _screenManager.AddScore(points),
                () => _screenManager.AddCoin(),
                () => _screenManager.ResetLevel()
            );

            // ---------- HUD / SCREENS ----------
            _font = Content.Load<SpriteFont>("marioFont");
            _coin = Texture2D.FromFile(GraphicsDevice, "coin2.png");

            _hud = new HUDScreen(_font, _coin, _screenManager);
            _titleScreen = new TitleScreen(this, _screenManager, Texture2D.FromFile(GraphicsDevice, "titlescreen.png"), _font, _coin);
            _introScreen = new LevelIntroScreen(this, _screenManager, _font, _coin);
            _timeUpScreen = new TimeUpScreen(this, _screenManager, _font, _coin);
            _gameOverScreen = new GameOverScreen(this, _screenManager, _font, _coin);

            _pauseScreen = new PauseScreen(this, _screenManager, _font);
        }

        // =========================================================
        // UPDATE
        // =========================================================
        protected override void Update(GameTime gameTime)
        {
            // Pause logic first
            _pauseScreen.Update(gameTime);
            if (_pauseScreen.IsPaused)
                return;

            var pad = GamePad.GetState(PlayerIndex.One);
            var kb = Keyboard.GetState();

            if (pad.Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape))
                Exit();

            _screenManager.Update(gameTime);

            switch (_screenManager.CurrentState)
            {
                case GameState.Title: _titleScreen.Update(gameTime); return;
                case GameState.LevelIntro: _introScreen.Update(gameTime); return;
                case GameState.TimeUp: _timeUpScreen.Update(gameTime); return;
                case GameState.GameOver: _gameOverScreen.Update(gameTime); return;
            }

            if (!InputLocked)
            {
               _input.Update(); 
            }

            DebugGodMode = kb.IsKeyDown(Keys.D);

            /*if (PauseManager.HandlePauseInput(Keyboard.GetState()))
                return;*/

            /*// DEBUG GOD MODE (HOLD D)
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                DebugGodMode = true;
            else
                DebugGodMode = false;

            if (Keyboard.GetState().IsKeyUp(Keys.D))
            {
                // prevents permanent hold lock
            }*/

            /*ResetManager.HandleSoftResetInput(
                Keyboard.GetState(),
                _smallMario,
                _bigMario,
                _marioState.CurrentMario,
                _spawnPoint,
                _camera
            );*/

            //_marioState.CurrentMario.Update(gameTime);
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
            }

            _collisionManager.Update(gameTime, _marioState.CurrentMario, (CameraManager)_camera);

            _camera.LookAt(_marioState.CurrentMario.Position);
            _enemyManager.Update(gameTime, _mapTiles);

            if (Keyboard.GetState().IsKeyDown(Keys.M))
                _sound.ToggleMute();
            
            // Christmas mode toggle
            if (kb.IsKeyDown(Keys.C) && !_cWasDown)
            {
                ChristmasMode = !ChristmasMode;
                SoundLoader.LoadAllSounds(this, _sound);
                _sound.PlaySong("mainTheme");
            }

            _cWasDown = kb.IsKeyDown(Keys.C);

            base.Update(gameTime);
        }

        // =========================================================
        // DRAW
        // =========================================================
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(92, 148, 252));

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

            _backgroundManager.Draw(_spriteBatch, 0f);

            foreach (var tile in _mapTiles)
                tile.Draw(_spriteBatch);

            _marioState.CurrentMario.Draw(_spriteBatch, _marioState.CurrentMario.Position);
            _enemyManager.Draw(_spriteBatch);
            _powerupManager.Draw(_spriteBatch);
            _flagpole.Draw();

            _spriteBatch.End();

            _spriteBatch.Begin();
            _hud.Draw(_spriteBatch);
            _pauseScreen.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
