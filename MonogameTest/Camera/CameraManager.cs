using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest
{
    public class CameraManager : ICamera
{
    private readonly Viewport _viewport;
    private Vector2 _position = Vector2.Zero;
    public float Zoom { get; set; } = 1f;
    public float LeftEdge => _position.X - (_viewport.Width / 2f) / Zoom;

    private float _smoothSpeed = 0.15f;
    private float _levelWidth = TiledMapLoader.MapWidth * 16f;
    private float _levelHeight = TiledMapLoader.MapHeight * 16f;
    private float _furthestRight = 0f;
    private float _horizontalOffsetRatio = 0.35f;

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
        _position.X = MathHelper.Clamp(_position.X, 128, _levelWidth - (_viewport.Width / Zoom) + 256);
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