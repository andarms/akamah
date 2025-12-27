using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Inventories.Items;

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

  public override void Use(GameObject user)
  {
    // Wood logs are not directly usable
    Console.WriteLine($"{user} tried to use {Name}, but it's not usable.");
  }
}
