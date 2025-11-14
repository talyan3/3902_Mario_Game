using System;
using Microsoft.Xna.Framework;

public class IdleState : PlayerState
{
    public IdleState(PlayerMario player) : base(player) { }

    public override void Enter()
    {
        player.animPlayer.Play(player.animIdle);
        //player.SM.WalkSfxInstance?.Stop();
    }

    public override void Update(GameTime gameTime)
    {
        // Move → Run
        if (Math.Abs(player.PhysicsP.velocity.X) > 0.1f)
        {
            player.ChangeState(new RunState(player));
            return;
        }

        // Jump → JumpState
        if (player.Input.jumpPressed)
        {
            player.ChangeState(new JumpState(player));
            return;
        }
    }
}
