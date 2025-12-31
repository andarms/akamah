using Akamah.Engine.Engine.Core;
using Akamah.Engine.Gameplay.Combat;
using Akamah.Engine.Gameplay.Equipment;

namespace Akamah.Engine.World.Materials;

public class Stone : GameObject, IHandle<Mine>
{
  public void Handle(Mine action)
  {
    Parent?.Handle(new Damage(action.Damage));
  }
}
