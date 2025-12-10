using Microsoft.Xna.Framework;
using MonogameTest.Screens;
using MonogameTest.Managers;
using MonogameTest.Sounds;

namespace MonogameTest
{
    /// <summary>
    /// Centralized system for all reset logic:
    /// - Soft reset / death reset (R key)
    /// - Full reset (Game Over / level restart)
    /// Resets Mario, camera, music, HUD, and Christmas/snail state.
    /// </summary>
    public static class ResetManager
    {
        // =========================================================
        // SOFT RESET (R KEY)
        // =========================================================
        public static void SoftReset(
            SmallMarioSprite smallMario,
            BigMarioSprite bigMario,
            MarioStateController state,
            Vector2 spawnPoint,
            ICamera camera,
            EnemyManager enemyManager)
        {
            // Disable Christmas mode completely
            Game1.ChristmasMode = false;

            // Reset Mario to small form at start of level
            state.ForceSmall(spawnPoint);

            // Reset camera
            camera.Reset(spawnPoint);
            camera.LookAt(spawnPoint);

            // Reset snail
            enemyManager.ResetSnail(spawnPoint);

            // Reset music to normal
            SoundManager.Instance.StopSong();
        }

        // =========================================================
        // FULL LEVEL RESET (FLAGPOLE / GAME OVER / TIME UP)
        // =========================================================
        public static void FullLevelReset(Game1 game){
            game.HardReload();
        }

        private static void ResetCamera(ICamera camera, Vector2 spawnPoint)
        {
            if (camera == null) return;

            camera.Reset(spawnPoint);
            camera.LookAt(spawnPoint);
        }
    }
}
