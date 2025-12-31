using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Combat;

public class RemoveOnDeath() : GameObject
{
  public override void Initialize()
  {
    base.Initialize();
    When<HealthDepleted>(_ => GetRoot().Terminate());
  }
}