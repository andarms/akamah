using Akamah.Engine.Engine.Core;
using Akamah.Engine.World;

namespace Akamah.Engine.Engine.Camera;

public interface IViewport
{
  virtual Camera2D Camera => new();
  virtual Vector2 GetScreenCenter() => new();
  void UpdateTarget(Vector2 target);
  bool IsPointInView(Vector2 point, float margin);
  bool IsRectInView(Vector2 position, Vector2 size);
  (int minX, int minY, int maxX, int maxY) GetVisibleTileRange(int tileSize);
  (Vector2 topLeft, Vector2 bottomRight) Area { get; }
}

public class Viewport : IViewport
{

  private Camera2D camera = new()
  {
    Target = new Vector2(0, 0),
    Offset = new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2),
    Rotation = 0.0f,
    Zoom = Setting.ZOOM_LEVEL
  };

  public Camera2D Camera => camera;

  public Vector2 GetScreenCenter() => new(GetScreenWidth() / 2, GetScreenHeight() / 2);

  // cached viewport for the current frame
  private Vector2 cachedTopLeft;
  private Vector2 cachedBottomRight;


  public void Initialize()
  {
    camera = new Camera2D
    {
      Target = camera.Target,
      Offset = GetScreenCenter(),
      Rotation = camera.Rotation,
      Zoom = camera.Zoom
    };
  }


  public void Update()
  {
    Vector2 screenCenter = GetScreenCenter();
    cachedTopLeft = camera.Target - screenCenter / camera.Zoom - new Vector2(32);
    cachedBottomRight = camera.Target + screenCenter / camera.Zoom + new Vector2(32);

  }

  public (Vector2 topLeft, Vector2 bottomRight) Area => (cachedTopLeft, cachedBottomRight);

  public void UpdateTarget(Vector2 target)
  {
    Vector2 screenCenter = GetScreenCenter();
    Vector2 cameraLimit = new(
      Game.Map.Limits.X - screenCenter.X + camera.Offset.X / camera.Zoom,
      Game.Map.Limits.Y - screenCenter.Y + camera.Offset.Y / camera.Zoom
    );
    camera.Target = Vector2.Clamp(target, Vector2.Zero + camera.Offset / camera.Zoom, cameraLimit);
  }

  /// <summary>
  /// Checks if a point is within the camera viewport with optional margin
  /// </summary>
  public bool IsPointInView(Vector2 point, float margin = 0f)
  {
    return point.X >= cachedTopLeft.X - margin &&
           point.X <= cachedBottomRight.X + margin &&
           point.Y >= cachedTopLeft.Y - margin &&
           point.Y <= cachedBottomRight.Y + margin;
  }

  /// <summary>
  /// Checks if a rectangle is within the camera viewport
  /// </summary>
  public bool IsRectInView(Vector2 position, Vector2 size)
  {
    return !(position.X + size.X < cachedTopLeft.X ||
             position.X > cachedBottomRight.X ||
             position.Y + size.Y < cachedTopLeft.Y ||
             position.Y > cachedBottomRight.Y);
  }

  /// <summary>
  /// Gets the visible tile range for the current camera view
  /// </summary>
  public (int minX, int minY, int maxX, int maxY) GetVisibleTileRange(int tileSize)
  {
    int minX = Math.Max(0, (int)(cachedTopLeft.X / tileSize) - 1);
    int minY = Math.Max(0, (int)(cachedTopLeft.Y / tileSize) - 1);
    int maxX = Math.Min(Game.Map.Width - 1, (int)(cachedBottomRight.X / tileSize) + 1);
    int maxY = Math.Min(Game.Map.Height - 1, (int)(cachedBottomRight.Y / tileSize) + 1);

    return (minX, minY, maxX, maxY);
  }

}