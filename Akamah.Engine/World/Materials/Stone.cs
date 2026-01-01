using Akamah.Engine.Engine.Core;
using Akamah.Engine.Gameplay.Combat;
using Akamah.Engine.Gameplay.Equipment;

namespace Akamah.Engine.World.Materials;

public class Stone : GameObject
{
  public override void Initialize()
  {
    base.Initialize();
    Handle<ToolDamage>(Use);
  }

  bool Use(ToolDamage action)
  {
    Emit(new DamageTaken(10)); // Example fixed damage value
    return true;
  }
}
