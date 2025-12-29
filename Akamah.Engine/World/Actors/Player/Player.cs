using Akamah.Engine.Assets;
using Akamah.Engine.Engine.Camera;
using Akamah.Engine.Engine.Core;
using Akamah.Engine.Engine.Input;
using Akamah.Engine.Gameplay.Equipment;
using Akamah.Engine.Gameplay.Interactions.Equipment;
using Akamah.Engine.Gameplay.Inventories;
using Akamah.Engine.Systems.Collision;

namespace Akamah.Engine.World.Actors.Player;

public class Player : GameObject
{
  const float Speed = 100.0f;
  readonly Weapon weapon = new();
  Vector2 weaponOffset = new(4, -2);

  public Tool Tool { get; private set; } = ToolsFactory.CreateSword(Gameplay.Equipment.Material.Wood);

  public Player()
  {
    Anchor = new(8, 16);
    Collider = new Collider
    {
      Size = new Vector2(16, 8),
      Offset = new Vector2(0, 8)
    };
    Add(new Inventory(20));
    AddChild(weapon);
    Console.Write(Children.Count + " children");
  }

  public override void Initialize()
  {
    base.Initialize();
    // GameManager.AddGameObject(cursor);
    // GameWorld.AddGameObject(weapon);
  }

  public override void Update(float deltaTime)
  {
    base.Update(deltaTime);

    // Get input movement using InputManager
    Vector2 movement = Vector2.Zero;
    if (InputSystem.IsHold("move_left"))
    {
      movement.X -= 1;
    }
    if (InputSystem.IsHold("move_right"))
    {
      movement.X += 1;
    }
    if (InputSystem.IsHold("move_up"))
    {
      movement.Y -= 1;
    }
    if (InputSystem.IsHold("move_down"))
    {
      movement.Y += 1;
    }

    if (InputSystem.IsPressed("attack"))
    {
      weapon.TryAttack();
    }

    if (IsKeyPressed(KeyboardKey.One))
    {
      Tool = ToolsFactory.CreateSword(Gameplay.Equipment.Material.Wood);
      Console.WriteLine($"Equipped: {Tool}");
    }
    else if (IsKeyPressed(KeyboardKey.Two))
    {
      Tool = ToolsFactory.CreateAxe(Gameplay.Equipment.Material.Stone);
      Console.WriteLine($"Equipped: {Tool}");
    }
    else if (IsKeyPressed(KeyboardKey.Three))
    {
      Tool = ToolsFactory.CreatePickaxe(Gameplay.Equipment.Material.Wood);
      Console.WriteLine($"Equipped: {Tool}");
    }
    else if (IsKeyPressed(KeyboardKey.Four))
    {
      Tool = ToolsFactory.CreateShovel(Gameplay.Equipment.Material.Stone);
      Console.WriteLine($"Equipped: {Tool}");
    }

    // Apply movement with collision detection per axis
    Vector2 velocity = movement * Speed * deltaTime;
    MoveWithCollisionDetection(velocity);
    ViewportManager.UpdateTarget(Position);


    var cursorPosition = GetScreenToWorld2D(GetMousePosition(), ViewportManager.Camera);
    Vector2 direction = cursorPosition - Position;

    // Update player orientation and weapon position based on mouse direction with deadzone
    const float deadzone = 8f;
    Vector2 currentWeaponOffset = weaponOffset;

    // Only update orientation if mouse is outside deadzone
    if (direction.LengthSquared() > deadzone * deadzone)
    {
      // Basic face direction based on mouse X position relative to player position
      bool shouldFaceRight = cursorPosition.X > Position.X;

      FlipX = !shouldFaceRight;
      currentWeaponOffset = shouldFaceRight ? new Vector2(4, -2) : new Vector2(-4, -2);
    }
    // If within deadzone, keep current flip state and weapon position

    // Update weapon position using calculated offset
    weapon.UpdatePosition(Position + currentWeaponOffset);

  }

  private void MoveWithCollisionDetection(Vector2 velocity)
  {
    if (Collider == null) return;

    Vector2 originalPosition = Position;

    // Try moving horizontally first
    Position = new Vector2(originalPosition.X + velocity.X, originalPosition.Y);
    GameObject? solidCollision = GetSolidCollision();
    if (solidCollision != null)
    {
      CollisionsManager.ResolveSolidCollision(this, solidCollision, true, false);
    }

    // Then try moving vertically
    Vector2 currentPosition = Position;
    Position = new Vector2(currentPosition.X, currentPosition.Y + velocity.Y);
    solidCollision = GetSolidCollision();
    if (solidCollision != null)
    {
      CollisionsManager.ResolveSolidCollision(this, solidCollision, false, true);
    }
  }

  private GameObject? GetSolidCollision()
  {
    if (Collider == null) return null;

    foreach (var collision in CollisionsManager.GetPotentialCollisions(this))
    {
      if (collision.Collider != null && collision.Collider.Solid)
      {
        return collision;
      }
    }
    return null;
  }

  private bool HasCollisionWithSolidObject()
  {
    return GetSolidCollision() != null;
  }

  protected override bool IsInCameraView()
  {
    // Player should almost always be visible (around camera center)
    // But still check for edge cases
    return ViewportManager.IsRectInView(Position, new Vector2(16, 16));
  }

  public override void Draw()
  {
    // Player is usually always visible but still check for edge cases
    if (!IsInCameraView())
    {
      Visible = false;
      return;
    }

    Visible = true;
    Rectangle sourceRect = new(16, 112, FlipX ? -16 : 16, 16);
    DrawTexturePro(
      AssetsManager.Textures["TinyDungeon"],
      sourceRect,
      new Rectangle(RenderPosition.X, RenderPosition.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
    base.Draw();
  }
}