using Akamah.Engine.Gameplay.Interactions;

namespace Akamah.Engine.Gameplay.Traits;

public interface IDamageable
{
  bool CanTakeDamage(Damage damage);
  void TakeDamage(Damage damage);
}
