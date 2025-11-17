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

    private float _smoothSpeed;
    private float _levelWidth;
    private float _levelHeight;
    private float _furthestRight = 0f;
    private float _horizontalOffsetRatio;
    private readonly NumberStructure _numbers;

    public CameraManager(Viewport viewport, NumberStructure numbers)
    {
        _viewport = viewport;
        _numbers = numbers;

        Zoom = _viewport.Width / numbers.CameraManager.NesViewWidth;  // keep NES view width of 256px

        _smoothSpeed = numbers.CameraManager.SmoothSpeed;
        _horizontalOffsetRatio = numbers.CameraManager.HorizontalOffsetRatio;
    }

    public Matrix GetViewMatrix()
    {
        return Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0f)) *
               Matrix.CreateScale(Zoom, Zoom, 1f) *
               Matrix.CreateTranslation(new Vector3(_viewport.Width / 2f, _viewport.Height / 2f, 0f));
    }

    public void LookAt(Vector2 target)
    {
        float desiredX = target.X - (_viewport.Width / (Zoom * _numbers.CameraManager.ZoomDivisor));
        _position = Vector2.Lerp(_position, new Vector2(desiredX, 0), _smoothSpeed);
        _position.X = MathHelper.Clamp(_position.X, _numbers.CameraManager.LeftClamp, _levelWidth - (_viewport.Width / Zoom));
        _position.Y = _numbers.CameraManager.FixedCameraY;

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