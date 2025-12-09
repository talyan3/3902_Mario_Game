using Microsoft.Xna.Framework;
using MonogameTest.Screens;

namespace MonogameTest
{
    /// <summary>
    /// Handles all forms of resets: soft reset (R key), death reset, and full level reset.
    /// This removes reset clutter from Game1 entirely.
    /// </summary>
    public static class ResetManager
    {
        // =========================================================
        // SOFT RESET (R KEY)
        // =========================================================
        public static void SoftReset(
            SmallMarioSprite smallMario,
            BigMarioSprite bigMario,
            ref StaticSprite currentMario,
            Vector2 spawnPoint,
            ICamera camera)
        {
            ResetMarioPositions(smallMario, bigMario, ref currentMario, spawnPoint);
            ResetCamera(camera, spawnPoint);

            SoundManager.Instance.StopSong();
            SoundManager.Instance.PlaySong("mainTheme");
        }

        // =========================================================
        // FULL LEVEL RESET (GAME OVER / FLAG / TIME UP)
        // =========================================================
        public static void FullLevelReset(
            SmallMarioSprite smallMario,
            BigMarioSprite bigMario,
            ref StaticSprite currentMario,
            Vector2 spawnPoint,
            ICamera camera,
            HUDScreen hud,
            ScreenManager screenManager)
        {
            ResetMarioPositions(smallMario, bigMario, ref currentMario, spawnPoint);
            ResetCamera(camera, spawnPoint);

            screenManager.ResetLevel();

            SoundManager.Instance.StopSong();
            SoundManager.Instance.PlaySong("mainTheme");
            Game1.InputLocked = false;
        }

        // =========================================================
        // DEATH RESET (FORCES SMALL MARIO)
        // =========================================================
        public static void ResetOnDeath(
            ref StaticSprite currentMario,
            SmallMarioSprite smallMario)
        {
            if (smallMario != null)
                currentMario = smallMario;

            SoundManager.Instance.StopSong();
            SoundManager.Instance.PlaySong("youreDead", loop: false);
        }

        // =========================================================
        // INTERNAL HELPERS
        // =========================================================
        private static void ResetMarioPositions(
            SmallMarioSprite smallMario,
            BigMarioSprite bigMario,
            ref StaticSprite currentMario,
            Vector2 spawnPoint)
        {
            if (smallMario != null)
                smallMario.Position = spawnPoint;

            if (bigMario != null)
                bigMario.Position = spawnPoint;

            if (currentMario != null)
                currentMario.Position = spawnPoint;
        }

        private static void ResetCamera(ICamera camera, Vector2 spawnPoint)
        {
            if (camera == null) return;

            camera.Reset(spawnPoint);
            camera.LookAt(spawnPoint);
        }
    }
}
