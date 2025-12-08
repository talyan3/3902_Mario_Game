using System;
using Microsoft.Xna.Framework;
namespace MonogameTest;
public class JumpState : PlayerState
{
    public JumpState(PlayerMario player) : base(player) { }

    public override void Enter()
    {
        player.animPlayer.Play(player.animJump);
        System.Console.WriteLine("Entered Jump State");
        //player.SM.JumpSfx.Play();
        player.Physics.velocity.Y = -player.PhysicsP.jumpStrength;
    }

    public override void Update(GameTime gameTime)
    {
        // When grounded, transition based on horizontal speed
        if (player.PhysicsP.isGrounded)
        {
            if (Math.Abs(player.Physics.velocity.X) > 0.1f)
                player.ChangeState(new RunState(player));
            else
                player.ChangeState(new IdleState(player));
        }
    }
}