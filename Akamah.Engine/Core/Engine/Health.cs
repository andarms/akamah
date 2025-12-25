namespace Akamah.Engine.Core.Engine;

public record HealthChanged(IReadOnlyGameObject GameObject, int Before, int After, int Amount) : GameEvent;
public record HealthDepleted(IReadOnlyGameObject GameObject) : GameEvent;

public class Health(int max) : Component, IHandle<Damage>
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
    Owner.Emit(new HealthChanged(Owner, before, Current, before - Current));
    Console.WriteLine($"Health Hurt: {before} -> {Current}");
    if (Current == 0)
    {
      Owner.Emit(new HealthDepleted(Owner));
    }
  }

  public void Heal(int amount)
  {
    if (amount <= 0 || IsDepleted) return;

    int before = Current;
    Current = Math.Min(Max, Current + amount);
    Owner.Emit(new HealthChanged(Owner, before, Current, Current - before));
  }

  public void Handle(Damage action)
  {
    Hurt(action.Amount);
  }
}

