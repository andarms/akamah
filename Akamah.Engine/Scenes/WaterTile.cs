namespace Akamah.Engine.Scenes;

public class WaterTile : Tile
{
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Blue);
  }
}
