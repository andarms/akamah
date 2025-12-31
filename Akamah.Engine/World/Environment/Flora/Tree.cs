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
    AddChild(new Wooden());
    AddChild(new Health(30));
    AddChild(new RemoveOnDeath());
    AddChild(new ShowDamageOnHit());
    AddChild(new DropLootOnDeath(Loot()));
    AddChild(
      new Sprite()
      {
        TexturePath = "TinyTown",
        SourceRect = new Rectangle(64, 32, 16, 16)
      }
    );
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
