using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;
using Akamah.Engine.UserInterface;

namespace Akamah.Engine.Gameplay.Inventories;


public class InventoryWindowSlotComponent(InventorySlot slot) : Component
{
  const int SLOT_SIZE = 48;
  readonly InventorySlot slot = slot;
  bool highlight = false;

  public override void Update(float deltaTime)
  {
    base.Update(deltaTime);
    Vector2 cursor = GetMousePosition();
    Rectangle slotRect = new(Owner.Position.X, Owner.Position.Y, SLOT_SIZE, SLOT_SIZE);
    if (CheckCollisionPointRec(cursor, slotRect))
    {
      highlight = true;
    }
    else
    {
      highlight = false;
    }
  }

  public override void Draw()
  {
    base.Draw();

    Rectangle slotRect = new(Owner.Position.X, Owner.Position.Y, SLOT_SIZE, SLOT_SIZE);

    // Draw slot background
    DrawRectangleRec(slotRect, Color.White);
    if (highlight)
    {
      DrawRectangleLinesEx(slotRect, 2, Color.Blue);
    }
    else
    {
      DrawRectangleLinesEx(slotRect, 2, Color.Black);

    }

    // Draw item if slot is not empty
    if (!slot.IsEmpty() && slot.Item != null)
    {
      DrawItemInSlot(slotRect);
    }
  }

  private void DrawItemInSlot(Rectangle slotRect)
  {
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
      int fontSize = 32;
      int textWidth = MeasureText(initial, fontSize);
      float textX = (slotRect.X + (SLOT_SIZE - textWidth) / 2);
      float textY = (slotRect.Y + (SLOT_SIZE - fontSize) / 2);
      DrawTextEx(AssetsManager.DefaultFont, initial, new Vector2(textX, textY), fontSize, 1, Color.White);
    }
    else
    {
      // Draw item name (abbreviated and cleaned up)
      string itemName = !string.IsNullOrEmpty(item.Name) ? item.Name : CleanItemName(item.GetType().Name);
      if (itemName.Length > 6) { itemName = itemName.Substring(0, 6); }
      DrawTextEx(AssetsManager.DefaultFont, itemName, new Vector2(slotRect.X + 2, slotRect.Y + 2), 16, 1, Color.Black);
    }

    // Draw quantity in bottom right
    if (slot.Quantity > 1)
    {
      string quantityText = slot.Quantity.ToString();
      int textWidth = MeasureText(quantityText, 10);
      DrawRectangle(
        (int)slotRect.X + SLOT_SIZE - textWidth - 4,
        (int)slotRect.Y + SLOT_SIZE - 14,
        textWidth + 2,
        12,
        Color.Black
      );
      DrawTextEx(AssetsManager.DefaultFont, quantityText, new Vector2(slotRect.X + SLOT_SIZE - textWidth - 3, slotRect.Y + SLOT_SIZE - 13), 16, 1, Color.White);
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
}

public class InventoryWindowSlot : UIObject
{
  public InventoryWindowSlot(InventorySlot slot)
  {
    Add(new InventoryWindowSlotComponent(slot));
  }
}