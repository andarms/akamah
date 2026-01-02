using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Inventories;

public record AddToInventory(Item Item, int Quantity) : GameAction;

public class Inventory : GameObject
{
  public List<InventorySlot> Items { get; private set; } = [];

  private Inventory(int size)
  {
    for (int i = 0; i < size; i++)
    {
      Items.Add(new InventorySlot());
    }
  }

  public static Inventory Small()
  {
    return new Inventory(24);
  }

  public override void Initialize()
  {
    base.Initialize();
    Handle<AddToInventory>(AddItem);
  }


  public bool AddItem(AddToInventory action)
  {
    foreach (var slot in Items)
    {
      if (slot.CanAddItem(action.Item))
      {
        slot.AddItem(action.Item, action.Quantity);
        return true;
      }
    }
    return false; // Inventory full or no suitable slot found
  }
}