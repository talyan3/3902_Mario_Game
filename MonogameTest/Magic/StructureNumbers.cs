public class StructureNumbers
{
    public CameraMan CameraMan { get; set; }
    public GoombaNum Goomba { get; set; }
}

public class CameraMan
{
    public float SmoothSpeed { get; set; }
    public float HorizontalOffsetRatio { get; set; }
    public float NesViewWidth { get; set; }
    public float ZoomDivisor { get; set; }
    public float LeftClamp { get; set; }
    public float FixedCameraY { get; set; }
    public float TileSize { get; set; }
}

public class GoombaNum
{
    public float SpriteWidth { get; set; }
    public float SpriteHeight { get; set; }
    public float Speed { get; set; }
    public float AnimationDelay { get; set; }
    public float ClampMin { get; set; }
    public float ClampMax { get; set; }
}

