using Akamah.Engine.Assets;
using Akamah.Engine.World.Environment.Flora;

namespace Akamah.Engine.World.Tiles;

public class ForestTile : Tile
{
  readonly Random random = new();
  public override TileType Type { get; } = TileType.Forest;

  public override void Initialize()
  {
    if (Initialized) return;
    base.Initialize();
    if (random.NextDouble() < 0.3)
    {
      Tree tree = new() { Position = Position + new Vector2(4, 4) };
      GameWorld.AddGameObject(tree);
    }
  }

  public override void Draw()
  {
    base.Draw();
    DrawTexturePro(
      AssetsManager.Textures["TinyTown"],
      new Rectangle(16, 0, 16, 16),
      new Rectangle(Position.X, Position.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
  }
}
