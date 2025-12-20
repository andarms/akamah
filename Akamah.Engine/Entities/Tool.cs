namespace Akamah.Engine.Entities;

public class Tool
{
  public string Name { get; set; } = "Unnamed Tool";
  public Material Material { get; set; } = Material.None;
  public DamageType AttackType { get; set; } = DamageType.None;
  public float BasePower { get; set; } = 0;
  public Rectangle SourceSprite { get; set; } = new Rectangle(0, 0, 0, 0);

  public Damage CalculateDamage()
  {
    return new Damage
    {
      Power = BasePower * Material.EfficiencyMultiplier(),
      Type = AttackType
    };
  }


  public override string ToString()
  {
    return Name;
  }
}


public static class ToolsFactory
{

  public static Tool CreateSword(Material material)
  {
    if (material == Material.None) throw new ArgumentException("Sword material cannot be None.");

    return new Tool
    {
      Name = $"{material} Sword",
      Material = material,
      AttackType = DamageType.Slash,
      BasePower = 8,
      SourceSprite = new Rectangle(176, 128, 16, 16)
    };
  }


  public static Tool CreateAxe(Material material)
  {
    if (material == Material.None) throw new ArgumentException("Axe material cannot be None.");

    return new Tool
    {
      Name = $"{material} Axe",
      Material = material,
      AttackType = DamageType.Chop,
      BasePower = 15,
      SourceSprite = new Rectangle(176, 144, 16, 16)
    };
  }

  public static Tool CreatePickaxe(Material material)
  {
    if (material == Material.None) throw new ArgumentException("Pickaxe material cannot be None.");
    return new Tool
    {
      Name = $"{material} Pickaxe",
      Material = material,
      AttackType = DamageType.Mine,
      BasePower = 12,
      SourceSprite = new Rectangle(192, 144, 16, 16)
    };
  }

  public static Tool CreateShovel(Material material)
  {
    if (material == Material.None) throw new ArgumentException("Shovel material cannot be None.");
    return new Tool
    {
      Name = $"{material} Shovel",
      Material = material,
      AttackType = DamageType.Dig,
      BasePower = 10,
      SourceSprite = new Rectangle(192, 128, 16, 16)
    };
  }
}