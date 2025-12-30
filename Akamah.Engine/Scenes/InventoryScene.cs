using Akamah.Engine.Engine.Core;
using Akamah.Engine.Engine.Input;
using Akamah.Engine.Engine.Scenes;
using Akamah.Engine.Gameplay.Inventories;
using Akamah.Engine.UserInterface;

namespace Akamah.Engine.Scenes;

public class InventoryScene : Scene
{
  readonly InventoryPanel panel = new();


  public override Color BackgroundColor => Fade(Color.Black, 0.5f);

  public InventoryScene()
  {
  }

  public override void OnEnter()
  {
    base.OnEnter();
    panel.Position = Canvas.CalculatePosition(panel, Anchor.Center, Vector2.Zero);
    ui.Add(panel);
  }

  public override void OnExit()
  {
    base.OnExit();
    ui.Remove(panel);
  }

  public override void HandleInput()
  {
    if (InputSystem.IsPressed("inventory"))
    {
      Game.Scenes.Pop();
    }
  }
}