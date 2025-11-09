// using Microsoft.Xna.Framework;

// public class PlayerIdleState : IPlayerState
// {
//     public void Enter(MarioPhysiscsTest player)
//     {
//         player.SetAnimation("Idle");
//     }

//     public void Update(MarioPhysiscsTest player, MarioPhysiscsTest gameTime)
//     {
//         if (player.Input.MoveX != 0)
//         {
//             player.ChangeState(new PlayerRunningState());
//         }
//     }

//     public void Exit(MarioPhysiscsTest player) { }
// }

// public class PlayerRunningState : IPlayerState
// {
//     public void Enter(MarioPhysiscsTest player)
//     {
//         player.SetAnimation("Run");
//     }

//     public void Update(MarioPhysiscsTest player, GameTime gameTime)
//     {
//         if (player.Input.MoveX == 0)
//         {
//             player.ChangeState(new PlayerIdleState());
//         }

//         player.Position.X += player.Input.MoveX * player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
//     }

//     public void Exit(MarioPhysiscsTest player) { }
// }