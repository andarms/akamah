namespace Akamah.Engine.World.Tiles;

public class WaterTile : Tile
{
  public override TileType Type { get; } = TileType.Water;
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Blue);
  }
}
