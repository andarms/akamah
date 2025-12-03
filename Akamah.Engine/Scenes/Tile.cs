using Akamah.Engine.Managers;

namespace Akamah.Engine.Scenes;

public abstract class Tile : GameObject
{
  public virtual TileType Type { get; } = TileType.None;

  protected override bool IsInCameraView()
  {
    // Use rectangle-based visibility check for tiles (16x16 size)
    return ViewportManager.IsRectInView(Position, new Vector2(16, 16));
  }

  public override void Draw()
  {
    // Call base visibility check first
    if (!IsInCameraView())
    {
      Visible = false;
      return;
    }

    Visible = true;
    DrawRectangleV(Position, new Vector2(16), Color.Black);
  }

  public virtual List<(TileType type, float weight)> ValidNeighbors()
  {
    return
    [
      (TileType.Grass, 1.0f),
      (TileType.Forest, 1.0f),
      (TileType.Sand, 1.0f),
      (TileType.Mountain, 1.0f),
      (TileType.Water, 1.0f)
    ];
  }
}
