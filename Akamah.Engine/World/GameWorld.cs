using Akamah.Engine.Engine.Camera;
using Akamah.Engine.Engine.Core;
using Akamah.Engine.Engine.Input;
using Akamah.Engine.Engine.Physics.Spatial;
using Akamah.Engine.Shared;
using Akamah.Engine.Systems.Collision;
using Akamah.Engine.World.Actors.Player;
using Akamah.Engine.World.Environment.Flora;

namespace Akamah.Engine.World;

public static class GameWorld
{
  public static Player Player { get; set; } = new();

  public static List<GameObject> GameObjects { get; set; } = [];

  public static Map Map { get; set; } = new(200, 200);

  public static int Seed { get; } = new Random().Next();

  public static bool DebugMode { get; set; } = false;

  public static RandomNumberGenerator Rng { get; } = new(Seed);

  public static void Initialize()
  {
    // Initialize spatial system (handles both collision and rendering)
    SpatialSystem.Initialize();
    CollisionsManager.Initialize();

    Player.Position = new Vector2(160, 160);

    Map.GenerateRandomMap();
    GameObjects.Add(Map);
    GameObjects.Add(Player);

    // Register player with spatial system
    SpatialSystem.AddObject(Player);
    if (Player.Collider != null)
    {
      CollisionsManager.AddObject(Player);
    }

    foreach (var obj in GameObjects.ToArray())
    {
      obj.Initialize();
    }

    InputSystem.MapAction("move_left", KeyboardKey.Left, KeyboardKey.A);
    InputSystem.MapAction("move_right", KeyboardKey.Right, KeyboardKey.D);
    InputSystem.MapAction("move_up", KeyboardKey.Up, KeyboardKey.W);
    InputSystem.MapAction("move_down", KeyboardKey.Down, KeyboardKey.S);

    // Example of mapping attack to both keyboard and mouse inputs
    InputSystem.MapAction(
      "attack",
      [KeyboardKey.Space, KeyboardKey.Enter],
      [MouseButton.Left]
    );

  }


  public static void UpdateVisibleObjects(float deltaTime)
  {
    // Always update the player
    Player.Update(deltaTime);

    // Update the map (it has its own internal culsling)
    Map.Update(deltaTime);

    // Update spatial system and collision detection
    CollisionsManager.Update(deltaTime);

    // Get visible objects from spatial system for efficient updates
    var (viewportTopLeft, viewportBottomRight) = ViewportManager.CameraViewport;
    var visibleObjects = SpatialSystem.GetVisibleObjects(viewportTopLeft, viewportBottomRight);

    // Update only visible objects (except special cases)
    foreach (var obj in visibleObjects)
    {
      if (obj != Map && obj != Player && !ShouldAlwaysUpdate(obj))
      {
        obj.Update(deltaTime);
      }
    }

    // Update objects that should always be updated regardless of visibility
    foreach (var obj in GameObjects)
    {
      if (obj != Map && obj != Player && ShouldAlwaysUpdate(obj))
      {
        obj.Update(deltaTime);
      }
    }
  }

  public static void DrawVisibleObjects()
  {
    // Draw the map first (it has its own culling)
    Map.Draw();

    // Get visible objects from spatial system
    var (viewportTopLeft, viewportBottomRight) = ViewportManager.CameraViewport;
    var visibleObjects = SpatialSystem.GetVisibleObjects(viewportTopLeft, viewportBottomRight).ToList();

    // Sort visible objects by layer and then by Y position for proper rendering order
    visibleObjects.Sort((a, b) =>
    {
      int layerComparison = a.Layer.CompareTo(b.Layer);
      if (layerComparison != 0) return layerComparison;
      return a.Position.Y.CompareTo(b.Position.Y);
    });

    // Draw visible objects (excluding map since it's already drawn)
    foreach (var obj in visibleObjects)
    {
      if (obj != Map)
      {
        obj.Draw();
        if (DebugMode)
        {
          obj.Debug();
        }
      }
    }

    // Always draw the player if not already in visible objects
    if (!visibleObjects.Contains(Player))
    {
      Player.Draw();
      if (DebugMode)
      {
        Player.Debug();
      }
    }
  }


  public static void AddGameObject(GameObject gameObject)
  {
    if (gameObject == null) return;
    GameObjects.Add(gameObject);

    // Add to spatial system for rendering optimization
    SpatialSystem.AddObject(gameObject);

    // Add to collision system if it has a collider
    if (gameObject.Collider != null)
    {
      CollisionsManager.AddObject(gameObject);
    }

    gameObject.Initialize();
  }


  // random position around a point with minum distance at leat 5
  public static void Spawn(GameObject gameObject, Vector2 position, int radius)
  {
    float x = Rng.Next(-radius, radius);
    float y = Rng.Next(-radius, radius);
    Vector2 offset = new(x, y);
    gameObject.Position = position + offset;
    Console.WriteLine("Spawning " + gameObject.GetType().Name + " at " + gameObject.Position);
    AddGameObject(gameObject);
  }

  public static void RemoveGameObject(GameObject gameObject)
  {
    if (gameObject == null) return;

    GameObjects.Remove(gameObject);
    SpatialSystem.RemoveObject(gameObject);

    if (gameObject.Collider != null)
    {
      CollisionsManager.RemoveObject(gameObject);
    }

    gameObject.Terminate();
  }

  private static bool ShouldAlwaysUpdate(GameObject obj)
  {
    // Add logic here for objects that need constant updates (AI, physics, etc.)
    return obj is Player;
  }
}