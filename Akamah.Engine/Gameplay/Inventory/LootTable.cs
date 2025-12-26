using Akamah.Engine.Engine.Core;
using Akamah.Engine.World;

namespace Akamah.Engine.Gameplay.Inventory;

public record LootEntry(
  Func<GameObject> Create,
  int MinAmount,
  int MaxAmount,
  float Chance
);


public class LootTable
{
  readonly List<LootEntry> entries = [];

  public LootTable Add(LootEntry entry)
  {
    entries.Add(entry);
    return this;
  }

  public IEnumerable<GameObject> Roll()
  {
    foreach (var entry in entries)
    {
      var a = GameWorld.Rng.RollFloat();
      Console.WriteLine($"Loot roll: {a} vs Chance: {entry.Chance}");
      if (a > entry.Chance) continue;

      int amount = GameWorld.Rng.RollInt(entry.MinAmount, entry.MaxAmount);
      for (int i = 0; i < amount; i++)
      {
        yield return entry.Create();
      }
    }
  }
}
