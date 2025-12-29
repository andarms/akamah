using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Inventories;

public record AddToInventory(Item Item, int Quantity) : GameAction;

public class Inventory : Component, IHandle<AddToInventory>
{
  public List<InventorySlot> Items { get; private set; } = [];

  public Inventory(int size)
  {
    for (int i = 0; i < size; i++)
    {
      Items.Add(new InventorySlot());
    }
  }

  public bool AddItem(Item item, int quantity)
  {
    foreach (var slot in Items)
    {
      if (slot.CanAddItem(item))
      {
        slot.AddItem(item, quantity);
        return true;
      }
    }
    return false; // Inventory full or no suitable slot found
  }

  public void Handle(AddToInventory action)
  {
    AddItem(action.Item, action.Quantity);
  }
}