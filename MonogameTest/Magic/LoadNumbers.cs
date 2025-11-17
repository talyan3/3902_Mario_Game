using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest
{
    public static class LoadNumbers
    {
        // This method applies all numbers from JSON to your camera
        public static void ApplyCameraNumbers(ICamera camera, StructureNumbers numbers)
        {
            camera.SmoothSpeed = numbers.CameraMan.SmoothSpeed;
            camera.HorizontalOffsetRatio = numbers.CameraMan.HorizontalOffsetRatio;
            camera.NesViewWidth = numbers.CameraMan.NesViewWidth;
            camera.ZoomDivisor = numbers.CameraMan.ZoomDivisor;
            camera.LeftClamp = numbers.CameraMan.LeftClamp;
            camera.FixedCameraY = numbers.CameraMan.FixedCameraY;
            camera.TileSize = numbers.CameraMan.TileSize;
        }

        public class GoombaNumbers
    {   
        public float SpriteWidth { get; set; }
        public float SpriteHeight { get; set; }
        public float Speed { get; set; }
        public float AnimationDelay { get; set; }
        public float ClampMin { get; set; }
        public float ClampMax { get; set; }
    }




    
    }
}
