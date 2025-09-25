using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public class AnimatedMovingMarioSprite : AnimatedSprite
{
    // See StaticMarioSprite for in-depth comments
    public Color Color { get; set; } = Color.White;
    public float Rotation { get; set; } = 0.0f;
    public Vector2 Scale { get; set; } = new Vector2(5,5);
    public Vector2 Origin { get; set; } = Vector2.Zero;
    SpriteEffects Effects { get; set; } = SpriteEffects.None;
    public float LayerDepth { get; set; } = 0.0f;
    public float Width => Region.Width * Scale.X;
    public float Height => Region.Height * Scale.Y;

    // Variables establishing position on screen
    public float xPos { get; set; } = 0;
    public float yPos { get; set; } = 0;

    public AnimatedMovingMarioSprite(TextureRegion region, Animation animation)
    {
        Region = region;
        Animation = animation;
    }


    // TODO: Check if this needs to be called every frame of animation
    public void CenterOrigin()
    {
        Origin = new Vector2(Region.Width, Region.Height) * 0.5f;
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        Region.Draw(spriteBatch, position, Color, Rotation, Origin, Scale, Effects, LayerDepth);
    }

    public override void Update(GameTime gameTime)
    {
        _elapsed += gameTime.ElapsedGameTime;

        if (_elapsed >= _animation.Delay)
        {
            _elapsed -= _animation.Delay;
            _currentFrame++;

            if (_currentFrame >= _animation.Frames.Count)
            {
                _currentFrame = 0;
            }

            Region = _animation.Frames[_currentFrame];
        }

        Origin = new Vector2(++xPos, 0);
    }
}