using Akamah.Engine.Engine.Core;
using Akamah.Engine.World;

namespace Akamah.Engine.Gameplay.Combat;

public class ShowDamageOnHit : Component
{
  public override void Initialize()
  {
    base.Initialize();
    Owner.When<HealthChanged>(OnDamageTaken);
  }

  private void OnDamageTaken(HealthChanged evt)
  {
    var damageIndicator = new DamageIndicator(Owner.Position, evt.Before - evt.After);
    GameWorld.AddGameObject(damageIndicator);
  }
}