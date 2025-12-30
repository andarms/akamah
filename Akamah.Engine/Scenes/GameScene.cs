using Akamah.Engine.Engine.Camera;
using Akamah.Engine.Engine.Core;
using Akamah.Engine.Engine.Input;
using Akamah.Engine.Engine.Physics.Spatial;
using Akamah.Engine.Engine.Scenes;
using Akamah.Engine.Gameplay.Inventories;
using Akamah.Engine.Systems.Collision;
using Akamah.Engine.UserInterface;
using Akamah.Engine.World;

namespace Akamah.Engine.Scenes;

public class GameScene : Scene
{

  public override void Initialize()
  {

    base.Initialize();
    objects.Add(Game.Map);
    // objects.Add(Game.Player);
    Game.Player.Position = new Vector2(160, 160);
    Game.Map.GenerateRandomMap();
    SpatialSystem.AddObject(Game.Player);
    if (Game.Player.Collider != null)
    {
      CollisionsManager.AddObject(Game.Player);
    }

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

    // canvas.Add(new Toolbar(), Anchor.BottomCenter, new Vector2(0, -16));
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
          SpatialSystem.RemoveObject(obj);
          CollisionsManager.RemoveObject(obj);
        }
      }
      Game.Map.GenerateRandomMap();
      objects.Add(Game.Map);
      SpatialSystem.AddObject(Game.Player);
      objects.Add(Game.Player);
      if (Game.Player.Collider != null)
      {
        CollisionsManager.AddObject(Game.Player);
      }
    }
    if (IsKeyPressed(KeyboardKey.I))
    {
      Game.Scenes.Push<InventoryScene>();
    }

    if (IsKeyPressed(KeyboardKey.F1))
    {
      Game.DebugMode = !Game.DebugMode;
    }
  }

  protected override void UpdateWorld(float deltaTime)
  {
    UpdateVisibleObjects(deltaTime);
  }

  protected override void DrawWorld()
  {
    DrawVisibleObjects();
  }

  public void UpdateVisibleObjects(float deltaTime)
  {
    // Always update the player
    Game.Player.Update(deltaTime);
    Game.Map.Update(deltaTime);

    CollisionsManager.Update(deltaTime);


    var (viewportTopLeft, viewportBottomRight) = ViewportManager.CameraViewport;
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
    var (viewportTopLeft, viewportBottomRight) = ViewportManager.CameraViewport;
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

    // Always draw the player if not already in visible objects
    if (!visibleObjects.Contains(Game.Player))
    {
      Game.Player.Draw();
      if (Game.DebugMode)
      {
        Game.Player.Debug();
      }
    }
  }
}