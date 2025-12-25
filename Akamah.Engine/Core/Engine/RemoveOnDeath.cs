namespace Akamah.Engine.Core.Engine;

public class RemoveOnDeath() : Component
{
  public override void Initialize()
  {
    base.Initialize();
    Owner.When<HealthDepleted>(_ => Owner.Terminate());
  }
}