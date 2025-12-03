using Akamah.Engine.Scenes;

namespace Akamah.Engine.Managers;

public static class GameManager
{
  public static Player Player { get; set; } = new();

  public static List<GameObject> GameObjects { get; set; } = [];

  public static Map Map { get; set; } = new(100, 100);

  public static int Seed { get; } = new Random().Next();

  public static bool DebugMode { get; set; } = false;

  public static void Initialize()
  {
    // Initialize collision system
    CollisionsManager.Initialize();

    // Subscribe to collision events
    CollisionsManager.OnCollisionEnter += OnCollisionEnter;
    CollisionsManager.OnCollisionExit += OnCollisionExit;

    Player.Position = new Vector2(160, 160);

    Map.GenerateRandomMap();
    GameObjects.Add(Map);
    GameObjects.Add(Player);

    // Register player with collision system
    CollisionsManager.AddObject(Player);

    foreach (var obj in GameObjects.ToArray())
    {
      obj.Initialize();
    }
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

    // Update collision system
    CollisionsManager.Update(deltaTime);
    foreach (var obj in GameObjects)
    {
      if (obj != Map && obj != Player)
      {
        if (obj.Visible || ShouldAlwaysUpdate(obj))
        {
          obj.Update(deltaTime);
        }
      }
    }
  }

  public static void DrawVisibleObjects()
  {
    Map.Draw();
    foreach (var obj in GameObjects)
    {
      if (obj != Map && (obj.Visible || obj == Player))
      {
        obj.Draw();
      }
    }
  }


  public static void AddGameObject(GameObject gameObject)
  {
    if (gameObject == null) return;
    GameObjects.Add(gameObject);
    if (gameObject.Collider != null)
    {
      CollisionsManager.AddObject(gameObject);
    }
    gameObject.Initialize();
  }

  private static bool ShouldAlwaysUpdate(GameObject obj)
  {
    // Add logic here for objects that need constant updates (AI, physics, etc.)
    return obj is Player;
  }
}