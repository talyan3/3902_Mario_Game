using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest
{
    public static class TiledMapLoader
    {
        public const int MapWidth = 208;  // Level width in tiles
        public const int MapHeight = 15;  // Level height in tiles

        private class TiledMap
        {
            public int tilewidth { get; set; }
            public int tileheight { get; set; }
            public List<TiledLayer> layers { get; set; }
        }

        private class TiledLayer
        {
            public string name { get; set; }
            public string type { get; set; }
            public List<int> data { get; set; }
        }

        public static List<Tile> Load(string jsonPath, Texture2D tileset, int tilesPerRow)
        {
            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"Could not find level file: {jsonPath}");

            string json = File.ReadAllText(jsonPath);
            var map = JsonSerializer.Deserialize<TiledMap>(json);

            if (map == null)
                throw new Exception("Failed to parse Tiled map JSON.");

            var tiles = new List<Tile>();
            int tileW = map.tilewidth;
            int tileH = map.tileheight;

            foreach (var layer in map.layers)
            {
                if (layer.type != "tilelayer" || layer.data == null)
                    continue;

                for (int y = 0; y < MapHeight; y++)
                {
                    for (int x = 0; x < MapWidth; x++)
                    {
                        int index = y * MapWidth + x;
                        int tileId = layer.data[index] - 1; // Tiled uses 1-based indexing

                        if (tileId < 0)
                            continue; // skip empty

                        int srcX = (tileId % tilesPerRow) * tileW;
                        int srcY = (tileId / tilesPerRow) * tileH;

                        Rectangle srcRect = new Rectangle(srcX, srcY, tileW, tileH);
                        Vector2 pos = new Vector2(x * tileW, y * tileH);

                        tiles.Add(new Tile(tileset, srcRect, pos));
                    }
                }
            }

            return tiles;
        }
    }
}

