using Akamah.Engine.Engine.Core;
using Akamah.Engine.Gameplay.Combat;
using Akamah.Engine.World;

namespace Akamah.Engine.Gameplay.Inventories;

public class DropLootOnDeath(LootTable loot) : GameObject
{
  private readonly LootTable loot = loot;

  public override void Initialize()
  {
    When<HealthDepleted>(_ => DropLoot());
  }

  private void DropLoot()
  {
    foreach (var item in loot.Roll())
    {
      item.Position = GlobalPosition;
      Game.Add(item);
    }
  }
}
