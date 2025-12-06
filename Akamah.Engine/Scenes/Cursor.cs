namespace Akamah.Engine.Scenes;

public class Cursor : GameObject
{

  public override void Draw()
  {
    base.Draw();
    DrawTexturePro(
      AssetsManager.Textures["TinyDungeon"],
      new Rectangle(0, 80, 16, 16),
      new Rectangle(Position.X, Position.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
  }
}
