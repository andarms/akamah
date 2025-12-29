using Akamah.Engine.Engine.Input;
using Akamah.Engine.Engine.Scene;
using Akamah.Engine.Gameplay.Inventories;
using Akamah.Engine.UserInterface;

namespace Akamah.Engine.Scenes;

public class InventoryScene : Scene
{
  readonly InventoryPanel panel = new();

  public InventoryScene()
  {

  }

  public override void OnEnter()
  {
    base.OnEnter();
    Canvas.Add(panel, Anchor.Center);
  }

  public override void OnExit()
  {
    base.OnExit();
    Canvas.Remove(panel);
  }

  public override void HandleInput()
  {
    // base.Update(gameTime);
    if (InputSystem.IsPressed("inventory"))
    {
      SceneController.PopScene();
    }
  }
}