namespace Akamah.Engine.Scenes;

public class GrassTile : Tile
{
  public override TileType Type { get; } = TileType.Grass;

  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Green);
  }

  public override List<(TileType type, float weight)> ValidNeighbors()
  {
    return
    [
      (TileType.Grass, 10.0f),   // High chance to cluster
      (TileType.Forest, 2.0f),  // Medium chance
      (TileType.Sand, 0.5f),    // Low chance, transition tile
    ];
  }
}
