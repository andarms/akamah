using Akamah.Engine.Engine.Core;
using Akamah.Engine.Systems.Collision;
using Akamah.Engine.World;

namespace Akamah.Engine.Gameplay.Inventories;

public class InventoryPanel : GameObject
{
  Vector2 size = new(512, 300);
  InventoryWindow inventoryWindow;

  public InventoryPanel()
  {
    Collider = new Collider() { Size = size, };

    Add(new Backdrop());

    // Create inventory window and connect it to player inventory
    inventoryWindow = new InventoryWindow(size, 8, 4);
    Add(inventoryWindow);
  }

  public override void Initialize()
  {
    base.Initialize();

    // Connect to player inventory after initialization
    if (GameWorld.Player.TryGet<Inventory>(out var playerInventory))
    {
      inventoryWindow.Inventory = playerInventory;
    }
  }

  public override void Update(float deltaTime)
  {
    base.Update(deltaTime);

    // Ensure inventory connection is maintained
    if (inventoryWindow.Inventory == null)
    {
      if (GameWorld.Player.TryGet<Inventory>(out var playerInventory))
      {
        inventoryWindow.Inventory = playerInventory;
      }
    }
  }
}