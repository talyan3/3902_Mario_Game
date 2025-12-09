using Microsoft.Xna.Framework;
using MonogameTest.Managers;
using MonogameTest.Screens;
using MonogameTest.Sounds;

namespace MonogameTest.Managers
{
    public class PowerupCollisionHandlerWrapper
    {
        private readonly PowerupFieldManager _powerupManager;
        private readonly MarioStateController _marioState;
        private readonly ScreenManager _screenManager;
        private readonly SoundManager _sound;
        private readonly System.Action<int> _addScore;
        private readonly System.Action _addCoin;

        public PowerupCollisionHandlerWrapper(
            PowerupFieldManager powerupManager,
            MarioStateController marioState,
            ScreenManager screenManager,
            SoundManager sound,
            System.Action<int> addScore,
            System.Action addCoin)
        {
            _powerupManager = powerupManager;
            _marioState = marioState;
            _screenManager = screenManager;
            _sound = sound;
            _addScore = addScore;
            _addCoin = addCoin;
        }

        private void HandlePowerups(StaticSprite activeMario, GameTime gameTime)
        {
            var pickedUp = powerupManager.Update(gameTime, activeMario);
            if (pickedUp == null)
                return;

            pickedUp.IsAlive = false;

            switch (pickedUp.Type)
            {
                case PowerupType.Mushroom:
                    marioState.Grow();
                    addScore?.Invoke(200);
                    break;

                case PowerupType.Coin:
                    addCoin?.Invoke();
                    addScore?.Invoke(100);
                    break;

                case PowerupType.GreenMushroom:
                    sound.PlayEffect("oneUp");
                    screenManager.AddScore(200);
                    screenManager.ChangeState(screenManager.CurrentState);
                    break;

                case PowerupType.Star:
                    sound.PlayEffect("powerUp");
                    addScore?.Invoke(500);
                    break;

                case PowerupType.FireFlower:
                    sound.PlayEffect("powerUp");
                    addScore?.Invoke(300);
                    break;
            }
        }
    }
}