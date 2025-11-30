namespace Akamah.Engine.Scenes;

public abstract class Tile : GameObject
{
  public virtual TileType Type { get; } = TileType.None;
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Black);
  }

  public virtual List<(TileType type, float weight)> ValidNeighbors()
  {
    return
    [
      (TileType.Grass, 1.0f),
      (TileType.Forest, 1.0f),
      (TileType.Sand, 1.0f),
      (TileType.Mountain, 1.0f),
      (TileType.Water, 1.0f)
    ];
  }
}
