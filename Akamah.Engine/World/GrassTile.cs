using Akamah.Engine.Assets;

namespace Akamah.Engine.World;

public class GrassTile : Tile
{
  public override TileType Type { get; } = TileType.Grass;

  public override void Draw()
  {
    base.Draw();
    DrawTexturePro(
      AssetsManager.Textures["TinyTown"],
      new Rectangle(0, 0, 16, 16),
      new Rectangle(Position.X, Position.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
  }
}
