using Akamah.Engine.Engine.Camera;
using Akamah.Engine.Engine.Input;
using Akamah.Engine.Engine.Physics.Spatial;
using Akamah.Engine.Engine.Scenes;
using Akamah.Engine.Shared;
using Akamah.Engine.Systems.Collision;
using Akamah.Engine.World;
using Akamah.Engine.World.Actors.Player;

namespace Akamah.Engine.Engine.Core;

// Game is a façade. Do not add logic here.
public static class Game
{
  const int ZoomLevel = 3;
  public static IScenesController Scenes => scenes;
  public static int Seed { get; } = new Random().Next();
  public static Player Player { get; } = new();
  public static bool DebugMode { get; set; } = false;
  public static IViewport Viewport => viewport;
  public static RandomNumberGenerator Rng { get; } = new(Seed);
  public static Map Map { get; set; } = new(200, 200);


  static readonly ScenesController scenes = new();
  static readonly Viewport viewport = new();

  public static void Register(List<Scene> scenes)
  {
    foreach (var scene in scenes)
    {
      Game.scenes.AddScene(scene);
    }
  }

  public static void Add(GameObject obj)
  {
    scenes.Add(obj);
    CollisionsManager.Add(obj);
    SpatialSystem.Add(obj);
  }


  public static void AddUI(GameObject obj)
  {
    scenes.AddUI(obj);
  }

  public static void Remove(GameObject obj)
  {
    scenes.Remove(obj);
    CollisionsManager.Remove(obj);
    SpatialSystem.Remove(obj);
  }


  #region Lifecycle Hooks
  internal static void Initialize()
  {
    SpatialSystem.Initialize();
    CollisionsManager.Initialize();
    viewport.Initialize();
    scenes.Initialize();
  }

  internal static void Update(float deltaTime)
  {
    InputSystem.Update();
    viewport.Update();
    scenes.Update(deltaTime);
  }

  internal static void Draw()
  {
    scenes.Draw();
  }

  internal static void Terminate()
  {

  }
  #endregion
}