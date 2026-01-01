using Akamah.Engine.Engine.Core;
using Akamah.Engine.UserInterface;
using Akamah.Engine.World;

namespace Akamah.Engine.Gameplay.Combat;

public class DamageIndicator : GameObject
{
  const float LIFETIME = 0.5f;
  const float RiseDistance = 20.0f;

  float lifetime = LIFETIME;
  readonly Vector2 startPosition;
  readonly Text text;

  public DamageIndicator(Vector2 position, int damage) : base()
  {
    startPosition = position;
    text = new(damage.ToString())
    {
      Color = Color.White,
      FontSize = 16
    };
    Add(text);
  }

  public override void Initialize()
  {
    base.Initialize();
    Position = startPosition;
    Anchor = Vector2.Zero;
    text.Position = Vector2.Zero;
    text.Anchor = Vector2.Zero;
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


    float progress = 1 - (lifetime / LIFETIME);
    Vector2 newPosition = startPosition - new Vector2(0, RiseDistance * progress);
    Position = newPosition;

    float alpha = lifetime / LIFETIME;
    text.Color = new Color(text.Color.R, text.Color.G, text.Color.B, (byte)(alpha * 255));
  }
}