using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonogameTest.Sounds; 

namespace MonogameTest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public MarioManager MarioManager { get; set; } = new MarioManager();
        public CommandManager CommandManager { get; set; }
        private BlockManager blockManager;
        private PowerupManager powerupManager;
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
        KeyboardState previousState;
        const int ViewWidth = TilesVisibleX * TileSize;
        private ICamera camera;
        const int scale = 4;
        private MarioPhysiscsTest Mar;
        Texture2D Hollow;
        Texture2D platformTexture;
        Rectangle platformRect;
        private List<Rectangle> _solidRects;
        private Vector2 _spawnPoint;
        private BackgroundManager _backgroundManager;

        // Powerups and enemies
        public Texture2D powerupTexture;
        public ISprite mushroom;
        private List<object> _enemies = new List<object>();

        // UI
        private SpriteFont myFont;
        private string coins = "00";
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
            CommandManager = new CommandManager(this, MarioManager);
            _graphics.PreferredBackBufferWidth = ViewWidth * scale;
            _graphics.PreferredBackBufferHeight = 240 * scale;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            
            SoundLoader.LoadAllSounds(this);
            SoundManager.Instance.PlaySong("mainTheme"); // auto start overworld theme

            _backgroundManager = new BackgroundManager(GraphicsDevice);
            _backgroundManager.LoadContent();

            // Enemies
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

            // Tileset + Map
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

            // Physics Test
            Hollow = Texture2D.FromFile(GraphicsDevice, "mario-static.png");
            Mar = new MarioPhysiscsTest(Hollow);
            platformTexture = new Texture2D(GraphicsDevice, 1, 1);
            platformRect = new Rectangle(0, 209, 5000, 50);

            // Fonts & UI
            myFont = Content.Load<SpriteFont>("marioFont");
            coin = Texture2D.FromFile(GraphicsDevice, "coin2.png");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            CommandManager.checkKeys();
            CommandManager.checkClicks();
            if (MarioManager.ActiveSprite != null)
                MarioManager.ActiveSprite.Update(gameTime);

            KeyboardState state = Keyboard.GetState();
            previousState = state;

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

            if (_currentMario is BigMarioSprite bigMario)
            {
                bigMario.Update(gameTime, camera.LeftEdge);
            }
            else if (_currentMario is SmallMarioSprite smallMario)
            {
                smallMario.Update(gameTime, camera.LeftEdge);
            }
            else
            {
                _currentMario.Update(gameTime);
            }

            Vector2 marioPos = _currentMario.Position;
            camera.LookAt(marioPos);


            if (StaticCollisionHandler.HandleMany(_currentMario, _mapTiles, out var res, out var hitTile))
                Console.WriteLine($"Mario hit {hitTile.TileName} (gid={hitTile.Gid}) at {hitTile.Position} | Side={res.Side} | MTV={res.MTV}");

            Mar.Update(gameTime, state, platformRect);
            goom.Update(gameTime);
            koop.Update(gameTime);
            mushroom.Update(gameTime);

            // Enemy collisions
            HandleEnemyCollisions();

            time -= 0.02;
            base.Update(gameTime);
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
                        _smallMario.Position = new Vector2(_smallMario.Position.X, _smallMario.Position.Y + deltaFeet);
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
                    });
            }
        }

        private void ToggleMarioSize()
        {
            _isBig = !_isBig;
            Vector2 pos = (_currentMario is SmallMarioSprite sm ? sm.Position :
                           (_currentMario is BigMarioSprite bm ? bm.Position : Vector2.Zero));
            _currentMario = _isBig ? (StaticSprite)_bigMario : _smallMario;
            if (_currentMario is SmallMarioSprite sm2) sm2.Position = pos;
            if (_currentMario is BigMarioSprite bm2) bm2.Position = pos;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(92, 148, 252));
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
            if (mushroom != null && (mushroom as movePower).IsAlive)
                mushroom.Draw(_spriteBatch, (mushroom as movePower).Position);

            _spriteBatch.End();
            _spriteBatch.Begin();

            _spriteBatch.DrawString(myFont, "MARIO", new Vector2(90, 15), Color.White);
            _spriteBatch.DrawString(myFont, "000000", new Vector2(90, 55), Color.White);
            _spriteBatch.DrawString(myFont, "x" + coins, new Vector2(375, 55), Color.White);
            _spriteBatch.DrawString(myFont, "WORLD", new Vector2(550, 15), Color.White);
            _spriteBatch.DrawString(myFont, "1-1", new Vector2(580, 55), Color.White);
            _spriteBatch.DrawString(myFont, "TIME", new Vector2(800, 15), Color.White);
            _spriteBatch.DrawString(myFont, ((int)time).ToString(), new Vector2(825, 55), Color.White);
            _spriteBatch.Draw(coin, new Vector2(330, 45), null, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
