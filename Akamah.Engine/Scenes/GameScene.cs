using Akamah.Engine.Engine.Camera;
using Akamah.Engine.Engine.Physics.Spatial;
using Akamah.Engine.Engine.Scene;
using Akamah.Engine.Gameplay.Inventories;
using Akamah.Engine.UserInterface;
using Akamah.Engine.World;

namespace Akamah.Engine.Scenes;

public class GameScene : Scene
{
  public override void Initialize()
  {
    base.Initialize();
    GameWorld.Initialize();
    Canvas.Add(new Toolbar(), Anchor.BottomCenter, new Vector2(0, -16));
  }

  public override void HandleInput()
  {
    if (IsKeyPressed(KeyboardKey.R))
    {
      GameWorld.Map.GenerateRandomMap();
    }
    if (IsKeyPressed(KeyboardKey.I))
    {
      SceneController.PushScene<InventoryScene>();
    }
  }

  public override void Draw()
  {
    BeginMode2D(ViewportManager.Camera);
    base.Draw();
    EndMode2D();
    DrawUI();
  }

  private static void DrawUI()
  {
    if (GameWorld.DebugMode)
    {
      // Debug UI
      DrawRectangleV(new Vector2(0, 0), new Vector2(200, 100), Color.Black);
      DrawFPS(10, 10);

      // Always show basic controls
      DrawText("F1: Debug | R: Regen", 10, GetScreenHeight() - 30, 20, Color.White);

      var (totalObjects, visibleObjects, collisionChecks, _) = SpatialSystem.GetPerformanceInfo();

      DrawText($"Objects: {totalObjects}", 10, 30, 20, Color.White);
      DrawText($"Visible: {visibleObjects}", 10, 50, 20, Color.White);
      DrawText($"Collisions: {collisionChecks}", 10, 70, 20, Color.White);
    }
  }
}