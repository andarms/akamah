using Akamah.Engine.Engine.Core;
using Akamah.Engine.Engine.Input;
using Akamah.Engine.Engine.Scenes;
using Akamah.Engine.Gameplay.Inventories;
using Akamah.Engine.UserInterface;

namespace Akamah.Engine.Scenes;

public class InventoryScene : Scene
{
  InventoryUI inventory = new();

  public override void OnEnter()
  {
    base.OnEnter();
    inventory = new InventoryUI();
    AddUI(inventory);
  }

  public override void OnExit()
  {
    base.OnExit();
    RemoveUI(inventory);
  }

  public override void HandleInput()
  {
    if (InputSystem.IsPressed("inventory"))
    {
      Game.Scenes.Pop();
    }
  }
}