using Akamah.Engine.Core.Engine;
using Akamah.Engine.Systems;

namespace Akamah.Engine.Gameplay.UI;

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
    GameManager.AddGameObject(damageIndicator);
  }
}