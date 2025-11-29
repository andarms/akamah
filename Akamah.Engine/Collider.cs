namespace Akamah.Engine;

public class Collider
{
  public Vector2 Size { get; set; } = new Vector2(1, 1);
  public Vector2 Offset { get; set; } = Vector2.Zero;

  // Hold a list of GameObjects this collider is currently colliding with
  public List<GameObject> Collisions { get; } = [];
}
