using Akamah.Engine;

public class Player : GameObject
{
  const float Speed = 48.0f;

  public override void Update(float deltaTime)
  {
    base.Update(deltaTime);

    //  user input to move camera
    Vector2 movement = Vector2.Zero;
    if (IsKeyDown(KeyboardKey.Up))
      movement.Y -= 1;
    if (IsKeyDown(KeyboardKey.Down))
      movement.Y += 1;
    if (IsKeyDown(KeyboardKey.Left))
      movement.X -= 1;
    if (IsKeyDown(KeyboardKey.Right))
      movement.X += 1;
    Position += movement * Speed * deltaTime;
  }

  public override void Draw()
  {
    DrawTexturePro(
      AssetsManager.Textures["TinyDungeon"],
      new Rectangle(16, 112, 16, 16),
      new Rectangle(Position.X, Position.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
  }
}