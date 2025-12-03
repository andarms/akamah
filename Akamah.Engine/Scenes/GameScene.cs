using Akamah.Engine.Managers;

namespace Akamah.Engine.Scenes;


public class GameScene : Scene
{
  public override void Initialize()
  {
    base.Initialize();
    GameManager.Initialize();
  }

  public override void HandleInput()
  {
    if (IsKeyPressed(KeyboardKey.R))
    {
      GameManager.Map.GenerateRandomMap();
    }
  }

  public override void Draw()
  {
    GameManager.GameObjects.Sort((a, b) => a.Position.Y.CompareTo(b.Position.Y));
    BeginMode2D(ViewportManager.Camera);
    base.Draw();
    EndMode2D();
    DrawRectangleV(new Vector2(0, 0), new Vector2(100, 40), Color.Black);
    DrawFPS(10, 10);
  }
}