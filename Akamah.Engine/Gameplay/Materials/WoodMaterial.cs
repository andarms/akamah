using Akamah.Engine.Core.Engine;

namespace Akamah.Engine.Gameplay.Materials;

public class WoodMaterial() : Component, IHandle<Chop>, IHandle<Slash>, IHandle<Mine>
{
  public void Handle(Chop action)
  {
    Owner.Handle(new DamageAction(action.Damage));
  }

  public void Handle(Slash action)
  {
    Owner.Handle(new DamageAction(action.Damage / 3));
  }

  public void Handle(Mine action)
  {
    Owner.Handle(new DamageAction(action.Damage / 2));
  }
}

