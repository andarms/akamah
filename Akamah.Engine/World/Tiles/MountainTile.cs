using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;
using Akamah.Engine.World.Environment.Minerals;

namespace Akamah.Engine.World.Tiles;

public class MountainTile : Tile
{
  public override TileType Type { get; } = TileType.Mountain;

  public override void Initialize()
  {
    base.Initialize();
    Rock rock = new() { Position = Position };
    Game.Add(rock);
  }

  public override void Draw()
  {
    base.Draw();
    DrawTexturePro(
      AssetsManager.Textures["Desert"],
      new Rectangle(16, 48, 16, 16),
      new Rectangle(Position.X, Position.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
    ;
  }
}
