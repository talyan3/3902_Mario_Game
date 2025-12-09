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
        private GameConfig C => ConfigLoader.Config;

        // ===========================
        // CORE ENGINE OBJECTS
        // ===========================
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private KeyboardController _input;
        private ScreenManager _screenManager;
        private SoundManager _sound;
        public static bool InputLocked = false;


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

        // ===========================
        // CONFIG SHORTCUTS
        // ===========================
        private int TileSize => C.TileSize;
        private int ViewWidth => C.ViewWidth;
        private int Scale => C.Scale;
        private int ScaleMod => C.ScaleMod;
        public static bool DebugGodMode = false;

        //CHRISTMAS MODE
        public static bool ChristmasMode = false;

        private bool _cWasDown = false;



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
            var flagTex = Texture2D.FromFile(GraphicsDevice, "flag.png");
            var poleRect = new Rectangle(TileSize * 198, TileSize * 3, 5, 160);
            _flagpole = new Flagpole(
                _spriteBatch,
                flagTex,
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
                TileSize,
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
        }

        // =========================================================
        // UPDATE
        // =========================================================
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
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
                _input.Update();


            // DEBUG GOD MODE (HOLD D)
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                DebugGodMode = true;
            else
                DebugGodMode = false;


            _marioState.Update(gameTime);
            if (!InputLocked)
            {
                _marioState.CurrentMario.Update(gameTime);
            }
            _collisionManager.Update(gameTime, _marioState.CurrentMario, (CameraManager)_camera);

            _camera.LookAt(_marioState.CurrentMario.Position);
            _enemyManager.Update(gameTime, _mapTiles);

            if (Keyboard.GetState().IsKeyDown(Keys.M))
                _sound.ToggleMute();

            var kb = Keyboard.GetState();

            //PRESS C TO TOGGLE CHRISTMAS MODE
            if (kb.IsKeyDown(Keys.C) && !_cWasDown)
            {
                ChristmasMode = !ChristmasMode;   // toggle on/off
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
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
