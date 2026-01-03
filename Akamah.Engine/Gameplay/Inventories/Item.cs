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
  public bool Stackable { get; set; } = true;
  public Rectangle IconSourceRect { get; set; } = new Rectangle(0, 0, 0, 0);
  public string IconAssetPath { get; set; } = string.Empty;
  public abstract GameAction OnUse();
}