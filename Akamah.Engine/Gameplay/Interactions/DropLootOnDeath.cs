using Akamah.Engine.Core.Engine;
using Akamah.Engine.Systems;

namespace Akamah.Engine.Gameplay.Interactions;

public class LootTable
{
  public IEnumerable<GameObject> Roll()
  {
    return [new GameObject()];
  }
}

public class DropLootOnDeath(LootTable loot) : Component
{
  private readonly LootTable loot = loot;

  public override void Initialize() => Owner.When<HealthDepleted>(_ => DropLoot());

  private void DropLoot()
  {
    foreach (var item in loot.Roll())
    {
      item.Position = Owner.Position;
      GameWorld.AddGameObject(item);
    }
  }
}
