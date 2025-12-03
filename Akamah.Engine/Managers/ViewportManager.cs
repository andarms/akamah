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


  public static void UpdateTarget(Vector2 target)
  {
    Vector2 screenCenter = GetScreenCenter();
    Vector2 cameraLimit = new(
      GameManager.Map.Limits.X - screenCenter.X + camera.Offset.X / camera.Zoom,
      GameManager.Map.Limits.Y - screenCenter.Y + camera.Offset.Y / camera.Zoom
    );
    camera.Target = Vector2.Clamp(target, Vector2.Zero + camera.Offset / camera.Zoom, cameraLimit);
  }

}