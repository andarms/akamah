using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Gameplay.Equipment;

public enum ToolAction
{
  Chop,
  Mine,
  Dig,
  None
}

public class Tool
{
  public string Name { get; set; } = "Unnamed Tool";
  public Material Material { get; set; } = Material.None;
  public ToolAction Action { get; set; } = ToolAction.None;
  public float BasePower { get; set; } = 0;
  public Rectangle SourceSprite { get; set; } = new Rectangle(0, 0, 0, 0);
  public override string ToString()
  {
    return Name;
  }
}


public record ToolDamage(Tool Tool, int DamageAmount) : GameAction;


public static class ToolsFactory
{

  public static Tool CreateSword(Material material)
  {
    if (material == Material.None) throw new ArgumentException("Sword material cannot be None.");

    return new Tool
    {
      Name = $"{material} Sword",
      Material = material,
      BasePower = 8,
      SourceSprite = new Rectangle(176, 128, 16, 16),
      Action = ToolAction.None
    };
  }

  public static Tool CreateAxe(Material material)
  {
    if (material == Material.None) throw new ArgumentException("Axe material cannot be None.");

    return new Tool
    {
      Name = $"{material} Axe",
      Material = material,
      BasePower = 15,
      SourceSprite = new Rectangle(176, 144, 16, 16),
      Action = ToolAction.Chop
    };
  }

  public static Tool CreatePickaxe(Material material)
  {
    if (material == Material.None) throw new ArgumentException("Pickaxe material cannot be None.");
    return new Tool
    {
      Name = $"{material} Pickaxe",
      Material = material,
      BasePower = 12,
      SourceSprite = new Rectangle(192, 144, 16, 16),
      Action = ToolAction.Mine
    };
  }

  public static Tool CreateShovel(Material material)
  {
    if (material == Material.None) throw new ArgumentException("Shovel material cannot be None.");
    return new Tool
    {
      Name = $"{material} Shovel",
      Material = material,
      BasePower = 10,
      SourceSprite = new Rectangle(192, 128, 16, 16),
      Action = ToolAction.Dig
    };
  }
}