using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonogameTest.Snail;
using MonogameTest;
using System;




namespace MonogameTest.Managers
{
    public class EnemyManager
    {
        private readonly List<moveGoom> _goombas = new();
        private moveKoop _koopa;

        private Texture2D _goombaSprite;
        private Texture2D _koopaSprite;

        private SpriteBatch _spriteBatch;
        private readonly int _tileSize;

        private readonly Vector2 _koopaSpawnTile = new Vector2(106, 12);
        private Snail.Snail _snail;

        private readonly Random _rng = new Random();
        public Snail.Snail Snail => _snail;




        public EnemyManager(int tileSize)
        {
            _tileSize = tileSize;
        }

        // ===========================
        // PUBLIC ACCESSORS
        // ===========================

        public List<object> GetLiveEnemies()
        {
            var list = new List<object>();

            foreach (var g in _goombas)
                if (g.IsAlive)
                    list.Add(g);

            if (_koopa != null && _koopa.IsAlive)
                list.Add(_koopa);

            if (_snail != null && Game1.ChristmasMode)
                list.Add(_snail);   

            return list;
        }


        // ===========================
        // LOAD CONTENT
        // ===========================

        public void LoadContent(ContentManager content, GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;

            _goombaSprite = content.Load<Texture2D>("Sprites/goomba-Final");
            _koopaSprite = content.Load<Texture2D>("Sprites/green-koopa");

            try
{
    _goombaSprite = content.Load<Texture2D>("Sprites/goomba-Final");
}
catch (Exception ex)
{
    Console.WriteLine("Load failed: " + ex.Message);
}


            // ---- CREATE KOOPA ----
            _koopa = new moveKoop(_koopaSprite, spriteBatch)
            {
                Position = _koopaSpawnTile * _tileSize
            };

            // ---- LOAD GOOMBAS FROM STATIC POSITION DATA ----
            foreach (var posTile in EnemyPositions.Goombas)
            {
                var g = new moveGoom(_goombaSprite, spriteBatch);
                g.SpawnPosition = posTile * _tileSize;
                g.Position = g.SpawnPosition;
                _goombas.Add(g);
            }

            // Load snail textures directly from Snail folder
            Texture2D snail1 = Texture2D.FromFile(graphics, "Snail/snail1.png");
            Texture2D snail2 = Texture2D.FromFile(graphics, "Snail/snail2.png");

            // Spawn point (far left)
            Vector2 snailStart = new Vector2(200, 200);

            _snail = new Snail.Snail(snail1, snail2, snailStart);
        }

        // ===========================
        // UPDATE
        // ===========================

        public void Update(GameTime gameTime, List<Tile> mapTiles, Vector2 marioPos)
        {
            // ---- UPDATE GOOMBAS ----
            foreach (var g in _goombas)
            {
                if (!g.IsAlive)
                    continue;

                g.Update(gameTime);

                if (EnemyCollisionHandler.HandleMany(g, mapTiles, out var res, out var tile))
                {
                    if (res.HitWall)
                        g.ReverseDirection();
                }
            }

            // ---- UPDATE KOOPA ----
            if (_koopa != null && _koopa.IsAlive)
            {
                _koopa.Update(gameTime);

                if (EnemyCollisionHandler.HandleMany(_koopa, mapTiles, out var res, out var tile))
                {
                    if (res.HitWall)
                        _koopa.ReverseDirection();
                }
            }

            // Update snail
            // Snail tile collision
            if (_snail != null && Game1.ChristmasMode)
            {
                if (EnemyCollisionHandler.HandleMany(_snail, mapTiles, out var sResult, out var sTile))
                {
                    // Keep snail above ground
                    if (sResult.Grounded && sResult.MTV.Y < 0)
                    {
                        _snail.Position += new Vector2(0, sResult.MTV.Y);
                    }
                }

            }

            _snail?.Update(gameTime, marioPos);
        }

        // ===========================
        // DRAW
        // ===========================

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var g in _goombas)
                if (g.IsAlive)
                    g.Draw(spriteBatch, g.Position);

            if (_koopa != null && _koopa.IsAlive)
                _koopa.Draw(spriteBatch, _koopa.Position);

            _snail?.Draw(spriteBatch);
        }

        // ===========================
        // RESET
        // ===========================

        public void Reset()
        {
            foreach (var g in _goombas)
            {
                g.IsAlive = true;
                g.Position = g.SpawnPosition;
            }

            if (_koopa != null)
            {
                _koopa.IsAlive = true;
                _koopa.Position = _koopaSpawnTile * _tileSize;
            }
        }

        //SPAWN SNAIL
        private readonly Random _rng2 = new Random();

        public void ActivateSnail(Vector2 marioPos)
        {
            if (_snail == null)
                return;

            // World horizontal bounds — adjust as needed
            float minX = 0f;
            float maxX = 2500f;

            float randomX = (float)_rng2.NextDouble() * (maxX - minX) + minX;

            // Keep Y at Mario's height
            float y = marioPos.Y;

            _snail.Position = new Vector2(randomX, y);
        }


        public void ResetSnail(Vector2 spawnPoint)
        {
            if (_snail != null)
            {
                _snail.Position = spawnPoint;
                _snail.Reset();
            }
        }






    }
}
