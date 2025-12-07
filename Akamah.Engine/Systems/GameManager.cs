using Akamah.Engine.Entities;
using Akamah.Engine.World;
using Akamah.Engine.Core;
using Akamah.Engine.Collisions;

namespace Akamah.Engine.Systems;

public static class GameManager
{
  public static Player Player { get; set; } = new();

  public static List<GameObject> GameObjects { get; set; } = [];

  public static Map Map { get; set; } = new(200, 200);

  public static int Seed { get; } = new Random().Next();

  public static bool DebugMode { get; set; } = false;

  public static void Initialize()
  {
    // Initialize spatial system (handles both collision and rendering)
    SpatialManager.Initialize();
    CollisionsManager.Initialize();

    // Subscribe to collision events
    CollisionsManager.OnCollisionEnter += OnCollisionEnter;
    CollisionsManager.OnCollisionExit += OnCollisionExit;

    Player.Position = new Vector2(160, 160);

    Map.GenerateRandomMap();
    GameObjects.Add(Map);
    GameObjects.Add(Player);

    // Register player with spatial system
    SpatialManager.AddObject(Player);
    if (Player.Collider != null)
    {
      CollisionsManager.AddObject(Player);
    }

    foreach (var obj in GameObjects.ToArray())
    {
      obj.Initialize();
    }

    InputManager.MapAction("move_left", KeyboardKey.Left, KeyboardKey.A);
    InputManager.MapAction("move_right", KeyboardKey.Right, KeyboardKey.D);
    InputManager.MapAction("move_up", KeyboardKey.Up, KeyboardKey.W);
    InputManager.MapAction("move_down", KeyboardKey.Down, KeyboardKey.S);
    InputManager.MapAction("attack", KeyboardKey.Space, KeyboardKey.Enter);

  }

  private static void OnCollisionEnter(GameObject objA, GameObject objB)
  {
    // Handle collision events (can be used for sound effects, particles, etc.)
    if ((objA is Player && objB is Tree) || (objA is Tree && objB is Player))
    {
      Console.WriteLine("Player collided with tree!");
    }
  }

  private static void OnCollisionExit(GameObject objA, GameObject objB)
  {
    // Handle collision exit events
    if ((objA is Player && objB is Tree) || (objA is Tree && objB is Player))
    {
      Console.WriteLine("Player left tree collision area");
    }
  }

  public static void UpdateVisibleObjects(float deltaTime)
  {
    // Always update the player
    Player.Update(deltaTime);

    // Update the map (it has its own internal culling)
    Map.Update(deltaTime);

    // Update spatial system and collision detection
    CollisionsManager.Update(deltaTime);

    // Get visible objects from spatial system for efficient updates
    var (viewportTopLeft, viewportBottomRight) = ViewportManager.CameraViewport;
    var visibleObjects = SpatialManager.GetVisibleObjects(viewportTopLeft, viewportBottomRight);

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
    var visibleObjects = SpatialManager.GetVisibleObjects(viewportTopLeft, viewportBottomRight).ToList();

    // Sort visible objects by Y position for proper rendering order
    visibleObjects.Sort((a, b) => a.Position.Y.CompareTo(b.Position.Y));

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
    SpatialManager.AddObject(gameObject);

    // Add to collision system if it has a collider
    if (gameObject.Collider != null)
    {
      CollisionsManager.AddObject(gameObject);
    }

    gameObject.Initialize();
  }

  public static void RemoveGameObject(GameObject gameObject)
  {
    if (gameObject == null) return;

    GameObjects.Remove(gameObject);
    SpatialManager.RemoveObject(gameObject);

    if (gameObject.Collider != null)
    {
      CollisionsManager.RemoveObject(gameObject);
    }
  }

  private static bool ShouldAlwaysUpdate(GameObject obj)
  {
    // Add logic here for objects that need constant updates (AI, physics, etc.)
    return obj is Player;
  }
}