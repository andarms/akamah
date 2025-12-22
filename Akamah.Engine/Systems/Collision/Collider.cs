using Akamah.Engine.Core.Engine;

namespace Akamah.Engine.Systems.Collision;

public class Collider
{
  public Vector2 Size { get; set; } = new Vector2(1, 1);
  public Vector2 Offset { get; set; } = Vector2.Zero;
  public bool Solid { get; set; } = false;

  // Hold a list of GameObjects this collider is currently colliding with
  public List<GameObject> Collisions { get; } = [];

  public Color DebugColor { get; set; } = Fade(Color.Red, 0.5f);

  public virtual void Debug(Vector2 position, Vector2 anchor)
  {
    DrawRectangleV(position + Offset - anchor, Size, DebugColor);
  }

  public virtual Rectangle GetBounds(Vector2 position, Vector2 anchor)
  {
    return new Rectangle(
      position.X + Offset.X - anchor.X,
      position.Y + Offset.Y - anchor.Y,
      Size.X,
      Size.Y
    );
  }
}


public class CircleCollider : Collider
{
  public float Radius { get; set; } = 0.5f;


  public override void Debug(Vector2 position, Vector2 anchor)
  {
    DrawRectangleLinesEx(GetBounds(position, anchor), 1, DebugColor);
    DrawCircleV(position + Offset - anchor, Radius, DebugColor);
  }

  public override Rectangle GetBounds(Vector2 position, Vector2 anchor)
  {
    return new Rectangle(position.X + Offset.X - anchor.X - Radius, position.Y + Offset.Y - anchor.Y - Radius, Radius * 2, Radius * 2);
  }
}