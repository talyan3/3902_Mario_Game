using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonogameTest;

public class SmallMarioSprite : StaticSprite
{
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Scale { get; set; } = new Vector2(3f, 3f);

    public float MoveSpeed = 100f; // px/sec
    private SpriteEffects _effects = SpriteEffects.None;

    private const int FrameW = 16;
    private const int FrameH = 16;

    private List<TextureRegion> _runFrames = new();
    private TextureRegion _idleFrame;
    private TextureRegion _jumpFrame;
    private TextureRegion _current;

    private int _frameIndex = 0;
    private float _frameTimer = 0f;
    private float _frameTime = 0.12f;

    private bool _isJumping = false;
    private float _jumpOffset = 100f; // how far up he moves

    private Vector2 _groundPos;

    public SmallMarioSprite(GraphicsDevice graphicsDevice)
    {
        Texture2D texture = Texture2D.FromFile(graphicsDevice, "small-mario.png");

        _runFrames.Add(new TextureRegion(texture, 30 * 3, 0, FrameW, FrameH));
        _runFrames.Add(new TextureRegion(texture, 30 * 4, 0, FrameW, FrameH));
        _runFrames.Add(new TextureRegion(texture, 30 * 5, 0, FrameW, FrameH));

        _idleFrame = new TextureRegion(texture, 30 * 6, 0, FrameW, FrameH);
        _jumpFrame = new TextureRegion(texture, 30 * 2, 0, FrameW, FrameH); 

        _current = _idleFrame;
        Region = _current;
    }

    public override void Update(GameTime gameTime)
    {
        var kb = Keyboard.GetState();
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_groundPos == Vector2.Zero)
            _groundPos = Position; 

        if (kb.IsKeyDown(Keys.Left))
        {
            Position = new Vector2(Position.X - MoveSpeed * dt, Position.Y);
            _effects = SpriteEffects.None;
            AdvanceRun(dt);
        }
        else if (kb.IsKeyDown(Keys.Right))
        {
            Position = new Vector2(Position.X + MoveSpeed * dt, Position.Y);
            _effects = SpriteEffects.FlipHorizontally;
            AdvanceRun(dt);
        }
        else if (kb.IsKeyDown(Keys.Up))
        {
            if (!_isJumping)
            {
                _isJumping = true;
                Position = new Vector2(Position.X, Position.Y - _jumpOffset);
            }
            _current = _jumpFrame;
        }
        else
        {
            if (_isJumping)
            {
               
                Position = new Vector2(Position.X, _groundPos.Y);
                _isJumping = false;
            }
            _current = _idleFrame;
            _frameIndex = 0;
            _frameTimer = 0f;
        }
    }

    private void AdvanceRun(float dt)
    {
        _frameTimer += dt;
        if (_frameTimer >= _frameTime)
        {
            _frameTimer -= _frameTime;
            _frameIndex = (_frameIndex + 1) % _runFrames.Count;
        }
        _current = _runFrames[_frameIndex];
        Region = _current;
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 _)
    {
        if (_current == null) return;

        var origin = new Vector2(_current.Width / 2f, _current.Height);
        _current.Draw(spriteBatch, Position, Color.White, 0f, origin, Scale, _effects, 0f);
    }
}