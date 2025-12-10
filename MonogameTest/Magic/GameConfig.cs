namespace MonogameTest.Config
{
    public class ColorConfig
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
    }

    public class GameConfig
    {
        public int ViewWidth { get; set; }
        public int ViewHeight { get; set; }
        public int Scale { get; set; }
        public int ScaleMod { get; set; }
        public int TileSize { get; set; }
        public float DeathTimerSeconds { get; set; }
        public float ChristmasModeDuration { get; set; }
        public float ChristmasTextX { get; set; }
        public float ChristmasTextY { get; set; }
        public ColorConfig BackgroundColor { get; set; }
        public int SpawnTileX { get; set; }
        public int SpawnTileY { get; set; }
    }
}
