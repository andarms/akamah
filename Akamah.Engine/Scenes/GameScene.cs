namespace Akamah.Engine.Scenes;


public class GameScene : Scene
{
  readonly Map map = new(80, 45);

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
}