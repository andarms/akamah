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
    BeginMode2D(ViewportManager.Camera);
    base.Draw();



    EndMode2D();

    if (GameManager.DebugMode)
    {
      // Debug UI
      DrawRectangleV(new Vector2(0, 0), new Vector2(280, 150), Color.Black);
      DrawFPS(10, 10);

      // Always show basic controls
      DrawText("F1: Debug | R: Regen", 10, GetScreenHeight() - 30, 20, Color.White);

      var performanceInfo = SpatialManager.GetPerformanceInfo();

      DrawText($"Objects: {performanceInfo.totalObjects}", 10, 30, 20, Color.White);
      DrawText($"Visible: {performanceInfo.visibleObjects}", 10, 50, 20, Color.White);
      DrawText($"Collisions: {performanceInfo.collisionChecks}", 10, 70, 20, Color.White);
    }
  }
}