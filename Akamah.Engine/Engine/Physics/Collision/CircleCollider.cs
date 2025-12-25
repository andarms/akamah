namespace Akamah.Engine.Systems.Collision;

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