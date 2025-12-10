using System.Collections.Generic;
using MonogameTest;
using MonogameTest.Managers;
using MonogameTest.Screens;

public class NumberStructure
{
    public CameraManager CameraManager { get; set; }
    public CollisionManager CollisionManager {get; set; }
    public EnemyCollision EnemyCollision { get; set; }
    public PowerupCoinState PowerupCoinState {get; set;}
    public PowerupFactory PowerupFactory {get; set;}
    public PowerupStarState PowerupStarState {get; set;}
    public goombaMove goombaMove { get; set; }
    public koopaMove koopaMove { get; set; }
    public KeyCombo KeyCombo { get; set; }
    public MapSettings MapSettings { get; set; }
    
    public Dictionary<int, string> TileGrid { get; set; }
    public Gphysics GPhysics { get; set; }

    public PlayerSmall PlayerSmall { get; set; }
    public PlayerNumbers PlayerPhysics { get; set; }
    public PlayerMario PlayerMario { get; set; }

    public GameOverScreen GameOverScreen { get; set; }
    public HUDScreen HUDScreen { get; set; }

    public LevelIntroScreen LevelIntroScreen { get; set; }

    public TimeUpScreen TimeUpScreen { get; set; }

    public BigMarioNumbers BigMario { get; set; }

    public SmallMarioNumbers SmallMario { get; set; }

    public Game1 Game1 { get; set; }

    public List<BackgroundElementInfo> BackgroundElements { get; set; }



}

public class CameraManager
{
    public float SmoothSpeed { get; set; }
    public float HorizontalOffsetRatio { get; set; }
    public float NesViewWidth { get; set; }
    public float ZoomDivisor { get; set; }
    public float LeftClamp { get; set; }
    public float FixedCameraY { get; set; }
    public float TileSize { get; set; }
    public int RightClampPadding { get; set; }
}

public class CollisionManager
{
    public double HurtInvincibleTime { get; set; }

    public int SpecialBlockStartTileX {get; set; }
    public int SpecialBlockEndTileX {get; set; }

    public int BrickScore { get; set; }
    public int QuestionCoinScore { get; set; }
    public int SmallCoinScore { get; set; }
    public int GreenMushroomScore { get; set; }
    public int StarScore {get; set;}
    public int FireFlowerScore {get; set;}

    public int PowerupSpawnYOffsetTiles {get; set;}
}

public class EnemyCollision
{
    public int EnemyInflateX { get; set; }
    public int StompInflateX { get; set; }
    public int SnailInflateX { get; set; }
    public int SnailKillScore { get; set; }
    public int StompThreshold { get; set; }
    public float BounecVelocity { get; set; }
    public int[] StompComboScores { get; set; }
}

public class PowerupCoinState
{
    public float RiseSpeed { get; set; }
    public float LifeTime { get; set; }
}

public class PowerupFactory
{
    public int TileSize {get; set;}

    public int MushroomX { get; set; }
    public int MushroomY { get; set; }

    public int GreenMushroomX { get; set; }
    public int GreenMushroomY { get; set; }

    public int FireFlowerX { get; set; }
    public int FireFlowerY { get; set; }

    public int StarX { get; set; }
    public int StarY { get; set; }

    public int CoinX { get; set; }
    public int CoinY { get; set; }

}

public class PowerupStarState
{
    public float BobFrequency { get; set; }
    public float BobAmplitude { get; set; }
}
public class goombaMove
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

public class koopaMove
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

public class KeyCombo
{
    public int Res { get; set; }
    public int Multiplier { get; set; }
}

public class MapSettings
{
    public int MapWidth { get; set; }
    public int MapHeight { get; set; }
}

public class Gphysics
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

public class PlayerMario
{
    public string Texture { get; set; }
    public AnimInfo Idle { get; set; }
    public AnimInfo Run { get; set; }
    public AnimInfo Jump { get; set; }
    public AnimInfo Star { get; set; }
}

public class GameOverScreen
{
    public double DefaultTimerSeconds { get; set; }
    public float TextOffsetX { get; set; }
    public float TextOffsetY { get; set; }
}

public class HUDScreen
{
        public float MarioTextX { get; set; }
        public float MarioTextY { get; set; }
        public float ScoreX { get; set; }
        public float ScoreY { get; set; }
        public float CoinTextureX { get; set; }
        public float CoinTextureY { get; set; }
        public float CoinScale { get; set; }
        public float CoinTextX { get; set; }
        public float CoinTextY { get; set; }
        public float TimeTextX { get; set; }
        public float TimeTextY { get; set; }
        public float TimerX { get; set; }
        public float TimerY { get; set; }
}
public class LevelIntroScreen
{
    public double DefaultIntroTime { get; set; }
        public float CoinScale { get; set; }

        public float MarioLabelX { get; set; }
        public float MarioLabelY { get; set; }

        public float ScoreX { get; set; }
        public float ScoreY { get; set; }

        public float CoinIconX { get; set; }
        public float CoinIconY { get; set; }

        public float CoinTextX { get; set; }
        public float CoinTextY { get; set; }

        public float WorldLabelX { get; set; }
        public float WorldLabelY { get; set; }

        public float WorldValueX { get; set; }
        public float WorldValueY { get; set; }

        public float TimeLabelX { get; set; }
        public float TimeLabelY { get; set; }

        public float TimeValueX { get; set; }
        public float TimeValueY { get; set; }

        public float WorldIntroX { get; set; }
        public float WorldIntroY { get; set; }

        public float MarioIntroX { get; set; }
        public float MarioIntroY { get; set; }

        public float LivesIntroX { get; set; }
        public float LivesIntroY { get; set; }
}

public class TimeUpScreen
{
    public double DefaultTimerSeconds { get; set; }
        public float TextOffsetX { get; set; }
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
    public float Scale { get; set; }
    public float JumpOffset { get; set; }
}

public class Game1
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

public class BackgroundElementInfo
{
    public string Type {get; set;}
    public float X {get; set;}
    public float Y {get; set;}
}