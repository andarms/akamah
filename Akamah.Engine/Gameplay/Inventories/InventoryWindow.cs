using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Inventories;

public class InventoryWindow(Vector2 size, int columns = 8, int rows = 4) : GameObject
{
  Vector2 size = size;
  readonly int slotSize = 48;
  readonly int slotPadding = 4;
  readonly int headerHeight = 48;
  readonly int columns = columns;
  readonly int rows = rows;

  public Inventory? Inventory { get; set; }
  public bool ShowHeader { get; set; } = true;

  public int SlotSize => slotSize;
  public int SlotPadding => slotPadding;
  public int HeaderHeight => headerHeight;
  public int Columns => columns;
  public int Rows => rows;

  public override void Draw()
  {
    base.Draw();

    // Draw background
    DrawRectangleV(Position, size, Color.LightGray);
    DrawRectangleLinesEx(new Rectangle(Position.X, Position.Y, size.X, size.Y), 2, Color.DarkGray);

    // Draw header if enabled
    if (ShowHeader)
    {
      DrawHeader();
    }
  }

  private void DrawHeader()
  {
    Rectangle headerRect = new(Position.X, Position.Y, size.X, headerHeight);
    DrawRectangleRec(headerRect, Color.DarkGray);

    string title = "Player Inventory";
    int titleWidth = MeasureText(title, 32);
    int titleX = (int)(Position.X + (size.X - titleWidth) / 2);
    int titleY = (int)(Position.Y + (headerHeight - 32) / 2);

    DrawTextEx(AssetsManager.DefaultFont, title, new Vector2(titleX, titleY), 32, 1, Color.White);
    // Show inventory statistics
    if (Inventory != null)
    {
      int totalSlots = Inventory.Items.Count;
      int usedSlots = Inventory.Items.Count(slot => !slot.IsEmpty());

      string stats = $"{usedSlots}/{totalSlots}";
      DrawTextEx(AssetsManager.DefaultFont, stats, new Vector2(Position.X + 8, titleY), 16, 1, Color.LightGray);
    }
  }

  public Vector2 GetSlotPosition(int slotIndex)
  {
    int row = slotIndex / columns;
    int col = slotIndex % columns;

    float gridStartY = Position.Y + (ShowHeader ? headerHeight : 0) + slotPadding;

    return new Vector2(
      Position.X + slotPadding + col * (slotSize + slotPadding),
      gridStartY + row * (slotSize + slotPadding)
    );
  }

  public Vector2 GetGridSize()
  {
    return new Vector2(
      columns * (slotSize + slotPadding) + slotPadding,
      rows * (slotSize + slotPadding) + slotPadding + (ShowHeader ? headerHeight : 0)
    );
  }
}
