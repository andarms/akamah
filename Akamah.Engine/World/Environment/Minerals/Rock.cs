using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;
using Akamah.Engine.Gameplay.Combat;
using Akamah.Engine.Gameplay.Inventories;
using Akamah.Engine.Systems.Collision;
using Akamah.Engine.World.Materials;

namespace Akamah.Engine.World.Environment.Minerals;

public class Rock : GameObject
{
  public Rock()
  {
    Collider = new Collider
    {
      Size = new Vector2(16, 16),
      Offset = new Vector2(0, 0),
      Solid = true
    };
    Add(new Stone());
    Add(new Health(50));
    Add(new ShowDamageOnHit());
    Add(new DropLootOnDeath(LootTable()));
    Add(new Sprite() { TexturePath = "Desert", SourceRect = new Rectangle(64, 48, 16, 16) });
    Add(new RemoveOnDeath());
  }

  public static LootTable LootTable()
  {
    return new LootTable()
      .Add(new(
        Create: () => new Collectable(new Gameplay.Inventories.Items.Stone()),
        MinAmount: 1,
        MaxAmount: 1,
        Chance: 1.0f
      ));
  }
}
