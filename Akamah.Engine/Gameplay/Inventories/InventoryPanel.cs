using Akamah.Engine.Engine.Core;
using Akamah.Engine.Systems.Collision;
using Akamah.Engine.World;

namespace Akamah.Engine.Gameplay.Inventories;

public class InventoryPanel : GameObject
{
  Vector2 size = new(512, 300);
  InventoryWindow inventoryWindow;
  private readonly List<InventoryWindowSlot> slotObjects = new();

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
    if (Game.Player.TryGet<Inventory>(out var playerInventory))
    {
      inventoryWindow.Inventory = playerInventory;
      UpdateSlots();
    }
  }

  public override void Update(float deltaTime)
  {
    base.Update(deltaTime);

    // Ensure inventory connection is maintained
    if (inventoryWindow.Inventory == null)
    {
      if (Game.Player.TryGet<Inventory>(out var playerInventory))
      {
        inventoryWindow.Inventory = playerInventory;
        UpdateSlots();
      }
    }
    else
    {
      // Update slot positions based on window position
      UpdateSlotPositions();
    }
  }

  private void UpdateSlots()
  {
    // Clear existing slot objects
    foreach (var slotObj in slotObjects)
    {
      RemoveChild(slotObj);
    }
    slotObjects.Clear();

    if (inventoryWindow.Inventory == null) return;

    // Create slot objects for each inventory slot
    int totalSlots = Math.Min(inventoryWindow.Rows * inventoryWindow.Columns, inventoryWindow.Inventory.Items.Count);

    for (int i = 0; i < totalSlots; i++)
    {
      var slotObject = new InventoryWindowSlot(inventoryWindow.Inventory.Items[i])
      {
        Position = inventoryWindow.GetSlotPosition(i)
      };

      slotObjects.Add(slotObject);
      AddChild(slotObject);
    }
  }

  private void UpdateSlotPositions()
  {
    for (int i = 0; i < slotObjects.Count; i++)
    {
      slotObjects[i].Position = inventoryWindow.GetSlotPosition(i);
    }
  }
}