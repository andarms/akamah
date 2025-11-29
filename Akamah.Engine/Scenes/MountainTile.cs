namespace Akamah.Engine.Scenes;

public class MountainTile : Tile
{
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Gray);
  }
}
