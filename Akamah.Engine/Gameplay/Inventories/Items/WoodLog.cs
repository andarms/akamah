using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Inventories.Items;

public record DoNothing() : GameAction;

public class WoodLog : Item
{
  public WoodLog()
  {
    Name = "Wood Log";
    Description = "A sturdy log of wood, useful for crafting and building.";
    MaxStackSize = 20;
    IconAssetPath = "TinyTown";
    IconSourceRect = new Rectangle(160, 128, 16, 16);
    Category = ItemCategory.Material;
  }

  public override GameAction OnUse()
  {
    return new DoNothing();
  }
}
