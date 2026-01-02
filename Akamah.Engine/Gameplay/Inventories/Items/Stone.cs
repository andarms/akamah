using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Inventories.Items;

public class Stone : Item
{
  public Stone()
  {
    Name = "Stone";
    Description = "A small piece of stone, useful for crafting and building.";
    MaxStackSize = 50;
    IconAssetPath = "Desert";
    IconSourceRect = new Rectangle(192, 192, 16, 16);
    Category = ItemCategory.Material;
  }

  public override GameAction OnUse()
  {
    return new DoNothing();
  }
}
