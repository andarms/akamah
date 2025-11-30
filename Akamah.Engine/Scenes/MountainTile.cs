namespace Akamah.Engine.Scenes;

public class MountainTile : Tile
{
  public override TileType Type { get; } = TileType.Mountain;

  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Gray);
  }

  public override List<TileType> ValidNeighbors()
  {
    return
    [
      TileType.Mountain,
      TileType.Forest,
    ];
  }
}
