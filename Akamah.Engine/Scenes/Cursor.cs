namespace Akamah.Engine.Scenes;

public class Cursor : GameObject
{

  public bool Colliding { get; set; } = false;
  public Cursor()
  {
    Collider = new Collider
    {
      Size = new Vector2(16),
      Offset = new Vector2(0)
    };
  }

  Color normalColor = Color.Yellow;
  Color collidingColor = Color.Red;

  public override void Draw()
  {
    base.Draw();
    DrawTexturePro(
      AssetsManager.Textures["TinyDungeon"],
      new Rectangle(0, 80, 16, 16),
      new Rectangle(Position.X, Position.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Colliding ? collidingColor : normalColor
    );
  }
}
