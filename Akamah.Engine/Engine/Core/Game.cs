using Akamah.Engine.Assets;
using Akamah.Engine.Core.Scene;
using Akamah.Engine.Engine.Input;

namespace Akamah.Engine.Core.Engine;

public class Game
{
  protected int ScreenWidth { get; set; } = 1280;
  protected int ScreenHeight { get; set; } = 720;
  protected string WindowTitle { get; set; } = "Akamah Engine";
  protected int TargetFPS { get; set; } = 60;

  protected virtual void Initialize()
  {
    SceneController.Initialize();
  }

  protected virtual void LoadContent()
  {
    AssetsManager.LoadAssets();
  }

  protected virtual void Update(float deltaTime)
  {
    InputSystem.Update();
    SceneController.Update(deltaTime);
  }

  protected virtual void Draw()
  {
    BeginDrawing();
    SceneController.Draw();
    EndDrawing();
  }

  protected virtual void UnloadContent()
  {
    SceneController.ClearAllScenes();
    AssetsManager.UnloadAssets();
  }

  public void Run()
  {
    InitWindow(ScreenWidth, ScreenHeight, WindowTitle);
    SetTargetFPS(TargetFPS);

    try
    {

      Initialize();
      LoadContent();


      while (!WindowShouldClose())
      {
        float deltaTime = GetFrameTime();
        Update(deltaTime);
        Draw();
      }
    }
    finally
    {
      UnloadContent();
      CloseWindow();
    }
  }

  protected void Exit()
  {
    // This will cause WindowShouldClose() to return true
    // The game loop will exit and cleanup will occur
  }
}