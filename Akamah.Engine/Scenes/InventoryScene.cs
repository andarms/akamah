using Akamah.Engine.Engine.Input;
using Akamah.Engine.Engine.Scene;

namespace Akamah.Engine.Scenes;

public class InventoryScene : Scene
{
  Color backgroundColor = Fade(Color.Gray, 0.5f);
  public InventoryScene()
  {
  }

  public override void HandleInput()
  {
    // base.Update(gameTime);
    if (InputSystem.IsPressed("inventory"))
    {
      SceneController.PopScene();
    }
  }

  public override void Draw()
  {
    DrawRectangleV(new(0, 0), new(12800, 720), backgroundColor);
  }
}