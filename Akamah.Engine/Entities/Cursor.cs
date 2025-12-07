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
    Collider = new Collider
    {
      Size = new Vector2(4),
      Offset = new Vector2(0)
    };
  }


  public override void Update(float dt)
  {
    var screenPosition = GetMousePosition();
    var worldMousePosition = GetScreenToWorld2D(screenPosition, ViewportManager.Camera);

    // Calculate direction from player to real mouse cursor
    var directionToMouse = worldMousePosition - GameManager.Player.Position;

    // Limit the cursor distance (adjust maxDistance as needed)


    // If mouse is too far, limit cursor to maxDistance
    var normalizedDirection = Vector2.Normalize(directionToMouse);
    Position = GameManager.Player.Position - offset + normalizedDirection * maxDistance;


    base.Update(dt);
  }


  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(4), Colliding ? collidingColor : normalColor);
  }
}
