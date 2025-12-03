using Akamah.Engine.Managers;

namespace Akamah.Engine;

public class GameObject
{
  public Vector2 Position { get; set; } = Vector2.Zero;

  public Collider? Collider { get; set; }

  public bool Visible { get; set; } = true;

  public virtual void Initialize()
  {
  }

  public virtual void Update(float deltaTime)
  {
  }

  public virtual void Draw()
  {
    // Spatial system now handles visibility culling more efficiently
    // Individual objects don't need to check visibility anymore
    // The spatial manager filters objects before calling Draw()
    Visible = true;
  }

  protected virtual bool IsInCameraView()
  {
    // Default implementation uses point-based visibility with small margin
    return ViewportManager.IsPointInView(Position, 32f);
  }

  protected bool IsInCameraView(Vector2 size)
  {
    return ViewportManager.IsRectInView(Position, size);
  }
}
