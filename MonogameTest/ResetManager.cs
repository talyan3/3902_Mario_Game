using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameTest.Screens;

namespace MonogameTest
{
    /// <summary>
    /// Handles soft resets and death-trigger resets for Mario.
    /// Keeps logic centralized and easy to modify.
    /// </summary>
    public static class ResetManager
    {
        // === Soft Reset (R Key) ===
        // Used for debugging: returns Mario to spawn, resets camera, and restarts music.
        public static void ResetOnKeyPress(
            SmallMarioSprite smallMario,
            BigMarioSprite bigMario,
            StaticSprite currentMario,
            Vector2 spawnPoint,
            ICamera camera)
        {
            // Reset Mario's position (for all possible forms)
            if (currentMario != null)
                currentMario.Position = spawnPoint;

            if (smallMario != null)
                smallMario.Position = spawnPoint;

            if (bigMario != null)
                bigMario.Position = spawnPoint;

            // Reset camera back to spawn
            if (camera != null)
            {
                camera.Reset(spawnPoint);
                camera.LookAt(spawnPoint);
            }

            // Restart overworld theme
            SoundManager.Instance.StopSong();
            SoundManager.Instance.PlaySong("mainTheme");
        }


        // === Death Trigger (Called before the death sequence starts) ===
        // NES logic: Mario always dies as Small Mario.
        // This does *not* fully reset the level — Game1 handles that
        // after the death animation finishes.
        internal static void ResetOnDeath(
            ref StaticSprite currentMario,
            SmallMarioSprite smallMario)
        {
            // Ensure Mario is Small when dying
            if (smallMario != null)
                currentMario = smallMario;

            // Play death jingle
            SoundManager.Instance.StopSong();
            SoundManager.Instance.PlaySong("youreDead", loop: false);

            // Full level reset is handled later in:
            // - Game1.StartDeathSequence()
            // - Game1.UpdateDeathSequence()
            // - Game1.ResetLevel()
        }
    }
}