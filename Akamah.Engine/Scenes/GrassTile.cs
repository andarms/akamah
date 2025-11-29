namespace Akamah.Engine.Scenes;

public class GrassTile : Tile
{
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Green);
  }
}
