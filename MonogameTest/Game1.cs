using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonogameTest.Sounds;
using MonogameTest.Screens;

namespace MonogameTest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public MarioManager MarioManager { get; set; } = new MarioManager();
        public CommandManager CommandManager { get; set; }

        // Gameplay fields
        private BlockManager blockManager;
        private PowerupManager powerupManager;
        public Texture2D goombaSprite;
        public Texture2D koopaSprite;
        public ISprite goom;
        public ISprite koop;

        private SmallMarioSprite _smallMario;
        private BigMarioSprite _bigMario;
        private StaticSprite _currentMario;
        private bool _isBig = false;
        private bool _bHeldLast = false;

        private Texture2D _tileset;
        private List<Tile> _mapTiles;

        const int TilesVisibleX = 16;
        const int TileSize = 16;
        const int ViewWidth = TilesVisibleX * TileSize;
        const int scale = 4;

        private ICamera camera;
        private MarioPhysiscsTest Mar;
        private Texture2D Hollow;
        private Texture2D platformTexture;
        private Rectangle platformRect;

        private List<Rectangle> _solidRects;
        private Vector2 _spawnPoint;
        private BackgroundManager _backgroundManager;

        // Powerups and enemies
        public Texture2D powerupTexture;
        public ISprite mushroom;

        // Screen + HUD
        private ScreenManager _screenManager;
        private HUDScreen _hud;

        private TitleScreen _titleScreen;
        private LevelIntroScreen _introScreen;
        private TimeUpScreen _timeUpScreen;
        private GameOverScreen _gameOverScreen;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            CommandManager = new CommandManager(this, MarioManager);
            _graphics.PreferredBackBufferWidth = ViewWidth * scale;
            _graphics.PreferredBackBufferHeight = 240 * scale;
            _graphics.ApplyChanges();

            _screenManager = new ScreenManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SoundLoader.LoadAllSounds(this);
            SoundManager.Instance.PlaySong("mainTheme");

            _backgroundManager = new BackgroundManager(GraphicsDevice);
            _backgroundManager.LoadContent();

            // Enemy sprites
            goombaSprite = Content.Load<Texture2D>("Sprites/goomba-Final");
            koopaSprite = Content.Load<Texture2D>("Sprites/green-koopa");
            goom = new moveGoom(goombaSprite, _spriteBatch);
            koop = new moveKoop(koopaSprite, _spriteBatch);
            (goom as moveGoom).Position = new Vector2(16 * 20, 16 * 12);
            (koop as moveKoop).Position = new Vector2(16 * 25, 16 * 12);

            // Powerups
            powerupTexture = Texture2D.FromFile(GraphicsDevice, "powerups.png");
            mushroom = new movePower(powerupTexture, _spriteBatch);
            (mushroom as movePower).Position = new Vector2(16 * 10, 16 * 11);

            new SpriteCommand(GraphicsDevice, MarioManager).Execute();

            // Mario
            _smallMario = new SmallMarioSprite(GraphicsDevice);
            _bigMario = new BigMarioSprite(GraphicsDevice);
            var pos = new Vector2(16 * 5, 16 * 13);
            _smallMario.Position = pos;
            _bigMario.Position = pos;
            _spawnPoint = pos;
            _currentMario = _smallMario;

            // Tileset + map
            using (FileStream fs = new FileStream("blocksV10.png", FileMode.Open))
                _tileset = Texture2D.FromStream(GraphicsDevice, fs);

            string mapPath = Path.Combine(Directory.GetCurrentDirectory(), "level1.json");
            _mapTiles = TiledMapLoader.Load(mapPath, _tileset);
            _solidRects = new List<Rectangle>(_mapTiles.Count);
            foreach (var tile in _mapTiles)
                _solidRects.Add(tile.Bounds);

            // Camera
            camera = new CameraManager(GraphicsDevice.Viewport);
            camera.Reset(_currentMario.Position);
            camera.LookAt(_currentMario.Position);

            // Physics test
            Hollow = Texture2D.FromFile(GraphicsDevice, "mario-static.png");
            Mar = new MarioPhysiscsTest(Hollow);
            platformTexture = new Texture2D(GraphicsDevice, 1, 1);
            platformRect = new Rectangle(0, 209, 5000, 50);

            // Load HUD assets
            var font = Content.Load<SpriteFont>("marioFont");
            var coinTex = Texture2D.FromFile(GraphicsDevice, "coin2.png");
            var titleTex = Content.Load<Texture2D>("titlescreen");


            _hud = new HUDScreen(font, coinTex, _screenManager);

            // Load screens
            _titleScreen = new TitleScreen(this, _screenManager, titleTex, font, coinTex);
            _introScreen = new LevelIntroScreen(this, _screenManager, font, coinTex);
            _timeUpScreen = new TimeUpScreen(this, _screenManager, font, coinTex);
            _gameOverScreen = new GameOverScreen(this, _screenManager, font, coinTex);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
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

            // ----------- GAMEPLAY UPDATE -----------

            CommandManager.checkKeys();
            CommandManager.checkClicks();

            if (MarioManager.ActiveSprite != null)
                MarioManager.ActiveSprite.Update(gameTime);

            KeyboardState state = Keyboard.GetState();
            bool bDown = state.IsKeyDown(Keys.B);
            if (bDown && !_bHeldLast)
                ToggleMarioSize();
            _bHeldLast = bDown;

            if (state.IsKeyDown(Keys.R))
                ResetLevel();

            if (_currentMario is BigMarioSprite bigMario)
                bigMario.Update(gameTime, camera.LeftEdge);
            else if (_currentMario is SmallMarioSprite smallMario)
                smallMario.Update(gameTime, camera.LeftEdge);
            else
                _currentMario.Update(gameTime);

            camera.LookAt(_currentMario.Position);

            StaticCollisionHandler.HandleMany(_currentMario, _mapTiles, out _, out _);

            Mar.Update(gameTime, state, platformRect);
            goom.Update(gameTime);
            koop.Update(gameTime);
            mushroom.Update(gameTime);

            HandleEnemyCollisions();

            //--------------------------------------------------------------------
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

        private void HandleEnemyCollisions()
        {
            var enemies = new List<object>();
            if (goom is moveGoom g2 && g2.IsAlive) enemies.Add(g2);
            if (koop is moveKoop k2 && k2.IsAlive) enemies.Add(k2);

            if (_isBig)
            {
                EnemyCollisionHandler.HandleMarioEnemyCollision(
                    _bigMario, enemies,
                    onBigHit: () =>
                    {
                        _isBig = false;
                        _smallMario.Position = _bigMario.Position;
                        var deltaFeet = _bigMario.Bounds.Bottom - _smallMario.Bounds.Bottom;
                        _smallMario.Position = new Vector2(_smallMario.Position.X,
                                                           _smallMario.Position.Y + deltaFeet);
                        _currentMario = _smallMario;
                    });
            }
            else
            {
                EnemyCollisionHandler.HandleMarioEnemyCollision(
                    _smallMario, enemies,
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
                    });
            }
        }

        private void ToggleMarioSize()
        {
            _isBig = !_isBig;
            Vector2 pos = _currentMario.Position;
            _currentMario = _isBig ? (StaticSprite)_bigMario : _smallMario;
            _currentMario.Position = pos;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(92, 148, 252));

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
                    return;

                case GameState.Playing:
                    break;
            }

            _spriteBatch.End();

            // ----------- GAMEPLAY DRAW ---------------------
            _spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());

            _backgroundManager.Draw(_spriteBatch, cameraX: 0f);
            _currentMario.Draw(_spriteBatch, _currentMario.Position);

            foreach (var tile in _mapTiles)
                tile.Draw(_spriteBatch);

            Mar.Draw(_spriteBatch);

            if (goom is moveGoom g && g.IsAlive)
                g.Draw(_spriteBatch, g.Position);

            if (koop is moveKoop k && k.IsAlive)
                k.Draw(_spriteBatch, k.Position);

            if (mushroom is movePower m && m.IsAlive)
                m.Draw(_spriteBatch, m.Position);

            _spriteBatch.End();

            // HUD
            _spriteBatch.Begin();
            _hud.Draw(_spriteBatch);
            _spriteBatch.End();

            
            
            
            base.Draw(gameTime);
        }
    }
}
