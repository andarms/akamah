
namespace Akamah.Engine.Scenes;

public class WaterTile : Tile
{
  public override TileType Type { get; } = TileType.Water;
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Blue);
  }


  public override List<TileType> ValidNeighbors()
  {
    return
    [
      TileType.Water,
      TileType.Sand,
    ];
  }
}
