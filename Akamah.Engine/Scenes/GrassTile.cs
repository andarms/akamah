namespace Akamah.Engine.Scenes;

public class GrassTile : Tile
{
  public override TileType Type { get; } = TileType.Grass;

  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Green);
  }

  public override List<TileType> ValidNeighbors()
  {
    return
    [
      TileType.Grass,
      TileType.Forest,
      TileType.Sand,
    ];
  }
}
