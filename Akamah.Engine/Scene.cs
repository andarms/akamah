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
    foreach (var gameObject in GameManager.GameObjects.ToArray())
    {
      gameObject.Update(deltaTime);
    }
  }

  public virtual void Draw()
  {
    foreach (var gameObject in GameManager.GameObjects.ToArray())
    {
      gameObject.Draw();
    }
  }

  public virtual void HandleInput()
  {
    // Override in derived classes for scene-specific input handling
  }

  public virtual void Dispose()
  {
    // Override in derived classes for cleanup
    IsInitialized = false;
    GC.SuppressFinalize(this);
  }

  public virtual void Unload()
  {
    Dispose();
  }
}