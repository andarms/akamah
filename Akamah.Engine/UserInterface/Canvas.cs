using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.UserInterface;

public enum Anchor
{
  TopLeft,
  TopRight,
  BottomLeft,
  BottomRight,
  Center,
  TopCenter,
  BottomCenter,
}

public static class Canvas
{
  static readonly List<GameObject> objects = [];

  public static void Add(GameObject obj, Anchor anchor, Vector2 offset)
  {
    Vector2 position = CalculatePosition(obj, anchor, offset);
    obj.Position = position;
    objects.Add(obj);
  }

  public static void Add(GameObject obj, Anchor anchor)
  {
    Vector2 position = CalculatePosition(obj, anchor, new Vector2(0));
    obj.Position = position;
    objects.Add(obj);
  }

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


  private static Vector2 CalculatePosition(GameObject obj, Anchor anchor, Vector2 offset)
  {
    float x = 0;
    float y = 0;
    Rectangle bounds = obj.GetBounds();

    switch (anchor)
    {
      case Anchor.TopLeft:
        x = 0 + offset.X;
        y = 0 + offset.Y;
        break;
      case Anchor.TopRight:
        x = Setting.SCREEN_WIDTH - bounds.Width + offset.X;
        y = 0 + offset.Y;
        break;
      case Anchor.BottomLeft:
        x = 0 + offset.X;
        y = Setting.SCREEN_HEIGHT - bounds.Height + offset.Y;
        break;
      case Anchor.BottomRight:
        x = Setting.SCREEN_WIDTH - bounds.Width + offset.X;
        y = Setting.SCREEN_HEIGHT - bounds.Height + offset.Y;
        break;
      case Anchor.Center:
        x = (Setting.SCREEN_WIDTH - bounds.Width) / 2 + offset.X;
        y = (Setting.SCREEN_HEIGHT - bounds.Height) / 2 + offset.Y;
        break;
      case Anchor.TopCenter:
        x = (Setting.SCREEN_WIDTH - bounds.Width) / 2 + offset.X;
        y = 0 + offset.Y;
        break;
      case Anchor.BottomCenter:
        x = (Setting.SCREEN_WIDTH - bounds.Width) / 2 + offset.X;
        y = Setting.SCREEN_HEIGHT - bounds.Height + offset.Y;
        break;
      default:
        break;
    }

    return new Vector2(x, y);
  }
}