namespace Akamah.Engine.Scenes;

public class ForestTile : Tile
{
  public override TileType Type { get; } = TileType.Forest;
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.DarkGreen);
  }

  public override List<(TileType type, float weight)> ValidNeighbors()
  {
    return
    [
      (TileType.Forest, 3.0f),    // High chance to cluster
      (TileType.Grass, 2.0f),     // Medium chance (forest edges)
      (TileType.Mountain, 1.5f),  // Medium chance (forested hills)
    ];
  }
}
