using Akamah.Engine.Assets.Management;
using Akamah.Engine.Core.Engine;
using Akamah.Engine.Gameplay.Materials;
using Akamah.Engine.Gameplay.UI;
using Akamah.Engine.Systems.Collision;

namespace Akamah.Engine.Gameplay.World.Flora;

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
