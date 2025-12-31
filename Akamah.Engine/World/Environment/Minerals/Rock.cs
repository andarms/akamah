using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;
using Akamah.Engine.Gameplay.Combat;
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
    Add(new Health(50));
    Add(new Stone());
    Add(new RemoveOnDeath());
    Add(new ShowDamageOnHit());
    Add(new Sprite() { TexturePath = "Desert", SourceRect = new Rectangle(64, 48, 16, 16) });
  }
}
