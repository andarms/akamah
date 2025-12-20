using Akamah.Engine.Core;
using Akamah.Engine.Assets;
using Akamah.Engine.Collisions;
using Akamah.Engine.Systems;

namespace Akamah.Engine.Entities;

public class Tree : GameObject, IDamageable
{
  public Tree()
  {
    Visible = true;
    Anchor = new(8, 16);
    Collider = new Collider
    {
      Size = new Vector2(16, 8),
      Offset = new Vector2(0, 8),
      Solid = true
    };
  }

  public Health Health { get; } = new Health(30);

  public override void Draw()
  {
    // Spatial system handles visibility culling for trees
    Visible = true;

    DrawTexturePro(
      AssetsManager.Textures["TinyTown"],
      new Rectangle(64, 32, 16, 16),
      new Rectangle(RenderPosition.X, RenderPosition.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
  }

  public bool CanTakeDamage(Damage damage) => true;

  public void TakeDamage(Damage damage)
  {
    int amount = damage.Amount;
    if (damage.Source == DamageSource.Axe)
    {
      amount *= 2;
    }
    Health.TakeDamage(amount);
    Console.WriteLine($"Tree took {amount} damage, current health: {Health.Current}");
    if (Health.Current <= 0)
    {
      GameManager.RemoveGameObject(this);
    }
  }
}
