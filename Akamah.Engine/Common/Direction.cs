namespace Akamah.Engine.Common;

public enum Direction
{
  Up,
  Down,
  Left,
  Right
}


public static class DirectionExtensions
{
  public static Direction Opposite(this Direction direction)
  {
    return direction switch
    {
      Direction.Up => Direction.Down,
      Direction.Down => Direction.Up,
      Direction.Left => Direction.Right,
      Direction.Right => Direction.Left,
      _ => direction
    };
  }

  public static Vector2 ToVector2(this Direction direction)
  {
    return direction switch
    {
      Direction.Up => new Vector2(0, -1),
      Direction.Down => new Vector2(0, 1),
      Direction.Left => new Vector2(-1, 0),
      Direction.Right => new Vector2(1, 0),
      _ => Vector2.Zero
    };
  }
}