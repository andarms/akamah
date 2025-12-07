using Akamah.Engine.Systems;
using Akamah.Engine.Common;
using Akamah.Engine.Core;
using Akamah.Engine.Assets;
using Akamah.Engine.Collisions;

namespace Akamah.Engine.Entities;


public class Player : GameObject
{
  const float Speed = 100.0f;

  Direction facing = Direction.Down;
  bool flipTexture = false;

  readonly Cursor cursor = new();


  public Player()
  {
    Collider = new Collider
    {
      Size = new Vector2(12, 12),
      Offset = new Vector2(2, 4)
    };
  }

  public override void Initialize()
  {
    base.Initialize();
    GameManager.AddGameObject(cursor);
  }

  public override void Update(float deltaTime)
  {
    base.Update(deltaTime);

    // Get input movement using InputManager
    Vector2 movement = Vector2.Zero;
    if (InputManager.IsHold("move_left"))
    {
      movement.X -= 1;
      facing = Direction.Left;
      flipTexture = true;
    }
    if (InputManager.IsHold("move_right"))
    {
      movement.X += 1;
      facing = Direction.Right;
      flipTexture = false;
    }
    if (InputManager.IsHold("move_up"))
    {
      movement.Y -= 1;
      facing = Direction.Up;
    }
    if (InputManager.IsHold("move_down"))
    {
      movement.Y += 1;
      facing = Direction.Down;
    }


    // Apply movement with collision detection per axis
    Vector2 velocity = movement * Speed * deltaTime;
    MoveWithCollisionDetection(velocity);

    cursor.Position = Position + facing.ToVector2() * 16;
    var c = CollisionsManager.GetPotentialCollisions(cursor);
    cursor.Colliding = c.Any();
    cursor.Position = c.FirstOrDefault()?.Position ?? cursor.Position;

    ViewportManager.UpdateTarget(Position);
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

    // Create source rectangle with proper flipping
    Rectangle sourceRect = new Rectangle(16, 112, flipTexture ? -16 : 16, 16);

    DrawTexturePro(
      AssetsManager.Textures["TinyDungeon"],
      sourceRect,
      new Rectangle(Position.X, Position.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );
  }
}