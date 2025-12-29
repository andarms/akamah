using Akamah.Engine.Engine.Core;
using Akamah.Engine.Systems.Collision;
using Akamah.Engine.UserInterface;

namespace Akamah.Engine.Gameplay.Inventories;

public class Backdrop() : Component
{
  Color color = Fade(Color.Gray, 0.5f);

  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Vector2.Zero, Canvas.Size, color);
  }
}

public class Window(Vector2 size) : Component
{
  Vector2 size = size;

  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Owner.Position, size, Color.White);
  }
}

public class InventoryPanel : GameObject
{
  Vector2 size = new(512, 300);

  public InventoryPanel()
  {
    Collider = new Collider() { Size = size, };

    Add(new Backdrop());
    Add(new Window(size));
  }
}