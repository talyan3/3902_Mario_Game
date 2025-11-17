using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace MonogameTest;

public class SmallMarioSprite : StaticSprite
{
    ContentManager _content;
    override public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Scale { get; set; } = new Vector2(1f, 1f);

    public float MoveSpeed = 100f; // movement speed

    private SpriteEffects _effects = SpriteEffects.None;

    private const int FrameW = 16;
    private const int FrameH = 16;

    private List<TextureRegion> _runFrames = new();
    private TextureRegion _idleFrame;
    private TextureRegion _jumpFrame;
    private TextureRegion _current;

    private int _frameIndex;
    private float _frameTimer;
    private float _frameTime = 0.12f;

    // physics
    private float verticalVelocity = 0f;
    private float gravity = 900f;
    private float jumpStrength = -350f;

    private bool _isJumping = false;
    private Vector2 _groundPos;

    private SoundEffect jumpSound;

    public Rectangle Bounds
    {
        get
        {
            if (Region == null) return Rectangle.Empty;
            int w = (int)(Region.Width * Scale.X);
            int h = (int)(Region.Height * Scale.Y);
            int left = (int)(Position.X - w / 2f);
            int top = (int)(Position.Y - h);
            return new Rectangle(left, top, w, h);
        }
    }
    public void Bounce(float pixels = 20f)                        
    {
        Position = new Vector2(Position.X, Position.Y - pixels);
    }


    public SmallMarioSprite(GraphicsDevice graphicsDevice, ContentManager _content)
    {
        Texture2D texture = _content.Load<Texture2D>("Sprites/Entity/small-mario-final");

        // RUN FRAMES
        _runFrames.Add(new TextureRegion(texture, 30 * 3, 0, FrameW, FrameH));
        _runFrames.Add(new TextureRegion(texture, 30 * 4, 0, FrameW, FrameH));
        _runFrames.Add(new TextureRegion(texture, 30 * 5, 0, FrameW, FrameH));

        // IDLE + JUMP FRAMES
        _idleFrame = new TextureRegion(texture, 30 * 6, 0, FrameW, FrameH);
        _jumpFrame = new TextureRegion(texture, 30 * 2, 0, FrameW, FrameH);

        _current = _idleFrame;
        Region = _current;

        jumpSound = _content.Load<SoundEffect>("smb_jump-super");
    }

    public override void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var kb = Keyboard.GetState();

        // Save ground position the first frame
        if (_groundPos == Vector2.Zero)
            _groundPos = Position;

        bool moving = false;

        // ---- LEFT / RIGHT MOVEMENT ----
        if (kb.IsKeyDown(Keys.Left))
        {
            Position = new Vector2(Position.X - MoveSpeed * dt, Position.Y);
            _effects = SpriteEffects.None;
            moving = true;
        }
        else if (kb.IsKeyDown(Keys.Right))
        {
            Position = new Vector2(Position.X + MoveSpeed * dt, Position.Y);
            _effects = SpriteEffects.FlipHorizontally;
            moving = true;
        }

        // ---- JUMP INPUT ----
        if (kb.IsKeyDown(Keys.Up) && !_isJumping)
        {
            _isJumping = true;
            verticalVelocity = jumpStrength;
            jumpSound.Play();
        }

        // ---- GRAVITY ----
        verticalVelocity += gravity * dt;
        Position = new Vector2(Position.X, Position.Y + verticalVelocity * dt);

        // ---- LANDING ----
        if (Position.Y >= _groundPos.Y)
        {
            Position = new Vector2(Position.X, _groundPos.Y);
            verticalVelocity = 0;
            _isJumping = false;
        }

        // ---- ANIMATIONS ----
        if (_isJumping)
        {
            _current = _jumpFrame;
        }
        else if (moving)
        {
            AdvanceRun(dt);
        }
        else
        {
            _current = _idleFrame;
            _frameIndex = 0;
            _frameTimer = 0f;
        }

        Region = _current;
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
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 _)
    {
        if (_current == null) return;

        var origin = new Vector2(_current.Width / 2f, _current.Height);
        _current.Draw(spriteBatch, Position, Color.White, 0f, origin, Scale, _effects, 0f);
    }
}
