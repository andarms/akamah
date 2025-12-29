using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Input;
using Akamah.Engine.Engine.Scene;

namespace Akamah.Engine.Core.Engine;

public class Game
{

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
    InitWindow(Setting.SCREEN_WIDTH, Setting.SCREEN_HEIGHT, Setting.TITLE);
    SetTargetFPS(Setting.TARGET_FPS);

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