namespace Akamah.Engine.Scenes;

public class ForestTile : Tile
{
  public override TileType Type { get; } = TileType.Forest;
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.DarkGreen);
  }

  public override List<TileType> ValidNeighbors()
  {
    return
    [
      TileType.Forest,
      TileType.Grass,
      TileType.Mountain,
    ];
  }
}
