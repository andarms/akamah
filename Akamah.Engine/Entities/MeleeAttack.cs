using Akamah.Engine.Collisions;
using Akamah.Engine.Core;

namespace Akamah.Engine.Entities;

public class MeleeAttack : GameObject
{
  public HashSet<GameObject> ImpactList { get; } = [];

  public MeleeAttack()
  {
    Collider = new CircleCollider
    {
      Size = new Vector2(16),
      Offset = new Vector2(0),
      Radius = 8
    };
  }

  public override void Initialize()
  {
    base.Initialize();
    ImpactList.Clear();
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    var collisions = CollisionsManager.GetPotentialCollisions(this);
    foreach (var other in collisions)
    {
      if (other is Player || ImpactList.Contains(other)) continue;

      ImpactList.Add(other);
      if (other is IDamageable damagable)
      {
        InflictDamage(damagable);
      }
    }
  }

  public virtual void InflictDamage(IDamageable damagable)
  {
    Damage damage = new(10, DamageSource.None);
    if (damagable.CanTakeDamage(damage))
    {
      damagable.TakeDamage(new Damage(10, DamageSource.Axe));
    }
  }
}