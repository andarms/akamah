using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Combat;

public class ShowDamageOnHit : GameObject
{
  public override void Initialize()
  {
    base.Initialize();
    When<HealthChanged>(OnDamageTaken);
  }

  private void OnDamageTaken(HealthChanged evt)
  {
    var damageIndicator = new DamageIndicator(Position, evt.Amount);
    Game.Add(damageIndicator);
  }
}