
namespace Akamah.Engine.Scenes;

public class WaterTile : Tile
{
  public override TileType Type { get; } = TileType.Water;
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Blue);
  }


  public override List<(TileType type, float weight)> ValidNeighbors()
  {
    return
    [
      (TileType.Water, 10.0f),   // Very high chance to cluster (lakes, rivers)
      (TileType.Sand, 1.0f),    // Medium chance (beaches)
    ];
  }
}
