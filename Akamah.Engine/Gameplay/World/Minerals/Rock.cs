using Akamah.Engine.Assets.Management;
using Akamah.Engine.Core.Engine;
using Akamah.Engine.Gameplay.Materials;
using Akamah.Engine.Systems.Collision;

namespace Akamah.Engine.Gameplay.World.Minerals;

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
    Add(new Health(50));
    Add(new Stone());
    Add(new RemoveOnDeath());
  }


  public override void Draw()
  {
    base.Draw();
    DrawTexturePro(
      AssetsManager.Textures["Desert"],
      new Rectangle(64, 48, 16, 16),
      new Rectangle(RenderPosition.X, RenderPosition.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
  }
}
