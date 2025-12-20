using Akamah.Engine.Core;
using Akamah.Engine.Assets;
using Akamah.Engine.Collisions;
using Akamah.Engine.Systems;

namespace Akamah.Engine.Entities;

public class Tree : GameObject, IDamageable
{

  public readonly DamageProfile DamageProfile = new()
  {
    TypeMultipliers = new Dictionary<DamageType, float>
    {
      { DamageType.Chop, 2.0f },
      { DamageType.Slash, 0.5f },
      { DamageType.Mine, 0.2f },
      { DamageType.Dig, 0.2f }
    }
  };

  public Tree()
  {
    Anchor = new(8, 16);
    Collider = new Collider
    {
      Size = new Vector2(16, 8),
      Offset = new Vector2(0, 8),
      Solid = true
    };
  }

  public Health Health { get; } = new Health(30);

  public override void Draw()
  {
    DrawTexturePro(
      AssetsManager.Textures["TinyTown"],
      new Rectangle(64, 32, 16, 16),
      new Rectangle(RenderPosition.X, RenderPosition.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
  }

  public bool CanTakeDamage(Damage attack) => true;

  public void TakeDamage(Damage attack)
  {
    int amount = (int)(attack.Power * DamageProfile.GetMultiplier(attack.Type));
    Console.WriteLine($"Tree takes {amount} damage.");
    Health.TakeDamage(amount);
    if (Health.Current <= 0)
    {
      GameManager.RemoveGameObject(this);
    }
  }
}
