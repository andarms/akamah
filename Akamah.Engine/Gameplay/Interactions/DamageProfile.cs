namespace Akamah.Engine.Gameplay.Interactions;

public class DamageProfile
{
  public Dictionary<DamageType, float> TypeMultipliers { get; set; } = [];

  public float GetMultiplier(DamageType attackType)
  {
    return TypeMultipliers.TryGetValue(attackType, out float value) ? value : 1.0f;
  }
}
