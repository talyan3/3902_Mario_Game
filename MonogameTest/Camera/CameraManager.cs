using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest
{
    public class CameraManager : ICamera
{
    private readonly Viewport _viewport;
    private Vector2 _position = Vector2.Zero;
    public float Zoom { get; set; };

    private float _smoothSpeed;
    private float _levelWidth;
    private float _levelHeight;
    private float _tileSize;
    private float _furthestRight = 0f;
    private float _horizontalOffsetRatio;

    //Magic Numbers
    private readonly CameraMan _set = Game1.Numbers.CameraMan;

    public float TileSize
        {
            set
            {
                _tileSize = value;
                _levelWidth = TiledMapLoader.MapWidth * _tileSize;
                _levelHeight = TiledMapLoader.MapHeight * _tileSize;
            }
        }

    public CameraManager(Viewport viewport)
    {
        _viewport = viewport;
        Zoom = _viewport.Width / 256f;  // keep NES view width of 256px
    }

    public Matrix GetViewMatrix()
    {
        return Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0f)) *
               Matrix.CreateScale(Zoom, Zoom, 1f) *
               Matrix.CreateTranslation(new Vector3(_viewport.Width / 2f, _viewport.Height / 2f, 0f));
    }

    public void LookAt(Vector2 target)
    {
        float desiredX = target.X - (_viewport.Width / (Zoom * 100f));
        _position = Vector2.Lerp(_position, new Vector2(desiredX, 0), _smoothSpeed);
        _position.X = MathHelper.Clamp(_position.X, 128, _levelWidth - (_viewport.Width / Zoom));
        
        //float visibleWorldHeight = _viewport.Height / Zoom;
        _position.Y = 120;

        if (_position.X > _furthestRight)
            _furthestRight = _position.X;
        else
            _position.X = _furthestRight;
    }

    public void Reset(Vector2 startPosition)
    {
        _position = startPosition;
        _furthestRight = startPosition.X;
    }
}

}