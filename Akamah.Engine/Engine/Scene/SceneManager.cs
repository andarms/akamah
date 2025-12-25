using Akamah.Engine.Assets;

namespace Akamah.Engine.Core.Scene;

public static class SceneController
{
  static Scene? currentScene = null;
  static Scene? previousScene = null;
  static readonly Dictionary<Type, Scene> scenes = [];
  static readonly Stack<Scene> sceneStack = [];

  public static Scene? CurrentScene => currentScene;
  public static bool IsTransitioning { get; private set; } = false;
  public static Color BackgroundColor { get; set; } = Color.Black;

  public static void Initialize()
  {
    currentScene?.Initialize();
    AssetsManager.LoadAssets();
  }

  public static void AddScene(Scene scene)
  {
    scenes.Add(scene.GetType(), scene);
  }

  public static void AddScene<T>(T scene) where T : Scene
  {
    scenes[typeof(T)] = scene;
  }

  public static void SwitchTo<T>() where T : Scene, new()
  {
    if (scenes.TryGetValue(typeof(T), out var scene))
    {
      SwitchToScene(scene);
    }
    else
    {
      // Create new scene if not found
      var newScene = new T();
      scenes[typeof(T)] = newScene;
      SwitchToScene(newScene);
    }
  }

  public static void SwitchTo<T>(T scene) where T : Scene
  {
    scenes[typeof(T)] = scene;
    SwitchToScene(scene);
  }

  private static void SwitchToScene(Scene scene)
  {
    IsTransitioning = true;
    previousScene = currentScene;
    previousScene?.OnExit();

    currentScene = scene;
    currentScene.OnEnter();
    currentScene.Initialize();

    IsTransitioning = false;
  }

  public static void Update(float dt)
  {
    currentScene?.HandleInput();
    currentScene?.Update(dt);
  }

  public static void Draw()
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

  public static void PushScene<T>() where T : Scene
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

  public static void PopScene()
  {
    if (sceneStack.Count > 0)
    {
      currentScene?.OnExit();
      currentScene = sceneStack.Pop();
      currentScene.OnResume();
    }
  }

  public static T? GetScene<T>() where T : Scene
  {
    return scenes.TryGetValue(typeof(T), out var scene) ? scene as T : null;
  }

  public static bool HasScene<T>() where T : Scene
  {
    return scenes.ContainsKey(typeof(T));
  }

  public static void RemoveScene<T>() where T : Scene
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

  public static void ClearAllScenes()
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
}