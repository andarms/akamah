using Akamah.Engine.Collisions;
using Akamah.Engine.Core;
using Akamah.Engine.Systems;

namespace Akamah.Engine.Entities;

public class MeleeAttack : GameObject
{
  public HashSet<GameObject> ImpactList { get; } = [];


  public MeleeAttack()
  {
    Collider = new CircleCollider
    {
      Radius = 6
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
    var damage = GameManager.Player.Tool.CalculateDamage();
    GameManager.Player.Tool.Durability.Decrease(1);
    if (damagable.CanTakeDamage(damage))
    {
      damagable.TakeDamage(damage);
    }
  }
}
