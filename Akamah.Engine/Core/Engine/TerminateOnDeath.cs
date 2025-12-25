namespace Akamah.Engine.Core.Engine;

public class TerminateOnDeath() : Component
{
  public override void Initialize()
  {
    base.Initialize();
    Owner.When<HealthDepleted>(_ => Owner.Terminate());
  }
}

