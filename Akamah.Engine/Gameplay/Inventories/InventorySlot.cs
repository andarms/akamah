using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Inventories;

public class InventorySlot()
{
  public Item? Item { get; set; } = null;
  public int Quantity { get; set; } = 0;

  public bool IsEmpty() => Item == null || Quantity <= 0;

  public bool CanAddItem(Item item)
  {
    if (IsEmpty()) return true;
    if (Item?.GetType() != item.GetType()) return false;
    return Quantity < Item!.MaxStackSize;
  }

  public void AddItem(Item item, int quantity)
  {
    if (!CanAddItem(item)) throw new InvalidOperationException("Cannot add item to this slot.");

    if (IsEmpty()) { Item = item; }
    Quantity += quantity;
    if (Quantity > item.MaxStackSize)
    {
      Quantity = item.MaxStackSize;
    }
  }
}



