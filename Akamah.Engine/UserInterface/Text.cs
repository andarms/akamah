using Akamah.Engine.Engine.Core;

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
