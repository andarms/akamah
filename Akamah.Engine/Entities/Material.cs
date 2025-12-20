namespace Akamah.Engine.Entities;

public enum Material
{
  None,
  Wood,
  Stone,
  Gem
}

public static class MaterialExtensions
{
  public static float EfficiencyMultiplier(this Material material)
  {
    return material switch
    {
      Material.Wood => 1.5f,
      Material.Stone => 2f,
      Material.Gem => 5f,
      _ => 1.0f
    };
  }
}