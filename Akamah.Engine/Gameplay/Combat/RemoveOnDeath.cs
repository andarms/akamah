using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Combat;

public class RemoveOnDeath() : Component
{
  public override void Initialize()
  {
    base.Initialize();
    Owner.When<HealthDepleted>(_ => Owner.Terminate());
  }
}