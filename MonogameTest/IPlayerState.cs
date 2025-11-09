
using Microsoft.Xna.Framework;

public interface IPlayerState
{
    void Enter(MarioPhysiscsTest player);
    void Update(MarioPhysiscsTest player, GameTime gameTime);
    void Exit(MarioPhysiscsTest player);
}