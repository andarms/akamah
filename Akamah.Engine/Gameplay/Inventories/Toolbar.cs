using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Inventories;

public class Toolbar : GameObject
{
  public override void Draw()
  {
    base.Draw();
    DrawRectangle(Setting.SCREEN_WIDTH / 2 - 150, Setting.SCREEN_HEIGHT - 60, 300, 50, Color.DarkGray);
  }
}