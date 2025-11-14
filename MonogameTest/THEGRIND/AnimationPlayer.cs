using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class AnimationPlayer
{
    public Animation CurrentAnimation { get; private set; }
    public int CurrentFrame { get; private set; }
    private float _timer;

    public void Play(Animation animation)
    {
        if (CurrentAnimation == animation)
            return;

        CurrentAnimation = animation;
        CurrentFrame = 0;
        _timer = 0f;
    }

    public void Update(GameTime gameTime)
    {
        if (CurrentAnimation == null)
            return;

        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_timer >= CurrentAnimation.FSpeed)//F stands for frame
        {
            _timer -= CurrentAnimation.FSpeed;
            CurrentFrame++;

            if (CurrentFrame >= CurrentAnimation.FCount)
                CurrentFrame = 0;
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects, float scale)
    {
        var source = CurrentAnimation.GetFrameRect(CurrentFrame);
        spriteBatch.Draw(CurrentAnimation.Texture, position, source, Color.White, 
            0f, Vector2.Zero, 1f, effects, 0f);
    }
}
