using Akamah.Engine.Scenes;

namespace Akamah.Engine.Managers;

public static class GameManager
{
  public static Player Player { get; set; } = new();

  public static List<GameObject> GameObjects { get; set; } = [];

  public static Map Map { get; set; } = new(100, 100);

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
}