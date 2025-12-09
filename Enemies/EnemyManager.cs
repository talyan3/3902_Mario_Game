using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
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
        private readonly MarioStateController marioState;

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
        }

        // ===========================
        // UPDATE
        // ===========================

        public void Update(GameTime gameTime, List<Tile> mapTiles)
        {
            // ---- UPDATE GOOMBAS ----
            foreach (var g in _goombas)
            {
                if (!g.IsAlive)
                    continue;

                g.Update(gameTime);

                // ---------------------------------------
                // NEW: Check if Goomba is about to walk off a cliff
                // ---------------------------------------
                if (IsAtCliff(g, mapTiles))
                {
                    g.ReverseDirection();
                }

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
                // ---------------------------------------
                // NEW: Check if Goomba is about to walk off a cliff
                // ---------------------------------------
                if (IsAtCliff_K(_koopa, mapTiles))
                {
                    _koopa.ReverseDirection();
                }

                if (EnemyCollisionHandler.HandleMany(_koopa, mapTiles, out var res, out var tile))
                {
                    if (res.HitWall)
                        _koopa.ReverseDirection();
                }
            }
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
                _koopa.ResetState();   // <-- ADD THIS
                _koopa.Position = _koopaSpawnTile * _tileSize;
            }
        }
        private bool IsAtCliff(moveGoom g, List<Tile> tiles)
        {
            // Feet position one pixel below Goomba
            Rectangle futureFeet = new Rectangle(
                g.Bounds.Center.X,
                g.Bounds.Bottom + 1,
                2,   // tiny check width
                2
            );

            foreach (var t in tiles)
            {
                if (t.TileName == "Air") 
                    continue;

                if (futureFeet.Intersects(t.Bounds))
                    return false; // Solid ground exists
            }

            return true; // No ground → this is a cliff
        }
        private bool IsAtCliff_K(moveKoop k, List<Tile> tiles)
        {
            // Feet position one pixel below Goomba
            Rectangle futureFeet = new Rectangle(
                k.Bounds.Center.X,
                k.Bounds.Bottom + 1,
                2,   // tiny check width
                2
            );

            foreach (var t in tiles)
            {
                if (t.TileName == "Air") 
                    continue;

                if (futureFeet.Intersects(t.Bounds))
                    return false; // Solid ground exists
            }

            return true; // No ground → this is a cliff
        }
    }
}
