using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.UserInterface;

public class UIObject : GameObject
{
  public bool IsMouseHOver()
  {
    Vector2 cursor = GetMousePosition();
    Rectangle rect = GetBounds();
    return CheckCollisionPointRec(cursor, rect);
  }

  public bool IsMouseLClicked()
  {
    return IsMouseHOver() && IsMouseButtonPressed(MouseButton.Left);
  }
}