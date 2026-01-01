using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;
using Akamah.Engine.Gameplay.Combat;
using Akamah.Engine.Gameplay.Inventories;
using Akamah.Engine.Gameplay.Inventories.Items;
using Akamah.Engine.Systems.Collision;
using Akamah.Engine.World.Materials;

namespace Akamah.Engine.World.Environment.Flora;

public class Tree : GameObject
{
  public Tree()
  {
    Anchor = new(8, 16);
    Collider = new Collider
    {
      Size = new Vector2(16, 8),
      Offset = new Vector2(0, 8),
      Solid = true
    };
    Add(new Wooden());
    Add(new Health(30));
    Add(new ShowDamageOnHit());
    Add(new DropLootOnDeath(Loot()));
    Add(
      new Sprite()
      {
        TexturePath = "TinyTown",
        SourceRect = new Rectangle(64, 32, 16, 16)
      }
    );
    Add(new RemoveOnDeath());
  }

  public static LootTable Loot()
  {
    return new LootTable()
      .Add(new(
        Create: () => new Collectable(new WoodLog()),
        MinAmount: 1,
        MaxAmount: 1,
        Chance: 1.0f
      ));
  }
}
