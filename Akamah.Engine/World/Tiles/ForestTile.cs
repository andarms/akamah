using Akamah.Engine.Assets.Management;
using Akamah.Engine.Gameplay.World.Flora;
using Akamah.Engine.Systems;

namespace Akamah.Engine.World.Tiles;

public class ForestTile : Tile
{
  readonly Random random = new();
  public override TileType Type { get; } = TileType.Forest;

  public override void Initialize()
  {
    base.Initialize();
    if (random.NextDouble() < 0.3)
    {
      Tree tree = new() { Position = Position + new Vector2(4, 4) };
      GameManager.AddGameObject(tree);
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
