using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;
using Akamah.Engine.UserInterface;
using Akamah.Engine.World.Actors.Player;

namespace Akamah.Engine.Gameplay.Inventories;

public class InventoryWindow : GameObject
{
  private const int SlotSize = 64;
  private const int SlotPadding = 4;
  private const int HeaderHeight = 48;
  private const int Columns = 6;
  private const int SlotTotalSize = SlotSize + SlotPadding;
  private const int WindowPadding = 8;
  private const int BorderThickness = 6;
  private const int TitleFontSize = 32;

  private Vector2 size;
  private int rows;

  public Inventory? Inventory { get; set; }

  public InventoryWindow() : base()
  {
    Collider = new() { Size = Vector2.Zero };
  }

  public override void Initialize()
  {
    base.Initialize();

    Inventory = Game.Player?.Inventory;
    if (Inventory == null) return;

    CalculateWindowSize();
    SetupCollider();
    CenterWindow();
    CreateSlotUIs();
  }

  private void CalculateWindowSize()
  {
    if (Inventory == null) return;

    rows = Math.Max(1, (Inventory.Items.Count + Columns - 1) / Columns); // Ceiling division

    // Calculate content area size
    float contentWidth = Columns * SlotTotalSize;
    float contentHeight = rows * SlotTotalSize + HeaderHeight;

    // Add padding and border thickness
    size = new Vector2(
      contentWidth + 2 * WindowPadding + 2 * BorderThickness,
      contentHeight + 2 * WindowPadding + 2 * BorderThickness
    );
  }

  private void SetupCollider()
  {
    if (Collider != null)
    {
      Collider.Size = size;
    }
  }

  private void CenterWindow()
  {
    Position = Canvas.CalculatePosition(this, UserInterface.Anchor.Center, Vector2.Zero);
  }

  private void CreateSlotUIs()
  {
    if (Inventory?.Items == null) return;

    int count = 0;
    foreach (var slot in Inventory.Items)
    {
      var slotUI = new InventorySlotUI(slot)
      {
        Position = CalculateSlotPosition(slot),
        IsToolbarSlot = count < Inventory.ToolbarSize
      };
      Add(slotUI);
      count++;
    }
  }

  private Vector2 CalculateSlotPosition(InventorySlot slot)
  {
    if (Inventory?.Items == null) return Vector2.Zero;

    int index = Inventory.Items.IndexOf(slot);
    if (index == -1) return Vector2.Zero;

    int column = index % Columns;
    int row = index / Columns;

    float x = Position.X + BorderThickness + WindowPadding + column * SlotTotalSize;
    float y = Position.Y + BorderThickness + HeaderHeight + WindowPadding + row * SlotTotalSize;

    return new Vector2(x, y);
  }

  public override void Draw()
  {
    // Draw window background
    DrawRectangleV(Position, size, Color.White);

    // Draw window border
    var windowRect = new Rectangle(Position.X, Position.Y, size.X, size.Y);
    DrawRectangleLinesEx(windowRect, BorderThickness, Color.DarkGray);
    DrawHeader();

    // Draw child elements (slots)
    base.Draw();
  }

  private void DrawHeader()
  {

    var headerRect = new Rectangle(Position.X, Position.Y, size.X, HeaderHeight);
    DrawRectangleRec(headerRect, Color.DarkGray);


    const string title = "Inventory";
    int titleWidth = MeasureText(title, TitleFontSize);
    int titleX = (int)(Position.X + (size.X - titleWidth) / 2);
    int titleY = (int)(Position.Y + (HeaderHeight - TitleFontSize) / 2);

    DrawTextEx(AssetsManager.DefaultFont, title, new Vector2(titleX, titleY), TitleFontSize, 1, Color.White);
  }
}
