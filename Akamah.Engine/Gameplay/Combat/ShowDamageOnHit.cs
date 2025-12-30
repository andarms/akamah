using Akamah.Engine.Engine.Core;

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
    var damageIndicator = new DamageIndicator(Owner.Position, evt.Amount);
    Game.Add(damageIndicator);
  }
}