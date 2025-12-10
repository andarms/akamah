using Akamah.Engine.Core;

namespace Akamah.Engine.Collisions;

public class Collider
{
  public Vector2 Size { get; set; } = new Vector2(1, 1);
  public Vector2 Offset { get; set; } = Vector2.Zero;
  public bool Solid { get; set; } = false;

  // Hold a list of GameObjects this collider is currently colliding with
  public List<GameObject> Collisions { get; } = [];

  public Color DebugColor { get; set; } = Fade(Color.Red, 0.5f);

  public virtual void Debug(Vector2 position)
  {
    DrawRectangleV(position + Offset, Size, DebugColor);
  }

  public virtual Rectangle GetBounds(Vector2 position)
  {
    return new Rectangle(position.X + Offset.X, position.Y + Offset.Y, Size.X, Size.Y);
  }
}


public class CircleCollider : Collider
{
  public float Radius { get; set; } = 0.5f;


  public override void Debug(Vector2 position)
  {
    DrawRectangleLinesEx(new Rectangle(position.X + Offset.X - Radius, position.Y + Offset.Y - Radius, Radius * 2, Radius * 2), 1, DebugColor);
    DrawCircleV(position + Offset, Radius, DebugColor);
  }

  public override Rectangle GetBounds(Vector2 position)
  {
    return new Rectangle(position.X + Offset.X - Radius, position.Y + Offset.Y - Radius, Radius * 2, Radius * 2);
  }
}