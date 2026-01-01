using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.UserInterface;

public class Text(string content) : GameObject
{
  public string Content { get; } = content;
  public Color Color { get; set; }
  public int FontSize { get; set; }

  public override void Draw()
  {
    base.Draw();
    DrawTextEx(AssetsManager.DefaultFont, Content, GlobalPosition, FontSize, 1.0f, Color);
  }
}
