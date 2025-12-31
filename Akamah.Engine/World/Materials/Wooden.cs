using Akamah.Engine.Engine.Core;
using Akamah.Engine.Gameplay.Combat;
using Akamah.Engine.Gameplay.Equipment;

namespace Akamah.Engine.World.Materials;

public class Wooden() : GameObject, IHandle<Chop>, IHandle<Slash>, IHandle<Mine>
{
  public void Handle(Chop action)
  {
    Parent?.Handle(new Damage(action.Damage));
  }

  public void Handle(Slash action)
  {
    Parent?.Handle(new Damage(action.Damage / 3));
  }

  public void Handle(Mine action)
  {
    Parent?.Handle(new Damage(action.Damage / 2));
  }
}

