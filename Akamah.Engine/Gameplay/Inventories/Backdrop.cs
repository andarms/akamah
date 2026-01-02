using Akamah.Engine.Engine.Core;
using Akamah.Engine.UserInterface;

namespace Akamah.Engine.Gameplay.Inventories;

public class Backdrop() : GameObject
{
  Color color = Fade(Color.Black, 0.5f);

  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Vector2.Zero, Canvas.Size, color);
  }
}
