using Akamah.Engine.Assets;

namespace Akamah.Engine.Engine.Core;

public class Sprite : GameObject
{
  public string TexturePath { get; set; } = string.Empty;
  public Rectangle SourceRect { get; set; } = new Rectangle(0, 0, 0, 0);
  public Color Tint { get; set; } = Color.White;


  public override void Draw()
  {
    Rectangle source = new(SourceRect.X, SourceRect.Y, FlipX ? -SourceRect.Width : SourceRect.Width, SourceRect.Height);
    DrawTexturePro(
      AssetsManager.Textures[TexturePath],
      source,
      new Rectangle(GlobalPosition.X, GlobalPosition.Y, 16, 16),
      GetRoot().Anchor,
      0.0f,
      Tint
    );

  }
}