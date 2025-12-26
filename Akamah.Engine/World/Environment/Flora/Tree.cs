using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;
using Akamah.Engine.Gameplay.Combat;
using Akamah.Engine.Gameplay.Inventory;
using Akamah.Engine.Systems.Collision;
using Akamah.Engine.World.Materials;

namespace Akamah.Engine.World.Environment.Flora;

public class WoodLog : GameObject;
public class TreeSeed : GameObject;

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
    Add(new RemoveOnDeath());
    Add(new ShowDamageOnHit());
    Add(new DropLootOnDeath(Loot()));
  }



  public LootTable Loot()
  {
    return new LootTable()
      .Add(new LootEntry(
        Create: () =>
        {
          WoodLog log = new() { Position = Position };
          GameWorld.AddGameObject(log);
          Console.WriteLine("Dropping a wood log at " + Position);
          return log;
        },
        MinAmount: 2,
        MaxAmount: 5,
        Chance: 1.0f
      ))
      .Add(new LootEntry(
        Create: () =>
        {
          TreeSeed seed = new() { Position = Position };
          Console.WriteLine("Dropping a tree seed at " + Position);
          GameWorld.AddGameObject(seed);
          return seed;
        },
        MinAmount: 1,
        MaxAmount: 1,
        Chance: 0.1f
      ));
  }

  public override void Draw()
  {
    DrawTexturePro(
      AssetsManager.Textures["TinyTown"],
      new Rectangle(64, 32, 16, 16),
      new Rectangle(RenderPosition.X, RenderPosition.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
  }
}
