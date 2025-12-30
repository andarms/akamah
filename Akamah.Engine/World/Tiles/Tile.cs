using Akamah.Engine.Engine.Camera;
using Akamah.Engine.Engine.Core;

namespace Akamah.Engine.World.Tiles;

public abstract class Tile : GameObject
{
  public virtual TileType Type { get; } = TileType.None;

  protected override bool IsInCameraView()
  {
    // Use rectangle-based visibility check for tiles (16x16 size)
    return Game.Viewport.IsRectInView(Position, new Vector2(16, 16));
  }

  public override void Draw()
  {
    if (!IsInCameraView())
    {
      Visible = false;
      return;
    }

    Visible = true;
    base.Draw();
  }

}
