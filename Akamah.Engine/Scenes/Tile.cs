namespace Akamah.Engine.Scenes;

public abstract class Tile : GameObject
{
  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, new Vector2(16), Color.Black);
  }
}
