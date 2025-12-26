using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Inventories;

public class Inventory : Component
{
  public List<InventorySlot> Items { get; set; } = [];

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
}