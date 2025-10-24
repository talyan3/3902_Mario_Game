using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace MonogameTest;

public abstract class AnimatedSprite : ISprite
{
    // Adding protected variables specific to AnimatedSprite to be used in implementing classes
    protected int _currentFrame;
    protected TimeSpan _elapsed;
    protected Animation _animation;
    public TextureRegion Region { get; set; }
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Scale { get; set; } = Vector2.One;
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

    public TextureRegion currentFrame()
    {
        return _animation.Frames[_currentFrame];
    }
}