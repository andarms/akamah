using Akamah.Engine.Core;
using Akamah.Engine.Assets;
using Akamah.Engine.Collisions;

namespace Akamah.Engine.Entities;

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
