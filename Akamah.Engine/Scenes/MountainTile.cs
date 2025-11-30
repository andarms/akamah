namespace Akamah.Engine.Scenes;

public class MountainTile : Tile
{
  public override TileType Type { get; } = TileType.Mountain;

  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Gray);
  }

  public override List<(TileType type, float weight)> ValidNeighbors()
  {
    return
    [
      (TileType.Mountain, 2.5f),  // High chance to cluster (mountain ranges)
      (TileType.Forest, 1.5f),    // Medium chance (forested mountains)
    ];
  }
}
