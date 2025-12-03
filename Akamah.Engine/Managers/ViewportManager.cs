
namespace Akamah.Engine.Managers;

public static class ViewportManager
{
  private const float DEFAULT_ZOOM = 3.0f;

  private static Camera2D camera = new()
  {
    Target = new Vector2(0, 0),
    Offset = new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2),
    Rotation = 0.0f,
    Zoom = DEFAULT_ZOOM
  };

  public static Camera2D Camera => camera;

  public static Vector2 GetScreenCenter() => new(GetScreenWidth() / 2, GetScreenHeight() / 2);

  // cached viewport for the current frame
  private static Vector2 cachedTopLeft;
  private static Vector2 cachedBottomRight;


  public static void Update()
  {
    Vector2 screenCenter = GetScreenCenter();
    cachedTopLeft = camera.Target - screenCenter / camera.Zoom - new Vector2(32);
    cachedBottomRight = camera.Target + screenCenter / camera.Zoom + new Vector2(32);
  }

  public static (Vector2 topLeft, Vector2 bottomRight) CameraViewport => (cachedTopLeft, cachedBottomRight);

  public static void UpdateTarget(Vector2 target)
  {
    Vector2 screenCenter = GetScreenCenter();
    Vector2 cameraLimit = new(
      GameManager.Map.Limits.X - screenCenter.X + camera.Offset.X / camera.Zoom,
      GameManager.Map.Limits.Y - screenCenter.Y + camera.Offset.Y / camera.Zoom
    );
    camera.Target = Vector2.Clamp(target, Vector2.Zero + camera.Offset / camera.Zoom, cameraLimit);
  }

  /// <summary>
  /// Checks if a point is within the camera viewport with optional margin
  /// </summary>
  public static bool IsPointInView(Vector2 point, float margin = 0f)
  {
    return point.X >= cachedTopLeft.X - margin &&
           point.X <= cachedBottomRight.X + margin &&
           point.Y >= cachedTopLeft.Y - margin &&
           point.Y <= cachedBottomRight.Y + margin;
  }

  /// <summary>
  /// Checks if a rectangle is within the camera viewport
  /// </summary>
  public static bool IsRectInView(Vector2 position, Vector2 size)
  {
    return !(position.X + size.X < cachedTopLeft.X ||
             position.X > cachedBottomRight.X ||
             position.Y + size.Y < cachedTopLeft.Y ||
             position.Y > cachedBottomRight.Y);
  }

  /// <summary>
  /// Gets the visible tile range for the current camera view
  /// </summary>
  public static (int minX, int minY, int maxX, int maxY) GetVisibleTileRange(int tileSize)
  {
    int minX = Math.Max(0, (int)(cachedTopLeft.X / tileSize) - 1);
    int minY = Math.Max(0, (int)(cachedTopLeft.Y / tileSize) - 1);
    int maxX = Math.Min(GameManager.Map.Width - 1, (int)(cachedBottomRight.X / tileSize) + 1);
    int maxY = Math.Min(GameManager.Map.Height - 1, (int)(cachedBottomRight.Y / tileSize) + 1);

    return (minX, minY, maxX, maxY);
  }

}