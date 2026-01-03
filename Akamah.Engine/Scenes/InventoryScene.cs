using Akamah.Engine.Engine.Core;
using Akamah.Engine.Engine.Input;
using Akamah.Engine.Engine.Scenes;
using Akamah.Engine.Gameplay.Inventories;
using Akamah.Engine.UserInterface;

namespace Akamah.Engine.Scenes;

public class InventoryScene : Scene
{
  public override void OnEnter()
  {
    base.OnEnter();
    // inventory = new InventoryUI();
    // AddUI(inventory);
    AddUI(Game.Inventory);
    AddUI(Game.Toolbar);
    Game.Toolbar.Position = Canvas.CalculatePosition(
      Game.Toolbar,
      Anchor.BottomCenter,
      new Vector2(0, -16)
    );
  }

  public override void OnExit()
  {
    base.OnExit();
    // RemoveUI(inventory);
    RemoveUI(Game.Inventory);
    RemoveUI(Game.Toolbar);
  }

  public override void HandleInput()
  {
    if (InputSystem.IsPressed("inventory"))
    {
      Game.Scenes.Pop();
    }
  }
}