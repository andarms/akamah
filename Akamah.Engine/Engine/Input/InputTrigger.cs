using Akamah.Engine.Engine.Input;

namespace Akamah.Engine.Core.Input;

public readonly struct InputTrigger
{
  public InputType Type { get; init; }
  public KeyboardKey? Key { get; init; }
  public MouseButton? MouseButton { get; init; }

  public static InputTrigger FromKey(KeyboardKey key) => new()
  {
    Type = InputType.Keyboard,
    Key = key
  };

  public static InputTrigger FromMouse(MouseButton button) => new()
  {
    Type = InputType.Mouse,
    MouseButton = button
  };
}
