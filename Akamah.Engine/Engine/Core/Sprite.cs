using Akamah.Engine.Assets;

namespace Akamah.Engine.Engine.Core;

public class Sprite : Component
{
  public string TexturePath { get; set; } = string.Empty;
  public Rectangle SourceRect { get; set; } = new Rectangle(0, 0, 0, 0);
  public Color Tint { get; set; } = Color.White;

  public Vector2 RenderPosition => Owner.Position - Owner.Anchor;

  public override void Draw()
  {
    Rectangle source = new(SourceRect.X, SourceRect.Y, Owner.FlipX ? -SourceRect.Width : SourceRect.Width, SourceRect.Height);
    DrawTexturePro(
      AssetsManager.Textures[TexturePath],
      source,
      new Rectangle(RenderPosition.X, RenderPosition.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Tint
    );

  }
}