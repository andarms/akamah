using Akamah.Engine.Core.Camera;
using Akamah.Engine.Systems.Collision;

namespace Akamah.Engine.Core.Engine;

public class GameObject
{
  public Vector2 Position { get; set; } = Vector2.Zero;
  public Collider? Collider { get; set; }
  public bool Visible { get; set; } = true;
  public int Layer { get; set; } = 0;
  public Vector2 Anchor { get; set; } = Vector2.Zero;

  public bool FlipX { get; set; } = false;

  public Vector2 RenderPosition => Position - Anchor;

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


  public virtual void Terminate()
  {
  }


  public virtual void Debug()
  {
    Collider?.Debug(Position, Anchor);
    DrawCircleV(Position, 2, Color.Lime);
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


  public virtual Rectangle GetBounds()
  {
    if (Collider == null) return new Rectangle(Position.X, Position.Y, 0, 0);
    return Collider.GetBounds(Position, Anchor);
  }
}
