namespace Akamah.Engine.Entities;

public enum DamageType
{
  None,
  Slash,
  Chop,
  Mine,
  Dig
}


public class Damage
{
  public float Power { get; set; } = 0;
  public DamageType Type { get; set; } = DamageType.None;
}