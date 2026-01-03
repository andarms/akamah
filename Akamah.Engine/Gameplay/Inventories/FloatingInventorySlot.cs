using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Inventories;

public class FloatingInventorySlot : GameObject
{
  public ItemStack Stack { get; }

  public FloatingInventorySlot(ItemStack stack)
  {
    Stack = stack;
    Add(new UISprite()
    {
      TexturePath = stack.Item?.IconAssetPath ?? string.Empty,
      SourceRect = stack.Item?.IconSourceRect ?? new Rectangle(0, 0, 0, 0),
    });
  }

  public override void Update(float deltaTime)
  {
    base.Update(deltaTime);
    Position = GetMousePosition();
  }
}



