using Akamah.Engine.Engine.Camera;
using Akamah.Engine.UserInterface;
using Akamah.Engine.World;

namespace Akamah.Engine.Engine.Scene;

public abstract class Scene : IDisposable
{
  public bool IsInitialized { get; private set; }

  public bool IsActive { get; internal set; }

  public virtual Color BackgroundColor => Color.Black;

  public virtual void Initialize()
  {
    IsInitialized = true;
    foreach (var gameObject in GameWorld.GameObjects.ToArray())
    {
      gameObject.Initialize();
    }
  }

  public virtual void OnEnter()
  {
  }

  public virtual void OnExit()
  {
  }

  public virtual void OnPause()
  {
  }

  public virtual void OnResume()
  {
  }


  public virtual void Update(float deltaTime)
  {
    if (IsKeyPressed(KeyboardKey.F1))
    {
      GameWorld.DebugMode = !GameWorld.DebugMode;
    }
    ViewportManager.Update();
    GameWorld.UpdateVisibleObjects(deltaTime);
    Canvas.Update(deltaTime);
  }

  public virtual void Draw()
  {
    ClearBackground(BackgroundColor);
    BeginMode2D(ViewportManager.Camera);
    GameWorld.DrawVisibleObjects();
    EndMode2D();
    Canvas.Draw();
  }

  public virtual void HandleInput()
  {
  }

  public virtual void Dispose()
  {
    IsInitialized = false;
    GC.SuppressFinalize(this);
  }

  public virtual void Unload()
  {
    Dispose();
  }
}