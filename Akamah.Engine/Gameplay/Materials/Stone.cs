using Akamah.Engine.Core.Engine;

namespace Akamah.Engine.Gameplay.Materials;

public class Stone : Component, IHandle<Mine>
{
  public void Handle(Mine action)
  {
    Owner.Handle(new Damage(action.Damage));
  }
}
