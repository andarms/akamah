using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Inventories;


public class InventorySlot : GameObject
{
  private const int SlotSize = 48;

  public int Index { get; set; } = -1;

  public InventorySlot() : base()
  {
    Collider = new() { Size = new Vector2(SlotSize, SlotSize) };
  }

  public override void Update(float deltaTime)
  {
    HandleMouseClick();
  }

  public override void Draw()
  {
    base.Draw();
    ItemStack stack = Game.Inventory.Items[Index];
    if (stack.IsEmpty) return;

    string texture = stack.Item.IconAssetPath;
    if (AssetsManager.Textures.TryGetValue(texture, out var itemTexture))
    {
      Rectangle destination = new(
        GlobalPosition.X,
        GlobalPosition.Y,
        stack.Item.IconSourceRect.Width * Setting.ZOOM_LEVEL,
        stack.Item.IconSourceRect.Height * Setting.ZOOM_LEVEL
      );
      DrawTexturePro(
        itemTexture,
        stack.Item.IconSourceRect,
        destination,
        Vector2.Zero,
        0.0f,
        Color.White
      );

      if (stack.Quantity > 1)
      {
        string quantityText = stack.Quantity.ToString();
        Vector2 textSize = MeasureTextEx(AssetsManager.DefaultFont, quantityText, 16, 1);
        Vector2 textPosition = new(
          GlobalPosition.X + SlotSize - textSize.X - 4,
          GlobalPosition.Y + SlotSize - textSize.Y - 4
        );
        DrawTextEx(AssetsManager.DefaultFont, quantityText, textPosition, 16, 1, Color.White);
      }
    }
  }

  private bool IsMouseOver()
  {
    Vector2 mousePosition = GetMousePosition();
    Rectangle slotRect = new(
      GlobalPosition.X,
      GlobalPosition.Y,
      SlotSize,
      SlotSize
    );
    return CheckCollisionPointRec(mousePosition, slotRect);
  }

  private void HandleMouseClick()
  {
    if (IsMouseOver() && IsMouseButtonPressed(MouseButton.Left))
    {
      ItemStack stack = Game.Inventory.Items[Index];
      if (stack.IsEmpty) return;
      Game.AddUI(new FloatingInventorySlot(stack)
      {
        Position = GetMousePosition()
      });
    }
  }
}


