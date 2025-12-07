using Akamah.Engine.Core;
using Akamah.Engine.Assets;
using Akamah.Engine.Collisions;

namespace Akamah.Engine.Entities;

public class Tree : GameObject
{

  public Tree()
  {
    Visible = true;
    Collider = new Collider
    {
      Size = new Vector2(16, 8),
      Offset = new Vector2(0, 8),
      Solid = true
    };
  }

  public override void Draw()
  {
    // Spatial system handles visibility culling for trees
    Visible = true;

    DrawTexturePro(
      AssetsManager.Textures["TinyTown"],
      new Rectangle(64, 32, 16, 16),
      new Rectangle(Position.X, Position.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
  }
}
