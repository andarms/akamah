using Akamah.Engine.Engine.Core;
using Akamah.Engine.UserInterface;

namespace Akamah.Engine.Gameplay.Inventories;

public class InventoryUI : GameObject
{
  public InventoryUI()
  {
    Add(new Backdrop());
    InventoryWindow window = new();
    Add(window);
  }
}