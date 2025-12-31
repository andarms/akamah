using Akamah.Engine.Engine.Core;
using Akamah.Engine.Gameplay.Combat;
using Akamah.Engine.Gameplay.Equipment;

namespace Akamah.Engine.World.Materials;

public class Stone : GameObject
{
  public Stone()
  {
    Handle<Mine>(action => Parent?.Dispatch(new Damage(action.Damage)));
  }
}
