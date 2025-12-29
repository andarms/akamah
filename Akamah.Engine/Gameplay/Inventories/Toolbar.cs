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
  }

  public override void Draw()
  {
    base.Draw();
    if (Collider == null) return;

    DrawRectangleV(Position, Collider.Size, Color.DarkGray);
    for (int i = 0; i < slotCount; i++)
    {
      DrawRectangleV(Position + new Vector2(padding + i * padding + i * slotSize, 4), new Vector2(slotSize, slotSize), Color.LightGray);
    }
  }
}