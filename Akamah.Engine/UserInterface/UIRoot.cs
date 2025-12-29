using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.UserInterface;

public class UIRoot : GameObject
{
  public override void Draw()
  {
    base.Draw();
    // Draw UI elements here
  }
}


public static class Canvas
{
  static readonly List<GameObject> objects = [];

  public static void Add(GameObject obj)
  {
    objects.Add(obj);
  }

  public static void Remove(GameObject obj)
  {
    objects.Remove(obj);
  }

  public static void Clear()
  {
    objects.Clear();
  }

  public static void Update(float deltaTime)
  {
    foreach (var obj in objects)
    {
      obj.Update(deltaTime);
    }
  }


  public static void Draw()
  {
    foreach (var obj in objects)
    {
      obj.Draw();
    }
  }
}