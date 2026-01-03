using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Inventories;

public class InventorySlotUI : GameObject
{
  private const int SlotSize = 64;
  private const int BorderThickness = 2;

  private readonly InventorySlot slot;

  public bool IsToolbarSlot { get; set; } = false;


  public InventorySlotUI(InventorySlot slot) : base()
  {
    Collider = new() { Size = new Vector2(SlotSize, SlotSize) };
    this.slot = slot;
  }

  public override void Update(float deltaTime)
  {
    HandleMouseClick();
  }

  public override void Draw()
  {
    base.Draw();

    // Draw slot background
    Color bgColor = IsToolbarSlot ? Color.SkyBlue : Color.Gray;
    DrawRectangleV(GlobalPosition, new Vector2(SlotSize, SlotSize), bgColor);

    // Draw slot border
    Color color = IsMouseOver() ? Color.White : Color.DarkGray;
    var slotRect = new Rectangle(GlobalPosition.X, GlobalPosition.Y, SlotSize, SlotSize);
    DrawRectangleLinesEx(slotRect, BorderThickness, color);

    // Draw item name if slot is not empty
    if (!slot.IsEmpty() && slot.Item != null)
    {
      string texture = slot.Item.IconAssetPath;
      if (AssetsManager.Textures.TryGetValue(texture, out var itemTexture))
      {
        Rectangle destination = new(
          GlobalPosition.X,
          GlobalPosition.Y,
          slot.Item.IconSourceRect.Width * Setting.ZOOM_LEVEL,
          slot.Item.IconSourceRect.Height * Setting.ZOOM_LEVEL
        );
        DrawTexturePro(
          itemTexture,
          slot.Item.IconSourceRect,
          destination,
          Vector2.Zero,
          0.0f,
          Color.White
        );

        int Quantity = slot.Quantity;
        if (Quantity > 1)
        {
          string quantityText = Quantity.ToString();
          Vector2 textSize = MeasureTextEx(AssetsManager.DefaultFont, quantityText, 16, 1);
          Vector2 textPosition = new(
            GlobalPosition.X + SlotSize - textSize.X - 4,
            GlobalPosition.Y + SlotSize - textSize.Y - 4
          );
          DrawTextEx(AssetsManager.DefaultFont, quantityText, textPosition, 16, 1, Color.White);
        }
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
      if (slot.IsEmpty()) return;
      Game.AddUI(new FloatingInventorySlot(slot));
    }
  }
}



public class FloatingInventorySlot : GameObject
{
  public InventorySlot Slot { get; }

  public FloatingInventorySlot(InventorySlot slot)
  {
    Slot = slot;
    Add(new UISprite()
    {
      TexturePath = slot.Item?.IconAssetPath ?? string.Empty,
      SourceRect = slot.Item?.IconSourceRect ?? new Rectangle(0, 0, 0, 0),
    });
  }

  public override void Update(float deltaTime)
  {
    base.Update(deltaTime);
    Position = GetMousePosition();
  }
}