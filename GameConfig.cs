using System.Numerics; // Only for Vector2 — NOT MonoGame's Vector2

namespace MonogameTest
{
    public class GameConfig
    {
        public int TilesVisibleX { get; set; }
        public int TileSize { get; set; }
        public int Scale { get; set; }
        public int ScaleMod { get; set; }
        public float SpriteScale { get; set; }
        public int Cooldown { get; set; }
        public double StartingTime { get; set; }

        public int MarioStartX { get; set; }
        public int MarioStartY { get; set; }

        public int ViewWidth => TilesVisibleX * TileSize;
    }
}
