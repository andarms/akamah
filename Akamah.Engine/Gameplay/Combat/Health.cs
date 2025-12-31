using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Combat;

public record HealthChanged(IReadOnlyGameObject GameObject, int Before, int After, int Amount) : GameEvent;
public record HealthDepleted(IReadOnlyGameObject GameObject) : GameEvent;

public class Health(int max) : GameObject, IHandle<Damage>
{
  public int Current { get; set; } = max;
  public int Max { get; } = max;
  public bool IsDepleted => Current == 0;

  public override void Initialize()
  {
    base.Initialize();
  }

  public void Hurt(int amount)
  {
    if (amount <= 0 || IsDepleted) return;

    int before = Current;
    Current = Math.Max(0, Current - amount);
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

  public void Handle(Damage action)
  {
    Hurt(action.Amount);
  }
}

