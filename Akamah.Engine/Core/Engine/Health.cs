namespace Akamah.Engine.Core.Engine;

public class Health(int max) : Component, IHandle<DamageAction>
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
    Owner.Emit(new HealthChanged(Owner, before, Current));
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
    Owner.Emit(new HealthChanged(Owner, before, Current));
  }

  public void Handle(DamageAction action)
  {
    Hurt(action.Amount);
  }
}

