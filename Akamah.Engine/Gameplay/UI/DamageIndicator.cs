using Akamah.Engine.Core.Engine;
using Akamah.Engine.Systems;

namespace Akamah.Engine.Gameplay.UI;


public class Text(string content) : Component
{
  public string Content { get; } = content;
  public Color Color { get; set; }
  public int FontSize { get; set; }

  public override void Draw()
  {
    base.Draw();
    DrawTextEx(GetFontDefault(), Content, Owner.Position, FontSize, 1.0f, Color);
  }
}

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
    Add(textComponent);
  }

  public override void Update(float deltaTime)
  {
    base.Update(deltaTime);

    lifetime -= deltaTime;
    if (lifetime <= 0)
    {
      GameWorld.RemoveGameObject(this);
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