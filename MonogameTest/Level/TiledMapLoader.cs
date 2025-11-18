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
        //Magic Numbers
        private static readonly MapSettings MapNumbers = NumberLoad.Numbers.MapSettings;
        private static readonly Dictionary<int, string> TileNames = NumberLoad.Numbers.TileGrid;

        //MY WARNING IF IT DOESNT WORK!!!!!!!!!!!!!!!!
        //I changed these below from constant to static and added the => because that was what made it work
        //idk if that causes an issue, I really hope it does not
        public static int MapWidth => MapNumbers.MapWidth;  // Number of tiles horizontally
        public static int MapHeight => MapNumbers.MapHeight;  // Number of tiles vertically

        // Represents the basic structure of a Tiled map
        private class TiledMap
        {
            public int tilewidth { get; set; }
            public int tileheight { get; set; }
            public List<TiledLayer> layers { get; set; }
        }

        // Represents a single Tiled layer
        private class TiledLayer
        {
            public string name { get; set; }
            public string type { get; set; }
            public List<int> data { get; set; }
        }

        public static List<Tile> Load(string jsonPath, Texture2D tileset)
        {
            // Read and parse the map JSON exported from Tiled
            string json = File.ReadAllText(jsonPath);
            var map = JsonSerializer.Deserialize<TiledMap>(json);

            // Create the final tile list to return
            var tiles = new List<Tile>();
            int tileW = map.tilewidth;
            int tileH = map.tileheight;

            // Local helper to label known gids -- CHANGED THIS ONE A BIT
            static string NameForGid(int gid)
            {

                string key = gid.ToString();
                if (TileNames.ContainsKey(gid))
                    return TileNames[gid];
                else
                    return $"gid={gid}";
            };

            // Loop through each layer in the map
            foreach (var layer in map.layers)
            {
                // Only process visible tile layers
                if (layer.type != "tilelayer" || layer.data == null)
                    continue;

                // Loop through every tile coordinate
                for (int y = 0; y < MapHeight; y++)
                {
                    for (int x = 0; x < MapWidth; x++)
                    {
                        // Compute the flat array index (row-major order)
                        int index = y * MapWidth + x;

                        // Tiled uses 1-based indexing for tile IDs
                        int tileId = layer.data[index] - 1;

                        // Skip blank tiles (0 in Tiled)
                        if (tileId < 0)
                            continue;

                        // Because the spritesheet is a single row,
                        // the source X offset is simply (tileId * tile width)
                        Rectangle srcRect = new Rectangle(
                            tileId * tileW, // srcX
                            0,              // srcY always 0 since one row
                            tileW,
                            tileH
                        );

                        // Destination position in world space
                        Vector2 pos = new Vector2(x * tileW, y * tileH);

                        int gid = tileId + 1;               // back to 1-based (matches JSON)
                        string name = NameForGid(gid);

                        // Add a new Tile using this source rectangle and position
                        tiles.Add(new Tile(tileset, srcRect, pos));
                    }
                }
            }

            return tiles;
        }
    }
}


