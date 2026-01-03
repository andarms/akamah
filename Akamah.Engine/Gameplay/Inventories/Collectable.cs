using Akamah.Engine.Engine.Core;
using Akamah.Engine.Systems.Collision;
using Akamah.Engine.World;
using Akamah.Engine.World.Actors.Player;

namespace Akamah.Engine.Gameplay.Inventories;

public class Collectable : GameObject
{
  public Item Item { get; private set; }

  public Collectable(Item item)
  {
    Item = item;
    Anchor = new(8, 8);
    Collider = new Collider
    {
      Size = new Vector2(16, 16),
      Offset = new Vector2(0, 0),
      Solid = false
    };
  }


  public override void Update(float dt)
  {
    base.Update(dt);
    var collision = CollisionsManager.GetPotentialCollisions(this);
    foreach (var other in collision)
    {
      if (other is not Player) { continue; }

      Game.Inventory.Trigger(new AddToInventory(Item, 1));
      Game.Remove(this);
      break;
    }
  }

  public override void Draw()
  {
    if (Item.IconAssetPath != string.Empty)
    {
      if (!Assets.AssetsManager.Textures.TryGetValue(Item.IconAssetPath, out var texture))
      {
        return;
      }
      DrawTextureRec(texture, Item.IconSourceRect, Position - Anchor, Color.White);
    }
  }
}