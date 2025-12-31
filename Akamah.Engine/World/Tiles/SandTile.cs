using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.World.Tiles;

public class SandTile : Tile
{
  public override TileType Type { get; } = TileType.Sand;

  public SandTile()
  {
    AddChild(new Sprite { TexturePath = "TinyTown", SourceRect = new Rectangle(16, 32, 16, 16) });
  }
}

