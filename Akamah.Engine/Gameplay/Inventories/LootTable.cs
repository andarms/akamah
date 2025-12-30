using Akamah.Engine.Engine.Core;
using Akamah.Engine.World;

namespace Akamah.Engine.Gameplay.Inventories;

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
      var a = Game.Rng.RollFloat();
      if (a > entry.Chance) continue;

      int amount = Game.Rng.RollInt(entry.MinAmount, entry.MaxAmount);
      for (int i = 0; i < amount; i++)
      {
        yield return entry.Create();
      }
    }
  }
}
