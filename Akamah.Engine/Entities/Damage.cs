namespace Akamah.Engine.Entities;

public interface IDamageable
{
  bool CanTakeDamage(Damage damage);
  void TakeDamage(Damage damage);
}
