using Akamah.Engine.Engine.Core;
using Akamah.Engine.Systems.Collision;
using Akamah.Engine.World;
using Akamah.Engine.World.Actors.Player;

namespace Akamah.Engine.Gameplay.Combat;

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
      other.Handle(Game.Player.Tool.Action);
    }
  }
}
