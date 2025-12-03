using Akamah.Engine.Scenes;

namespace Akamah.Engine.Managers;

public static class GameManager
{
  public static Player Player { get; set; } = new();

  public static List<GameObject> GameObjects { get; set; } = [];

  public static Map Map { get; set; } = new(200, 200);

  public static int Seed { get; } = new Random().Next();

  public static void Initialize()
  {
    Player.Position = new Vector2(160, 160);
    Map.GenerateRandomMap();
    GameObjects.Add(Map);
    GameObjects.Add(Player);

    foreach (var obj in GameObjects.ToArray())
    {
      obj.Initialize();
    }
  }

  public static void UpdateVisibleObjects(float deltaTime)
  {
    // Always update the player
    Player.Update(deltaTime);

    // Update the map (it has its own internal culling)
    Map.Update(deltaTime);
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


  private static bool ShouldAlwaysUpdate(GameObject obj)
  {
    // Add logic here for objects that need constant updates (AI, physics, etc.)
    return obj is Player;
  }
}