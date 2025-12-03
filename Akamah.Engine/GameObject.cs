namespace Akamah.Engine;

public class GameObject
{
  public Vector2 Position { get; set; } = Vector2.Zero;

  public Collider? Collider { get; set; }

  public virtual void Initialize()
  {
  }

  public virtual void Update(float deltaTime)
  {
  }

  public virtual void Draw()
  {
  }
}
