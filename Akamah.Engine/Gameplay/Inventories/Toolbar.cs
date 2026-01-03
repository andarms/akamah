using Akamah.Engine.Engine.Core;
using Akamah.Engine.Systems.Collision;

namespace Akamah.Engine.Gameplay.Inventories;

public class Toolbar : GameObject
{
  const int padding = 4;
  const int slotSize = 48;
  const int slotCount = 6;
  public Toolbar()
  {
    int width = slotCount * slotSize + padding * slotCount + padding;
    Collider = new Collider()
    {
      Size = new Vector2(width, 56),
    };
    for (int i = 0; i < slotCount; i++)
    {
      var slot = new InventorySlot()
      {
        Index = i,
        Position = new Vector2(
          Position.X + padding + (slotSize + padding) * i,
          Position.Y + padding
        )
      };
      Add(slot);
    }
  }

  public override void Draw()
  {
    if (Collider == null) return;
    DrawRectangleV(Position, Collider.Size, Color.DarkGray);
    base.Draw();
  }
}