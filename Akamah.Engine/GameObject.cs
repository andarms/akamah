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
    if (!IsInCameraView())
    {
      Visible = false;
      return;
    }
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
