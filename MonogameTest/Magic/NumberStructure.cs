using System.Collections.Generic;
using MonogameTest;

public class NumberStructure
{
    public CameraMan CameraMan { get; set; }
    public EnemyCollision EnemyCollision { get; set; }
    public GoombaNum GoombaM { get; set; }
    public KoopaNum KoopaM { get; set; }
    public MapSettings MapSettings { get; set; }
    public Dictionary<int, string> TileGrid { get; set; }
    public PhysicsNum GPhysics { get; set; }

    public PlayerSmall PlayerSmall { get; set; }
    public PlayerNumbers PlayerPhysics { get; set; }
    public PlayerAnimation PlayerAnimations { get; set; }

    public BigMarioNumbers BigMario { get; set; }

    public SmallMarioNumbers SmallMario { get; set; }

    public GameNumbers GameNum { get; set; }



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

public class EnemyCollision
{
    public int EnemyInflateX { get; set; }
    public int StompInflateX { get; set; }
    public int FeetHeight { get; set; }
    public int StompThreshold { get; set; }
    public float SmallMarioBounce { get; set; }
    public float BigMarioBounce { get; set; }
}

public class GoombaNum
{
    public float SpriteWidth { get; set; }
    public float SpriteHeight { get; set; }
    public float Speed { get; set; }
    public float AnimationDelay { get; set; }
    public float ClampMin { get; set; }
    public float ClampMax { get; set; }
    public float Direction { get; set; }
    public float Frames { get; set; }
}

public class KoopaNum
{
    public int DestWidth { get; set; }
    public int DestHeight { get; set; }
    public int SrcWidth { get; set; }
    public int SrcHeight { get; set; }
    public float Speed { get; set; }
    public float AnimationDelay { get; set; }
    public float ClampMin { get; set; }
    public float ClampMax { get; set; }
    public int[] LeftFrames { get; set; }
    public int[] RightFrames { get; set; }

    public float Direction { get; set; }
    public float Flag { get; set; }
}

public class MapSettings
{
    public int MapWidth { get; set; }
    public int MapHeight { get; set; }
}

public class PhysicsNum
{
    public float Gravity {get; set;}
    public float GroundY {get; set;}
}

public class PlayerNumbers
{
    public float MoveAcceleration { get; set; }
    public float MaxMoveSpeed { get; set; }
    public float GroundFriction { get; set; }
    public float AirFriction { get; set; }
    public float JumpStrength { get; set; }
}

public class PlayerSmall
{
    public int StartX { get; set; }
    public int StartY { get; set; }
    public float Scale { get; set; }
}

public class AnimInfo
{
    public int FrameWidth { get; set; }
    public int FrameHeight { get; set; }
    public int FrameCount { get; set; }
    public float FrameDuration { get; set; }
    public int StartFrame { get; set; }
}

public class PlayerAnimation
{
    public string Texture { get; set; }
    public AnimInfo Idle { get; set; }
    public AnimInfo Run { get; set; }
    public AnimInfo Jump { get; set; }
}

public class BigMarioNumbers
{
    public float MoveSpeed { get; set; }
    public float SprintMultiplier { get; set; }

    public int FrameWidth { get; set; }
    public int FrameHeight { get; set; }
    public int SheetColumnWidth { get; set; }

    public List<int> RunFrames { get; set; }
    public int IdleFrame { get; set; }
    public int JumpFrame { get; set; }
    public int CrouchFrame { get; set; }

    public float FrameTime { get; set; }

    public float JumpOffset { get; set; }
    public float BounceHeight { get; set; }

    public float CrouchOffset { get; set; }
}

public class SmallMarioNumbers
{
    public float MoveSpeed { get; set; }
    public float SprintMultiplier { get; set; }

    public int FrameWidth { get; set; }
    public int FrameHeight { get; set; }
    public int SheetColumnWidth { get; set; }

    public List<int> RunFrames { get; set; }
    public int CrouchFrame { get; set; }
    public int JumpFrame { get; set; }
    public int IdleFrame { get; set; }

    public float FrameTime { get; set; }

    public float JumpStrength { get; set; }
    public float Gravity { get; set; }

    public float BounceHeight { get; set; }
}

public class GameNumbers
{
    public int Scale { get; set; }
    public int TitlePosX { get; set; }
    public int TopPosY { get; set; }
    public int ScorePosY { get; set; }
    public int CoinsPosX { get; set; }
    public int LowPosY { get; set; }
    public int WorldPosX { get; set; }
    public int LevelPosX { get; set; }
    public int TimePosX { get; set; }
    public int ElapsedTimeX { get; set; }

    public int TilePosHighY { get; set; }
    public int TilePosLowY { get; set; }
    public int BackBufferHeight { get; set; }

    public int ColorFirst { get; set; }
    public int ColorSecond { get; set; }
    public int ColorThird { get; set; }
    public int TopLeftPosX { get; set; }
    public int TopLeftPosY { get; set; }

    public int StartMarioX { get; set; }
    public int StartMarioY { get; set; }

    public int MushroomPosX { get; set; }
    public int MushroomPosY { get; set; }

    public int KoopaPosX { get; set; }
    public int KoopaPosY { get; set; }
    public float SpriteScale { get; set; }

    public double TimeStart {get; set;}

}

