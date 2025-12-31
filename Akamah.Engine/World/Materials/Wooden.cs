using Akamah.Engine.Engine.Core;
using Akamah.Engine.Gameplay.Combat;
using Akamah.Engine.Gameplay.Equipment;

namespace Akamah.Engine.World.Materials;

public class Wooden : GameObject
{

  public override void Initialize()
  {
    base.Initialize();
    Handle<ToolDamage>(Use);
  }

  void Use(ToolDamage action)
  {
    int damage = (int)CalculateDamage(action);
    Emit(new DamageTaken(damage));
  }


  public float CalculateDamage(ToolDamage action)
  {
    return action.Tool switch
    {
      Tool tool when tool.Action == ToolAction.Chop => tool.BasePower * 1.5f,
      Tool tool when tool.Action == ToolAction.Mine => tool.BasePower * 0.5f,
      Tool tool when tool.Action == ToolAction.Dig => tool.BasePower * 0.2f,
      _ => 5,// Example fixed damage value
    };
  }
}

