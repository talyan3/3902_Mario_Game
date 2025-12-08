using Microsoft.Xna.Framework.Input;
using MonogameTest.Sounds;

namespace MonogameTest.Managers
{
    public static class PauseManager
    {
        private static bool _pHeldLast = false;
        private static bool _isPaused = false;

        public static bool IsPaused => _isPaused;

        /// <summary>
        /// Returns true if the game should be frozen this frame.
        /// </summary>
        public static bool HandlePauseInput(KeyboardState state)
        {
            bool pDown = state.IsKeyDown(Keys.P);

            if (pDown && !_pHeldLast)
            {
                _isPaused = !_isPaused;

                SoundManager.Instance.PlayEffect("pause");

                if (_isPaused)
                    SoundManager.Instance.PauseSong();
                else
                    SoundManager.Instance.ResumeSong();
            }

            _pHeldLast = pDown;

            // === FREEZE GAMEPLAY WHEN PAUSED ===
            return _isPaused;
        }
    }
}
