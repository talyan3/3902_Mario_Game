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
        private const int MapWidth = 208;  // Level width in tiles
        private const int MapHeight = 15;  // Level height in tiles

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

    // Local helper to label known gids
    static string NameForGid(int gid) => gid switch
    {
        1  => "Ground",
        2  => "Brick",
        5  => "Question",
        7  => "Question",
        8  => "PipeTopLeft",
        9  => "PipeTopRight",
        10 => "PipeBodyLeft",
        11 => "PipeBodyRight",
        _  => $"gid={gid}"
    };

    foreach (var layer in map.layers)
    {
        if (layer.type != "tilelayer" || layer.data == null)
            continue;

        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                int index = y * MapWidth + x;
                int tileId0 = layer.data[index] - 1;   // 0-based; -1 means empty
                if (tileId0 < 0) continue;

                int srcX = (tileId0 % tilesPerRow) * tileW;
                int srcY = (tileId0 / tilesPerRow) * tileH;

                Rectangle srcRect = new Rectangle(srcX, srcY, tileW, tileH);
                Vector2 pos = new Vector2(x * tileW, y * tileH);

                int gid = tileId0 + 1;               // back to 1-based (matches JSON)
                string name = NameForGid(gid);

                tiles.Add(new Tile(tileset, srcRect, pos, gid, name));
            }
        }
    }

    return tiles;
        }
    }
}
