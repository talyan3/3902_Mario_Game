using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Collections.Generic;
using MonogameTest.Sounds;

namespace MonogameTest.Managers
{
    public class TileCollisionHandler
    {
        private readonly List<Tile> _tiles;
        private readonly HashSet<Tile> _usedQuestionBlocks = new();
        private readonly PowerupFieldManager _powerupManager;
        private readonly SoundManager _sound;
        private readonly int _tileSize;
        private readonly System.Action<int> _addScore;
        private readonly System.Action _addCoin;

        public TileCollisionHandler(
            List<Tile> tiles,
            PowerupFieldManager powerupManager,
            SoundManager sound,
            int tileSize,
            System.Action<int> addScore,
            System.Action addCoin)
        {
            _tiles = tiles;
            _powerupManager = powerupManager;
            _sound = sound;
            _tileSize = tileSize;
            _addScore = addScore;
            _addCoin = addCoin;
        }

        private void HandleTileCollision(StaticSprite activeMario)
        {
            if (!StaticCollisionHandler.HandleMany(activeMario, mapTiles, out var res, out var hitTile))
                return;

            // --- HEAD HIT LOGIC ---
            if ((hitTile.TileName == "Question" || hitTile.TileName == "Brick") &&
                res.Side == typeCollision.Bottom &&
                !usedQuestionBlocks.Contains(hitTile))
            {
                usedQuestionBlocks.Add(hitTile);
                sound.PlayEffect("bump");

                Vector2 spawnPos = hitTile.Position;
                spawnPos.Y -= tileSize;

                if (hitTile.TileName == "Question")
                {
                    if (activeMario.Position.X < tileSize * 22 &&
                        activeMario.Position.X > tileSize * 19)
                    {
                        powerupManager.Spawn(PowerupType.Mushroom, spawnPos);
                        sound.PlayEffect("powerUpAppears");
                    }
                    else
                    {
                        powerupManager.Spawn(PowerupType.Coin, spawnPos);
                        sound.PlayEffect("coin");
                        addScore?.Invoke(100);
                        addCoin?.Invoke();
                    }
                }
                else if (hitTile.TileName == "Brick")
                {
                    sound.PlayEffect("break");
                }
            }
        }

        public void ClearUsedBlocks() =>
            _usedQuestionBlocks.Clear();
    }
}