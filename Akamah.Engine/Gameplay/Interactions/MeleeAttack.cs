using Akamah.Engine.Core.Engine;
using Akamah.Engine.Systems;
using Akamah.Engine.Systems.Collision;

namespace Akamah.Engine.Gameplay.Interactions;

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
      if (other is Player.Player || ImpactList.Contains(other)) continue;

      ImpactList.Add(other);
      other.Handle(GameWorld.Player.Tool.Action);
    }
  }
}
