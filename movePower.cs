using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest;

public class movePower : ISprite
{
    private SpriteBatch _spriteBatch;
    private Texture2D _texture;

    private Rectangle _sourceRect;
    private Rectangle _destRect;
    private float _elapsed;
    private float _delay = 150f;
    private int _frames;

    public Vector2 Velocity {get; set;} = new Vector2(1f,0);
    public bool IsAlive {get; set;} = true;

    public movePower(Texture2D texture, SpriteBatch spriteBatch)
    {
        _texture = texture;
        _spriteBatch = spriteBatch;

        if (_destRect.Width == 0 || _destRect.Height == 0)
            _destRect = new Rectangle(0, 0, 16, 16);
        if (_sourceRect.Width == 0 || _sourceRect.Height == 0)
            _sourceRect = new Rectangle(0, 0, 16, 16);
    }

    public Vector2 Position
    {
        get => new Vector2(_destRect.X, _destRect.Y);
        set => _destRect = new Rectangle((int)value.X, (int)value.Y, _destRect.Width == 0 ? 16 : _destRect.Width, _destRect.Height == 0 ? 16 : _destRect.Height);
    }

    public Rectangle Bounds => IsAlive ? _destRect : Rectangle.Empty;
    public Rectangle Region => _sourceRect;
    public Vector2 Scale => Vector2.One;

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        if (!IsAlive) return;
        _spriteBatch.Draw(_texture, _destRect, _sourceRect, Color.White);
    }

    public void Update(GameTime gameTime)
    {
        if (!IsAlive) return;
        _elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        if (_elapsed >= _delay)
        {
            //animation
            if (_frames >= 1)
            {
                _frames = 0;
            }
            else
            {
                _frames++;
            }
            _elapsed = 0;
        }

        //movement
        _destRect.X += (int)Velocity.X;
        _destRect.Y += (int)Velocity.Y;
    }
}