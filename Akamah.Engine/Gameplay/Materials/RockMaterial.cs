using Akamah.Engine.Core.Engine;

namespace Akamah.Engine.Gameplay.Materials;

public class RockMaterial : Component, IHandle<Mine>
{
  public void Handle(Mine action)
  {
    Owner.Handle(new DamageAction(action.Damage));
  }
}
