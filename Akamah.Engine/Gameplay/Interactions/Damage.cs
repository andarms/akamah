namespace Akamah.Engine.Gameplay.Interactions;

public enum DamageType
{
  None,
  Physical,
  Fire,
  Ice,
  Lightning,
  Poison,
  Magic,
  Chop,
  Slash,
  Mine,
  Dig
}

public class Damage
{
  public float Amount { get; set; }
  public float Power { get; set; } // For compatibility with existing code
  public DamageType Type { get; set; }
  public Vector2 Source { get; set; }

  public Damage(float amount, DamageType type, Vector2 source)
  {
    Amount = amount;
    Power = amount; // Set Power to the same as Amount for compatibility
    Type = type;
    Source = source;
  }

  public Damage(DamageType type = DamageType.Physical)
  {
    Type = type;
    Amount = 0;
    Power = 0;
    Source = Vector2.Zero;
  }
}
