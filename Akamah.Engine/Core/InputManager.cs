namespace Akamah.Engine.Core;

public static class InputManager
{
  private static readonly Dictionary<string, KeyboardKey[]> actionKeys = new();
  private static readonly Dictionary<string, bool> previousFrameState = new();

  public static void MapAction(string actionName, params KeyboardKey[] keys)
  {
    actionKeys[actionName] = keys;
    previousFrameState[actionName] = false;
  }

  public static bool IsPressed(string actionName)
  {
    if (!actionKeys.TryGetValue(actionName, out KeyboardKey[]? actions)) return false;
    foreach (var key in actions)
    {
      if (IsKeyPressed(key)) return true;
    }
    return false;
  }

  public static bool IsHold(string actionName)
  {
    if (!actionKeys.ContainsKey(actionName))
      return false;

    foreach (var key in actionKeys[actionName])
    {
      if (IsKeyDown(key))
        return true;
    }
    return false;
  }

  public static void Update()
  {
    foreach (var action in actionKeys.Keys)
    {
      previousFrameState[action] = IsHold(action);
    }
  }
}