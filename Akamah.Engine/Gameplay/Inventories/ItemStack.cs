using Akamah.Engine.Engine.Core;
using Akamah.Engine.Gameplay.Inventories.Items;

namespace Akamah.Engine.Gameplay.Inventories;

public class EmptyItem : Item
{
  public override GameAction OnUse()
  {
    return new DoNothing();
  }
}

public sealed class ItemStack(Item item, int quantity)
{
  public const int MaxStackSize = 16;

  public Item Item { get; private set; } = item;
  public int Quantity { get; private set; } = quantity;
  public bool IsEmpty => Quantity <= 0;

  public void Add(Item item)
  {
    Item = item;
    Quantity = 1;
  }

  public void Add(int amount)
  {
    Quantity += amount;
    if (Quantity > MaxStackSize)
    {
      Quantity = MaxStackSize;
    }
  }

  public bool CanAdd(Item item, int amount)
  {
    return Item.GetType() == item.GetType() && Quantity + amount <= MaxStackSize;
  }
}