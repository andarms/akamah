using Akamah.Engine.Engine.Core;
using Akamah.Engine.Gameplay.Combat;
using Akamah.Engine.World;

namespace Akamah.Engine.Gameplay.Inventories;

public class DropLootOnDeath(LootTable loot) : Component
{
  private readonly LootTable loot = loot;

  public override void Initialize() => Owner.When<HealthDepleted>(_ => DropLoot());

  private void DropLoot()
  {
    foreach (var item in loot.Roll())
    {
      GameWorld.Spawn(item, Owner.Position, 10);
    }
  }
}
