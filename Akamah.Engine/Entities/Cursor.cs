using Akamah.Engine.Core;
using Akamah.Engine.Assets;
using Akamah.Engine.Collisions;
using Akamah.Engine.Systems;

namespace Akamah.Engine.Entities;

public class Cursor : GameObject
{
  public bool Colliding { get; set; } = false;
  Color normalColor = Color.Yellow;
  Color collidingColor = Color.Red;
  readonly float maxDistance = 16;
  Vector2 offset = new(0, 8);

  public Cursor()
  {
  }


  public override void Update(float dt)
  {
    var screenPosition = GetMousePosition();
    var worldMousePosition = GetScreenToWorld2D(screenPosition, ViewportManager.Camera);

    var direction = worldMousePosition - GameManager.Player.Position;
    var normalizedDirection = Vector2.Normalize(direction);
    Position = GameManager.Player.Position + normalizedDirection * maxDistance;

    base.Update(dt);
  }


  public override void Draw()
  {
    base.Draw();
    // DrawRectangleV(Position, new Vector2(4), Colliding ? collidingColor : normalColor);
  }
}
