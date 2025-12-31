using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.World.Tiles;

public class GrassTile : Tile
{
  public override TileType Type { get; } = TileType.Grass;

  public GrassTile()
  {
    AddChild(new Sprite { TexturePath = "TinyTown", SourceRect = new Rectangle(0, 0, 16, 16) });
  }
}
