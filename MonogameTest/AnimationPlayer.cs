using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class AnimationPlayer
{
    private Animation _animation;
    private float _time;
    private int _frameIndex;

    public void Play(Animation animation)
    {
        if (_animation == animation)
        {
            return;
        }
        _animation = animation;
        _frameIndex = 0;
        _time = 0f;
    }

    public void Update(GameTime gameTime)
    {
        if (_animation == null) {
            return;
            }

        _time += (float)gameTime.ElapsedGameTime.TotalSeconds;

        while (_time > _animation.FrameTime)
        {
            _time -= _animation.FrameTime;

            if (_animation.IsLooping)
            {
                _frameIndex = (_frameIndex + 1) % _animation.FrameCount;
            }
            else
            {
                _frameIndex = Math.Min(_frameIndex + 1, _animation.FrameCount - 1);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None)
    {
        if (_animation == null) return;

        int frameWidth = _animation.FrameWidth;
        int frameHeight = _animation.FrameHeight;

        Rectangle source = new Rectangle(_frameIndex * frameWidth, 0, frameWidth, frameHeight);

        spriteBatch.Draw(_animation.Texture, position, source, Color.White, 0f, Vector2.Zero, 1f, effects, 0f);
    }
}
