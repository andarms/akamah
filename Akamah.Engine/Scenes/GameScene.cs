using Akamah.Engine.Managers;

namespace Akamah.Engine.Scenes;


public class GameScene : Scene
{
  public override void Initialize()
  {
    base.Initialize();
    GameManager.Map.GenerateRandomMap();
    GameObjects.Add(GameManager.Map);
    GameObjects.Add(GameManager.Player);
  }



  public override void HandleInput()
  {
    if (IsKeyPressed(KeyboardKey.R))
    {
      GameManager.Map.GenerateRandomMap();
    }
  }

  public override void Unload()
  {
    base.Unload();
  }


  public override void Update(float deltaTime)
  {
    base.Update(deltaTime);
  }

  public override void Draw()
  {
    BeginMode2D(ViewportManager.Camera);
    base.Draw();
    EndMode2D();

    DrawFPS(10, 10);
  }
}