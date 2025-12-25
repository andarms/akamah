using Akamah.Engine.Core.Input;

namespace Akamah.Engine.Engine.Input;

public enum InputType
{
  Keyboard,
  Mouse
}

public static class InputSystem
{
  private static readonly Dictionary<string, InputTrigger[]> actionTriggers = new();
  private static readonly Dictionary<string, bool> previousFrameState = new();

  public static void MapAction(string actionName, params InputTrigger[] triggers)
  {
    actionTriggers[actionName] = triggers;
    previousFrameState[actionName] = false;
  }

  public static void MapAction(string actionName, params KeyboardKey[] keys)
  {
    var triggers = keys.Select(InputTrigger.FromKey).ToArray();
    MapAction(actionName, triggers);
  }

  public static void MapAction(string actionName, params MouseButton[] buttons)
  {
    var triggers = buttons.Select(InputTrigger.FromMouse).ToArray();
    MapAction(actionName, triggers);
  }

  public static void MapAction(string actionName, KeyboardKey[] keys, MouseButton[] buttons)
  {
    var keyTriggers = keys.Select(InputTrigger.FromKey);
    var mouseTriggers = buttons.Select(InputTrigger.FromMouse);
    var allTriggers = keyTriggers.Concat(mouseTriggers).ToArray();
    MapAction(actionName, allTriggers);
  }

  public static bool IsPressed(string actionName)
  {
    if (!actionTriggers.TryGetValue(actionName, out InputTrigger[]? triggers)) return false;

    foreach (var trigger in triggers)
    {
      switch (trigger.Type)
      {
        case InputType.Keyboard when trigger.Key.HasValue:
          if (IsKeyPressed(trigger.Key.Value)) return true;
          break;
        case InputType.Mouse when trigger.MouseButton.HasValue:
          if (IsMouseButtonPressed(trigger.MouseButton.Value)) return true;
          break;
      }
    }
    return false;
  }

  public static bool IsHold(string actionName)
  {
    if (!actionTriggers.TryGetValue(actionName, out InputTrigger[]? value)) return false;

    foreach (var trigger in value)
    {
      switch (trigger.Type)
      {
        case InputType.Keyboard when trigger.Key.HasValue:
          if (IsKeyDown(trigger.Key.Value)) return true;
          break;
        case InputType.Mouse when trigger.MouseButton.HasValue:
          if (IsMouseButtonDown(trigger.MouseButton.Value)) return true;
          break;
      }
    }
    return false;
  }

  public static void Update()
  {
    foreach (var action in actionTriggers.Keys)
    {
      previousFrameState[action] = IsHold(action);
    }
  }
}