using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Engine.Scenes;

public interface IScenesController
{
  void SwitchTo<T>() where T : Scene;
  void Push<T>() where T : Scene;
  void Pop();
}

public class ScenesController : IScenesController
{
  Scene? currentScene = null;
  Scene? previousScene = null;
  readonly Dictionary<Type, Scene> scenes = [];
  readonly Stack<Scene> sceneStack = [];

  public bool IsTransitioning { get; private set; } = false;
  public Color BackgroundColor { get; set; } = Color.Black;

  public Scene? InitialScene => scenes.Values.FirstOrDefault();

  public void Initialize()
  {
    currentScene = InitialScene ?? throw new InvalidOperationException("No initial scene found. Please add at least one scene before initializing the SceneController.");
    currentScene.Initialize();
    AssetsManager.LoadAssets();
  }

  public void AddScene(Scene scene)
  {
    scenes.Add(scene.GetType(), scene);
  }

  public void AddScene<T>(T scene) where T : Scene
  {
    scenes[typeof(T)] = scene;
  }

  public void SwitchTo<T>() where T : Scene
  {
    if (!scenes.TryGetValue(typeof(T), out var scene))
    {
      throw new InvalidOperationException($"Scene of type {typeof(T).Name} not found. Please add the scene before switching to it.");
    }
    SwitchToScene(scene);
  }

  private void SwitchToScene(Scene scene)
  {
    IsTransitioning = true;
    previousScene = currentScene;
    previousScene?.OnExit();

    currentScene = scene;
    currentScene.OnEnter();
    currentScene.Initialize();

    IsTransitioning = false;
  }

  public void Update(float dt)
  {
    currentScene?.HandleInput();
    currentScene?.Update(dt);
  }

  public void Draw()
  {
    ClearBackground(BackgroundColor);

    // Draw stacked scenes (for overlays)
    foreach (var scene in sceneStack)
    {
      scene.Draw();
    }

    // Draw current scene
    currentScene?.Draw();
  }

  public void Push<T>() where T : Scene
  {
    Scene? scene = GetScene<T>();
    if (scene == null)
    {
      return;
    }

    if (currentScene != null)
    {
      sceneStack.Push(currentScene);
      currentScene.OnPause();
    }

    currentScene = scene;
    currentScene.OnEnter();
    currentScene.Initialize();
  }

  public void Pop()
  {
    if (sceneStack.Count > 0)
    {
      currentScene?.OnExit();
      currentScene = sceneStack.Pop();
      currentScene.OnResume();
    }
  }

  public T? GetScene<T>() where T : Scene
  {
    return scenes.TryGetValue(typeof(T), out var scene) ? scene as T : null;
  }

  public bool HasScene<T>() where T : Scene
  {
    return scenes.ContainsKey(typeof(T));
  }

  public void RemoveScene<T>() where T : Scene
  {
    if (scenes.TryGetValue(typeof(T), out var scene))
    {
      if (currentScene == scene)
      {
        currentScene = null;
      }
      scene.Dispose();
      scenes.Remove(typeof(T));
    }
  }

  public void ClearAllScenes()
  {
    foreach (var scene in scenes.Values)
    {
      scene.Dispose();
    }
    scenes.Clear();
    sceneStack.Clear();
    currentScene = null;
    previousScene = null;
  }

  public void Add(GameObject obj) => currentScene?.Add(obj);

  public void AddUI(GameObject obj) => currentScene?.AddUI(obj);


  public void Remove(GameObject obj) => currentScene?.Remove(obj);
}