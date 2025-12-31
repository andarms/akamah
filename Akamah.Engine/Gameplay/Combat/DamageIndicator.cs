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
  readonly Text text = new(damage.ToString())
  {
    Color = Color.Red,
    FontSize = 14
  };

  public override void Initialize()
  {
    base.Initialize();
    Position = startPosition;
    Anchor = new(0.5f, 0.5f);
    Add(text);
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

    if (text != null)
    {
      float alpha = MathF.Max(0, 1 - progress);
      text.Color = new Color(text.Color.R, text.Color.G, text.Color.B, alpha);
    }
  }
}