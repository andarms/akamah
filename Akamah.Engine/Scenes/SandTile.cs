namespace Akamah.Engine.Scenes;

public class SandTile : Tile
{
  public override TileType Type { get; } = TileType.Sand;

  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Yellow);
  }
}
