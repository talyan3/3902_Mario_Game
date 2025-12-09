using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonogameTest.Sounds;
using MonogameTest.Screens;
using MonogameTest.Managers;

namespace MonogameTest.Managers
{
    public class CollisionManager
    {
        //other collision handlers
        private readonly TileCollisionHandler _tileHandler;
        private readonly EnemyCollisionHandlerWrapper _enemyHandler;
        private readonly PowerupCollisionHandler _powerupHandler;
        private readonly Flagpole _flagpole;  

        public CollisionManager(
            TileCollisionHandler tileHandler,
            EnemyCollisionHandlerWrapper enemyHandler,
            PowerupCollisionHandler powerupHandler,
            Flagpole flagpole)
        {
            _tileHandler = tileHandler;
            _enemyHandler = enemyHandler;
            _powerupHandler = powerupHandler;
            _flagpole = flagpole;
        }      
        

        // =========================================================
        // MAIN UPDATE
        // =========================================================
        public void Update(GameTime gameTime, StaticSprite activeMario, CameraManager camera)
        {

            if (Game1.InputLocked)
            {
                flagpole.Update(gameTime);  // allow victory sequence
                return;
            }

            _tileHandler.HandleTileCollision(activeMario);
            _enemyHandler.HandleEnemy(activeMario, camera);
            _powerupHandler.HandlePowerups(activeMario, gameTime);


            _flagpole.Update(gameTime);
        }


    }
}
