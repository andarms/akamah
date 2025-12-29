using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.Engine.Scene;

public class GameObjectsCollection
{
  private readonly List<GameObject> gameObjects = [];

  public IEnumerable<GameObject> ToArray() => [.. gameObjects];

  public void Add(GameObject gameObject)
  {
    gameObjects.Add(gameObject);
  }

  public void Remove(GameObject gameObject)
  {
    gameObjects.Remove(gameObject);
  }

  public IEnumerator<GameObject> GetEnumerator()
  {
    return gameObjects.GetEnumerator();
  }
}
