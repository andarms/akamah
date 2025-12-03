namespace Akamah.Engine.Scenes;

public class MountainTile : Tile
{
  public override TileType Type { get; } = TileType.Mountain;

  public override void Draw()
  {
    base.Draw();
    DrawTexturePro(
      AssetsManager.Textures["Desert"],
      new Rectangle(16, 48, 16, 16),
      new Rectangle(Position.X, Position.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
    DrawTexturePro(
      AssetsManager.Textures["Desert"],
      new Rectangle(64, 48, 16, 16),
      new Rectangle(Position.X, Position.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
  }
}
