using Akamah.Engine.Engine.Core;
using Akamah.Engine.Gameplay.Combat;

namespace Akamah.Engine.World.Materials;

public class Wooden() : Component, IHandle<Chop>, IHandle<Slash>, IHandle<Mine>
{
  public void Handle(Chop action)
  {
    Owner.Handle(new Damage(action.Damage));
  }

  public void Handle(Slash action)
  {
    Owner.Handle(new Damage(action.Damage / 3));
  }

  public void Handle(Mine action)
  {
    Owner.Handle(new Damage(action.Damage / 2));
  }
}

