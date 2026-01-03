using Akamah.Engine.Engine.Camera;
using Akamah.Engine.Engine.Core;
using Akamah.Engine.Engine.Input;
using Akamah.Engine.Engine.Physics.Spatial;
using Akamah.Engine.Engine.Scenes;
using Akamah.Engine.Gameplay.Equipment;
using Akamah.Engine.Gameplay.Inventories;
using Akamah.Engine.Gameplay.Inventories.Items;
using Akamah.Engine.Systems.Collision;
using Akamah.Engine.UserInterface;
using Akamah.Engine.World;

namespace Akamah.Engine.Scenes;

public class GameScene : Scene
{

  public override void Initialize()
  {

    base.Initialize();
    Game.Add(Game.Map);
    Game.Add(Game.Player);
    Game.Player.Position = new Vector2(160, 160);
    Game.Map.GenerateRandomMap();

    var item = new Collectable(new Stone())
    {
      Position = new Vector2(200, 200)
    };
    Game.Add(item);



    InputSystem.MapAction("move_left", KeyboardKey.Left, KeyboardKey.A);
    InputSystem.MapAction("move_right", KeyboardKey.Right, KeyboardKey.D);
    InputSystem.MapAction("move_up", KeyboardKey.Up, KeyboardKey.W);
    InputSystem.MapAction("move_down", KeyboardKey.Down, KeyboardKey.S);
    InputSystem.MapAction("inventory", KeyboardKey.I);

    // Example of mapping attack to both keyboard and mouse inputs
    InputSystem.MapAction(
      "attack",
      [KeyboardKey.Space, KeyboardKey.Enter],
      [MouseButton.Left]
    );

    // Game.Inventory.Visible = false;
    Game.Inventory.Initialize();
    // AddUI(Game.Inventory);


    Game.Toolbar.Position = Canvas.CalculatePosition(
      Game.Toolbar,
      Anchor.BottomCenter,
      new Vector2(0, -16)
    );
    ui.Add(Game.Toolbar);

  }

  public override void HandleInput()
  {
    if (IsKeyPressed(KeyboardKey.R))
    {
      foreach (var obj in objects.ToArray())
      {
        if (obj != Game.Map && obj != Game.Player)
        {
          objects.Remove(obj);
          SpatialSystem.Remove(obj);
          CollisionsManager.Remove(obj);
        }
      }
      Game.Map.GenerateRandomMap();
    }
    if (IsKeyPressed(KeyboardKey.I))
    {
      Game.Scenes.Push<InventoryScene>();
      // Game.Inventory.Visible = !Game.Inventory.Visible;
    }

    if (IsKeyPressed(KeyboardKey.F1))
    {
      Game.DebugMode = !Game.DebugMode;
    }
  }

  protected override void UpdateWorld(float deltaTime)
  {
    base.UpdateWorld(deltaTime);
    UpdateVisibleObjects(deltaTime);
  }

  protected override void DrawWorld()
  {
    base.DrawWorld(); // Call base to draw all objects including DamageIndicators
    DrawVisibleObjects();
  }

  public void UpdateVisibleObjects(float deltaTime)
  {
    CollisionsManager.Update(deltaTime);


    var (viewportTopLeft, viewportBottomRight) = Game.Viewport.Area;
    var visibleObjects = SpatialSystem.GetVisibleObjects(viewportTopLeft, viewportBottomRight);

    // Update only visible objects (except special cases)
    foreach (var obj in visibleObjects)
    {
      if (obj != Game.Map && obj != Game.Player)
      {
        obj.Update(deltaTime);
      }
    }
  }

  public void DrawVisibleObjects()
  {
    // Draw the map first (it has its own culling)
    Game.Map.Draw();

    // Get visible objects from spatial system
    var (viewportTopLeft, viewportBottomRight) = Game.Viewport.Area;
    var visibleObjects = SpatialSystem.GetVisibleObjects(viewportTopLeft, viewportBottomRight).ToList();

    // Sort visible objects by layer and then by Y position for proper rendering order
    visibleObjects.Sort((a, b) =>
    {
      int layerComparison = a.Layer.CompareTo(b.Layer);
      if (layerComparison != 0) return layerComparison;
      return a.Position.Y.CompareTo(b.Position.Y);
    });

    // Draw visible objects (excluding map since it's already drawn)
    foreach (var obj in visibleObjects)
    {
      if (obj != Game.Map)
      {
        obj.Draw();
        if (Game.DebugMode)
        {
          obj.Debug();
        }
      }
    }
  }
}