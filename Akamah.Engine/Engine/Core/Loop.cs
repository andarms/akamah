using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;
using Akamah.Engine.Engine.Input;
using Akamah.Engine.Engine.Scenes;

namespace Akamah.Engine.Core.Engine;

public class Loop
{
  protected virtual void Initialize()
  {
    Game.Initialize();
  }

  protected virtual void LoadContent()
  {
    AssetsManager.LoadAssets();
  }

  protected virtual void Update(float deltaTime)
  {
    Game.Update(deltaTime);
  }

  protected virtual void Draw()
  {
    BeginDrawing();
    Game.Draw();
    EndDrawing();
  }

  protected virtual void UnloadContent()
  {
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
    Game.Terminate();
  }
}