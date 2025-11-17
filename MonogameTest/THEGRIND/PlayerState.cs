using Microsoft.Xna.Framework;
namespace MonogameTest;
public abstract class PlayerState
{
    protected PlayerMario player;

    public PlayerState(PlayerMario player)
    {
        this.player = player;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update(GameTime gameTime) { }
}