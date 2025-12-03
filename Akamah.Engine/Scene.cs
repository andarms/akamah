using Akamah.Engine.Managers;

namespace Akamah.Engine;

public abstract class Scene : IDisposable
{
  public bool IsInitialized { get; private set; }

  public bool IsActive { get; internal set; }

  public virtual Color BackgroundColor => Color.Black;

  public virtual void Initialize()
  {
    IsInitialized = true;
    foreach (var gameObject in GameManager.GameObjects.ToArray())
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
    ViewportManager.Update();
    GameManager.UpdateVisibleObjects(deltaTime);
  }

  public virtual void Draw()
  {
    ClearBackground(BackgroundColor);
    BeginMode2D(ViewportManager.Camera);
    GameManager.DrawVisibleObjects();
    EndMode2D();
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