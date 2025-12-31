using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;
using Akamah.Engine.World.Environment.Minerals;

namespace Akamah.Engine.World.Tiles;

public class MountainTile : Tile
{
  public override TileType Type { get; } = TileType.Mountain;

  public MountainTile()
  {
    AddChild(new Sprite { TexturePath = "Desert", SourceRect = new Rectangle(16, 48, 16, 16) });
  }

  public override void Initialize()
  {
    base.Initialize();
    Rock rock = new() { Position = Position };
    Game.Add(rock);
  }
}
