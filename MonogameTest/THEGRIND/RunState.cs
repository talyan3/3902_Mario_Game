using System;
using Microsoft.Xna.Framework;
namespace MonogameTest;
public class RunState : PlayerState
{
    public RunState(PlayerMario player) : base(player) { }

    public override void Enter()
    {
        player.animPlayer.Play(player.animRun);
    }

    public override void Update(GameTime gameTime)
    {
        // No movement → Idle
        if (Math.Abs(player.PhysicsP.velocity.X) < 0.1f)
        {
            player.ChangeState(new IdleState(player));
            return;
        }

        // Jump → JumpState
        if (player.Input.jumpPressed)
        {
            player.ChangeState(new JumpState(player));
            return;
        }
    }

    public override void Exit()
    {
        //player.SM.WalkSfxInstance.Stop();
    }
}
