using Akamah.Engine.Assets;
using Akamah.Engine.Collisions;
using Akamah.Engine.Core;
using Akamah.Engine.Systems;

namespace Akamah.Engine.Entities;

public class Weapon : GameObject
{
  float lifespan = 1;
  bool isActive = true;

  float rotation = 0f;

  public Weapon()
  {
    Anchor = new(8, 16);
    Collider = new Collider
    {
      Size = new Vector2(16),
      Offset = new Vector2(0)
    };
  }


  public bool Attack()
  {
    isActive = true;
    lifespan = 1;
    Visible = true;

    var mousePosition = GetScreenToWorld2D(GetMousePosition(), ViewportManager.Camera);
    Vector2 direction = Position - mousePosition;
    rotation = MathF.Atan2(direction.Y, direction.X) * (180 / MathF.PI) - 90;



    return isActive;
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    if (!isActive) return;
    lifespan -= dt;
    if (lifespan <= 0)
    {
      Visible = false;
      isActive = false;
      lifespan = 0;
    }
  }

  public override void Draw()
  {
    base.Draw();
    DrawTexturePro(
      AssetsManager.Textures["TinyDungeon"],
      new Rectangle(128, 128, 16, 16),
      new Rectangle(Position.X, Position.Y, 16, 16),
      Anchor,
      rotation,
      Color.White
    );
  }
}