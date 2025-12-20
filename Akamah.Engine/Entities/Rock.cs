using Akamah.Engine.Core;
using Akamah.Engine.Assets;
using Akamah.Engine.Collisions;
using Akamah.Engine.Systems;

namespace Akamah.Engine.Entities;

public class Rock : GameObject, IDamageable
{
  public Health Health { get; } = new Health(50);

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

  public bool CanTakeDamage(Damage damage) => damage.Type == DamageType.Mine;

  public void TakeDamage(Damage damage)
  {
    int amount = (int)damage.Power;
    Health.TakeDamage(amount);
    if (Health.Current <= 0)
    {
      GameManager.RemoveGameObject(this);
    }
  }
}
