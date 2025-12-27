using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Inventories;

public enum ItemCategory
{
  None,
  Tool,
  Weapon,
  Armor,
  Consumable,
  Material,
  Placeable
}

public abstract class Item
{
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public ItemCategory Category { get; set; } = ItemCategory.None;
  public int MaxStackSize { get; set; } = 1;
  public Rectangle IconSourceRect { get; set; } = new Rectangle(0, 0, 0, 0);
  public string IconAssetPath { get; set; } = string.Empty;
  public abstract void Use(GameObject user);
}