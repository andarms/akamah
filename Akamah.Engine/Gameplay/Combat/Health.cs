using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Combat;

public record HealthChanged(GameObject GameObject, int Before, int After, int Amount) : GameEvent;
public record HealthDepleted(GameObject GameObject) : GameEvent;

public record DamageTaken(int Amount) : GameEvent;

public class Health(int max) : GameObject
{
  public int Current { get; set; } = max;
  public int Max { get; } = max;
  public bool IsDepleted => Current == 0;

  public override void Initialize()
  {
    base.Initialize();
    When<DamageTaken>(action => Hurt(action.Amount));
  }

  public void Hurt(int amount)
  {
    if (amount <= 0 || IsDepleted) return;

    int before = Current;
    Current = Math.Max(0, Current - amount);

    // Emit the DamageTaken event for other components to react
    Emit(new DamageTaken(amount));
    Emit(new HealthChanged(this, before, Current, before - Current));
    Console.WriteLine($"Health Hurt: {before} -> {Current}");

    if (Current == 0)
    {
      Emit(new HealthDepleted(this));
    }
  }

  public void Heal(int amount)
  {
    if (amount <= 0 || IsDepleted) return;

    int before = Current;
    Current = Math.Min(Max, Current + amount);
    Emit(new HealthChanged(this, before, Current, Current - before));
  }
}

