namespace Akamah.Engine.Scenes;

public abstract class Tile : GameObject
{
  public virtual TileType Type { get; } = TileType.None;
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Black);
  }

  public virtual List<TileType> ValidNeighbors()
  {
    return
    [
      TileType.Grass,
      TileType.Forest,
      TileType.Sand,
      TileType.Mountain,
      TileType.Water
    ];
  }
}
