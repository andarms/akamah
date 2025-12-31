using Akamah.Engine.Engine.Core;
using Akamah.Engine.UserInterface;

namespace Akamah.Engine.Engine.Scenes;

public abstract class Scene : IDisposable
{
  public bool IsInitialized { get; private set; }

  public bool IsActive { get; internal set; }

  public virtual Color BackgroundColor => Color.Black;


  protected readonly List<GameObject> objects = [];
  protected readonly List<GameObject> pendingObjects = [];
  protected readonly List<GameObject> removeObjects = [];
  protected readonly List<GameObject> ui = [];

  public void Add(GameObject obj)
  {
    pendingObjects.Add(obj);
  }

  public void Remove(GameObject obj)
  {
    removeObjects.Add(obj);
  }

  public virtual void Initialize()
  {
    IsInitialized = true;
    foreach (var gameObject in objects)
    {
      gameObject.Initialize();
    }
  }

  public virtual void OnEnter() { }

  public virtual void OnExit() { }

  public virtual void OnPause() { }

  public virtual void OnResume() { }

  public virtual void Update(float deltaTime)
  {
    UpdateWorld(deltaTime);
    UpdateUI(deltaTime);
    ProcessPendingObjects();
  }

  private void ProcessPendingObjects()
  {
    if (pendingObjects.Count <= 0) return;
    objects.AddRange(pendingObjects);
    foreach (var obj in pendingObjects.ToArray())
    {
      obj.Initialize();
    }
    pendingObjects.Clear();

    if (removeObjects.Count <= 0) return;
    foreach (var obj in removeObjects.ToArray())
    {
      objects.Remove(obj);
      ui.Remove(obj);
      obj.Terminate();
    }
    removeObjects.Clear();
  }

  protected virtual void UpdateUI(float deltaTime)
  {
    foreach (var uiObject in ui)
    {
      uiObject.Update(deltaTime);
    }
  }

  protected virtual void UpdateWorld(float deltaTime)
  {
    foreach (var gameObject in objects)
    {
      gameObject.Update(deltaTime);
    }
  }

  public virtual void Draw()
  {
    BeginMode2D(Game.Viewport.Camera);
    DrawWorld();
    EndMode2D();
    DrawUI();
  }

  protected virtual void DrawWorld()
  {
    foreach (var obj in objects)
    {
      obj.Draw();
    }
  }

  protected virtual void DrawUI()
  {
    foreach (var uiObject in ui)
    {
      uiObject.Draw();
    }
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