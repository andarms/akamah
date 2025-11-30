namespace Akamah.Engine.Scenes;


public class GameScene : Scene
{
  readonly Map map = new(100, 100);
  Camera2D camera = new()
  {
    Target = new Vector2(0, 0),
    Offset = new Vector2(0, 0),
    Rotation = 0.0f,
    Zoom = 3.0f
  };

  public override void Initialize()
  {
    base.Initialize();
    map.GenerateRandomMap();
    GameObjects.Add(map);
  }



  public override void HandleInput()
  {
    if (IsKeyPressed(KeyboardKey.R))
    {
      map.GenerateRandomMap();
    }
  }

  public override void Unload()
  {
    base.Unload();
  }


  public override void Update(float deltaTime)
  {
    base.Update(deltaTime);
    //  user input to move camera
    Vector2 movement = Vector2.Zero;
    if (IsKeyDown(KeyboardKey.Up))
      movement.Y -= 200 * deltaTime;
    if (IsKeyDown(KeyboardKey.Down))
      movement.Y += 200 * deltaTime;
    if (IsKeyDown(KeyboardKey.Left))
      movement.X -= 200 * deltaTime;
    if (IsKeyDown(KeyboardKey.Right))
      movement.X += 200 * deltaTime;


    camera.Target += movement;
    camera.Target = Vector2.Clamp(camera.Target, Vector2.Zero, new Vector2(74 * 16, 85 * 16));
  }

  public override void Draw()
  {
    BeginMode2D(camera);
    base.Draw();
    EndMode2D();
  }
}