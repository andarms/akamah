using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Inventories;

public record AddToInventory(Item Item, int Quantity) : GameAction;

public class Inventory : GameObject
{
  public List<ItemStack> Items { get; private set; } = [];
  public List<InventorySlot> ToolbarItems { get; private set; } = [];

  public static int ToolbarSize => 6;

  private const int SlotSize = 64;
  private const int SlotPadding = 4;
  private const int HeaderHeight = 48;
  private const int Columns = 6;
  private const int SlotTotalSize = SlotSize + SlotPadding;
  private const int WindowPadding = 8;
  private const int BorderThickness = 6;

  private Inventory(int size)
  {
    Add(new Backdrop());
    for (int i = 0; i < size; i++)
    {
      var slot = new InventorySlot
      {
        Index = i,
        Position = CalculateSlotPosition(i)
      };
      Items.Add(new ItemStack(new EmptyItem(), 0));
      Add(slot);
    }
  }


  private Vector2 CalculateSlotPosition(int index)
  {
    if (index == -1) return Vector2.Zero;

    int column = index % Columns;
    int row = index / Columns;

    float x = Position.X + BorderThickness + WindowPadding + column * SlotTotalSize;
    float y = Position.Y + BorderThickness + HeaderHeight + WindowPadding + row * SlotTotalSize;
    return new Vector2(x, y);
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
    foreach (var stack in Items)
    {
      if (stack.IsEmpty)
      {
        stack.Add(action.Item);
        return true;
      }
      else if (stack.CanAdd(action.Item, action.Quantity))
      {
        stack.Add(action.Quantity);
        return true;
      }
    }
    return false; // Inventory full or no suitable slot found
  }
}