namespace Akamah.Engine.Scenes;

public class Tree : GameObject
{
  public override void Draw()
  {
    DrawTexturePro(
      AssetsManager.Textures["TinyTown"],
      new Rectangle(64, 32, 16, 16),
      new Rectangle(Position.X, Position.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
  }
}
