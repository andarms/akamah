using Akamah.Engine.Scenes;

namespace Akamah.Engine.Managers;

public static class GameManager
{
  public static Player Player { get; set; } = new();

  public static Map Map { get; set; } = new(100, 100);
}