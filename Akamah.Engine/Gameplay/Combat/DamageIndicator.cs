using Akamah.Engine.Engine.Core;
using Akamah.Engine.Gameplay.UI;
using Akamah.Engine.World;

namespace Akamah.Engine.Gameplay.Combat;

public class DamageIndicator(Vector2 position, int damage) : GameObject
{
  const float Lifetime = 0.5f;
  const float RiseDistance = 20.0f;

  float lifetime = Lifetime;
  readonly Vector2 startPosition = position;
  readonly int damageAmount = damage;

  public override void Initialize()
  {
    base.Initialize();
    Position = startPosition;
    Anchor = new(0.5f, 0.5f);
    // Set a simple text representation for the damage indicator
    var textComponent = new Text(damageAmount.ToString())
    {
      Color = Color.Red,
      FontSize = 14
    };
    AddChild(textComponent);
  }

  public override void Update(float deltaTime)
  {
    base.Update(deltaTime);

    lifetime -= deltaTime;
    if (lifetime <= 0)
    {
      Game.Remove(this);
      return;
    }

    // Move the indicator upwards over its lifetime
    float progress = 1 - (lifetime / Lifetime);
    Position = startPosition - new Vector2(0, RiseDistance * progress);

    var textComponent = Get<Text>();
    if (textComponent != null)
    {
      float alpha = MathF.Max(0, 1 - progress);
      textComponent.Color = new Color(textComponent.Color.R, textComponent.Color.G, textComponent.Color.B, alpha);
    }
  }
}