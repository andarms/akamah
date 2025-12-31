using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;
using Akamah.Engine.World.Environment.Flora;

namespace Akamah.Engine.World.Tiles;

public class ForestTile : Tile
{
  readonly Random random = new();
  public override TileType Type { get; } = TileType.Forest;

  public ForestTile()
  {
    Add(new Sprite { TexturePath = "TinyTown", SourceRect = new Rectangle(16, 0, 16, 16) });
  }

  public override void Initialize()
  {
    if (Initialized) return;
    base.Initialize();
    if (random.NextDouble() < 0.3)
    {
      Tree tree = new() { Position = Position + new Vector2(4, 4) };
      Game.Add(tree);
    }
  }
}
