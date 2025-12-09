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
    private float _zoomDivisor;
    private float _leftClamp;
    private float _fixedCameraY;
    private float _rightClampPadding;

    public CameraManager(Viewport viewport)
    {

        _viewport = viewport;

        //set and load JSON values
        var cam = NumberLoad.Numbers.CameraMan;
        _smoothSpeed = cam.SmoothSpeed;
        _horizontalOffsetRatio = cam.HorizontalOffsetRatio;
        _zoomDivisor = cam.ZoomDivisor;
        _leftClamp = cam.LeftClamp;
        _fixedCameraY = cam.FixedCameraY;
        _rightClampPadding = cam.RightClampPadding;

        //levels
        _levelWidth = TiledMapLoader.MapWidth * cam.TileSize;
        _levelHeight = TiledMapLoader.MapHeight * cam.TileSize;

        Zoom = _viewport.Width / cam.NesViewWidth;  // keep NES view width of 256px
    }

    public Matrix GetViewMatrix()
    {
        return Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0f)) *
               Matrix.CreateScale(Zoom, Zoom, 1f) *
               Matrix.CreateTranslation(new Vector3(_viewport.Width / 2f, _viewport.Height / 2f, 0f));
    }

    public void LookAt(Vector2 target)
    {
        float desiredX = target.X - (_viewport.Width / (Zoom * _zoomDivisor));
        _position = Vector2.Lerp(_position, new Vector2(desiredX, 0), _smoothSpeed);
        _position.X = MathHelper.Clamp(_position.X, _leftClamp, _levelWidth - (_viewport.Width / Zoom) + _rightClampPadding);
        _position.Y = _fixedCameraY;

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