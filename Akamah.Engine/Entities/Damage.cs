namespace Akamah.Engine.Entities;

public readonly record struct DamageSource(string Id)
{
  public static readonly DamageSource None = new("None");
  public static readonly DamageSource Axe = new("Axe");
  public static readonly DamageSource Sword = new("Sword");
}

public record Damage(int Amount, DamageSource Source);

public interface IDamageable
{
  bool CanTakeDamage(Damage damage);
  void TakeDamage(Damage damage);
}