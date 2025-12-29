using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Inventories;

public class InventoryWindow(Vector2 size, int columns = 8, int rows = 4) : Component
{
  Vector2 size = size;
  readonly int slotSize = 48;
  readonly int slotPadding = 4;
  readonly int headerHeight = 24;
  readonly int columns = columns;
  readonly int rows = rows;

  public Inventory? Inventory { get; set; }
  public bool ShowHeader { get; set; } = true;

  public override void Draw()
  {
    base.Draw();

    // Draw background
    DrawRectangleV(Owner.Position, size, Color.LightGray);
    DrawRectangleLinesEx(new Rectangle(Owner.Position.X, Owner.Position.Y, size.X, size.Y), 2, Color.DarkGray);

    // Draw header if enabled
    if (ShowHeader)
    {
      DrawHeader();
    }

    // Draw inventory grid
    DrawInventoryGrid();
  }

  private void DrawHeader()
  {
    Rectangle headerRect = new(Owner.Position.X, Owner.Position.Y, size.X, headerHeight);
    DrawRectangleRec(headerRect, Color.DarkGray);

    string title = "Player Inventory";
    int titleWidth = MeasureText(title, 16);
    int titleX = (int)(Owner.Position.X + (size.X - titleWidth) / 2);
    int titleY = (int)(Owner.Position.Y + (headerHeight - 16) / 2);

    DrawText(title, titleX, titleY, 16, Color.White);

    // Show inventory statistics
    if (Inventory != null)
    {
      int totalSlots = Inventory.Items.Count;
      int usedSlots = Inventory.Items.Count(slot => !slot.IsEmpty());

      string stats = $"{usedSlots}/{totalSlots}";
      DrawText(stats, (int)Owner.Position.X + 8, titleY, 14, Color.LightGray);
    }
  }

  private void DrawInventoryGrid()
  {
    float gridStartY = Owner.Position.Y + (ShowHeader ? headerHeight : 0) + slotPadding;

    for (int row = 0; row < rows; row++)
    {
      for (int col = 0; col < columns; col++)
      {
        int slotIndex = row * columns + col;
        Vector2 slotPosition = new(
          Owner.Position.X + slotPadding + col * (slotSize + slotPadding),
          gridStartY + row * (slotSize + slotPadding)
        );

        Rectangle slotRect = new(slotPosition.X, slotPosition.Y, slotSize, slotSize);

        // Draw slot background
        DrawRectangleRec(slotRect, Color.White);
        DrawRectangleLinesEx(slotRect, 2, Color.Black);

        // Draw item if exists
        if (Inventory != null)
        {
          DrawItemInSlot(slotRect, slotIndex);
        }
      }
    }
  }

  private void DrawItemInSlot(Rectangle slotRect, int slotIndex)
  {
    if (Inventory?.Items == null)
      return;

    // Show all items directly from the inventory
    if (slotIndex < Inventory.Items.Count && !Inventory.Items[slotIndex].IsEmpty())
    {
      var slot = Inventory.Items[slotIndex];
      var item = slot.Item!;

      // Draw item background with category-specific color
      Color itemColor = GetCategoryColor(item.Category);
      DrawRectangleRec(slotRect, itemColor);
      DrawRectangleLinesEx(slotRect, 2, Color.DarkBlue);

      // Try to draw item icon if available
      if (!string.IsNullOrEmpty(item.IconAssetPath) && item.IconSourceRect.Width > 0)
      {
        // TODO: Load and draw actual texture when asset system is ready
        // For now, draw a placeholder with item initial
        string initial = item.Name.Length > 0 ? item.Name[0].ToString().ToUpper() : "?";
        int fontSize = 24;
        int textWidth = MeasureText(initial, fontSize);
        int textX = (int)(slotRect.X + (slotSize - textWidth) / 2);
        int textY = (int)(slotRect.Y + (slotSize - fontSize) / 2);
        DrawText(initial, textX, textY, fontSize, Color.White);
      }
      else
      {
        // Draw item name (abbreviated and cleaned up)
        string itemName = !string.IsNullOrEmpty(item.Name) ? item.Name : CleanItemName(item.GetType().Name);
        if (itemName.Length > 6)
          itemName = itemName.Substring(0, 6);

        DrawText(itemName, (int)slotRect.X + 2, (int)slotRect.Y + 2, 9, Color.Black);
      }

      // Draw quantity in bottom right
      if (slot.Quantity > 1)
      {
        string quantityText = slot.Quantity.ToString();
        int textWidth = MeasureText(quantityText, 10);
        DrawRectangle(
          (int)slotRect.X + slotSize - textWidth - 4,
          (int)slotRect.Y + slotSize - 14,
          textWidth + 2,
          12,
          Color.Black
        );
        DrawText(quantityText, (int)slotRect.X + slotSize - textWidth - 3, (int)slotRect.Y + slotSize - 13, 10, Color.White);
      }
    }
  }

  private Color GetCategoryColor(ItemCategory category)
  {
    return category switch
    {
      ItemCategory.Tool => Color.Orange,
      ItemCategory.Weapon => Color.Red,
      ItemCategory.Armor => Color.Blue,
      ItemCategory.Material => Color.Brown,
      ItemCategory.Consumable => Color.Green,
      ItemCategory.Placeable => Color.Purple,
      ItemCategory.None => Color.Gray,
      _ => Color.SkyBlue
    };
  }

  private static string CleanItemName(string typeName)
  {
    // Remove common suffixes and clean up the name
    return typeName
      .Replace("Item", "")
      .Replace("Tool", "")
      .Replace("Weapon", "")
      .Trim();
  }

  public Vector2 GetGridSize()
  {
    return new Vector2(
      columns * (slotSize + slotPadding) + slotPadding,
      rows * (slotSize + slotPadding) + slotPadding + (ShowHeader ? headerHeight : 0)
    );
  }
}
