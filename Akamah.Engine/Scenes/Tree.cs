namespace Akamah.Engine.Scenes;

public class Tree : GameObject
{
  public override void Draw()
  {
    DrawRectangleV(Position + new Vector2(2, 0), new Vector2(4, 12), Color.Brown);
    DrawCircleV(Position + new Vector2(4, 0), 6, Color.Lime);
  }
}
