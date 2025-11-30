namespace Akamah.Engine.Scenes;

public class SandTile : Tile
{
  public override TileType Type { get; } = TileType.Sand;

  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Yellow);
  }

  public override List<(TileType type, float weight)> ValidNeighbors()
  {
    return
    [
      (TileType.Sand, 0.5f),    // Medium clustering (deserts, beaches)
      (TileType.Grass, 2.0f),   // Common transition
      (TileType.Water, 3.0f),   // Very common (beaches, shores)
    ];
  }
}
