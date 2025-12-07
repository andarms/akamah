using Akamah.Engine.Assets;

namespace Akamah.Engine.Core;

public class Game
{
  protected int ScreenWidth { get; set; } = 1280;
  protected int ScreenHeight { get; set; } = 720;
  protected string WindowTitle { get; set; } = "Akamah Engine";
  protected int TargetFPS { get; set; } = 60;

  protected virtual void Initialize()
  {
    SceneManager.Initialize();
  }

  protected virtual void LoadContent()
  {
    AssetsManager.LoadAssets();
  }

  protected virtual void Update(float deltaTime)
  {
    InputManager.Update();
    SceneManager.Update(deltaTime);
  }

  protected virtual void Draw()
  {
    BeginDrawing();
    SceneManager.Draw();
    EndDrawing();
  }

  protected virtual void UnloadContent()
  {
    SceneManager.ClearAllScenes();
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