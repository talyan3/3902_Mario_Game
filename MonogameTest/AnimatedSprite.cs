using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace MonogameTest;

public abstract class AnimatedSprite : ISprite
{
    protected int _currentFrame;
    protected TimeSpan _elapsed;
    protected Animation _animation;
    public TextureRegion Region { get; set; }
    public Animation Animation
    {
        get => _animation;
        set
        {
            _animation = value;
            Region = _animation.Frames[0];
        }
    }

    public abstract void Draw(SpriteBatch spriteBatch, Vector2 position);

    public abstract void Update(GameTime gameTime);
}