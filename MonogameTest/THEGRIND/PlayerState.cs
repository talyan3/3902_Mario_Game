using Microsoft.Xna.Framework;

public abstract class PlayerState
{
    //gotta change the name soon of marophysicstest
    protected PlayerMario player;

    public PlayerState(PlayerMario player)
    {
        this.player = player;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update(GameTime gameTime) { }
}