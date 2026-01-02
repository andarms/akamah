using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Combat;

public class RemoveOnDeath() : GameObject
{
  public override void Initialize()
  {
    base.Initialize();
    When<HealthDepleted>(Remove);
  }

  void Remove(HealthDepleted e)
  {
    if (Parent is not null)
    {
      Game.Remove(Parent);
      Parent.Terminate();
    }
    else
    {
      Game.Remove(this);
      Terminate();
    }
  }
}