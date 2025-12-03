using Akamah.Engine;
using Akamah.Engine.Managers;

namespace Akamah.Engine.Scenes;

public class Player : GameObject
{
  const float Speed = 100.0f;

  public Player()
  {
    Collider = new Collider
    {
      Size = new Vector2(8, 8),
      Offset = new Vector2(4, 8)
    };
  }

  public override void Update(float deltaTime)
  {
    base.Update(deltaTime);

    // Get input movement
    Vector2 movement = Vector2.Zero;
    if (IsKeyDown(KeyboardKey.Up)) { movement.Y -= 1; }
    if (IsKeyDown(KeyboardKey.Down)) { movement.Y += 1; }
    if (IsKeyDown(KeyboardKey.Left)) { movement.X -= 1; }
    if (IsKeyDown(KeyboardKey.Right)) { movement.X += 1; }

    // Apply movement with collision detection per axis
    Vector2 velocity = movement * Speed * deltaTime;
    MoveWithCollisionDetection(velocity);

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
      if (collision is Tree) // Trees are solid objects
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

    DrawTexturePro(
      AssetsManager.Textures["TinyDungeon"],
      new Rectangle(16, 112, 16, 16),
      new Rectangle(Position.X, Position.Y, 16, 16),
      new Vector2(0, 0),
      0.0f,
      Color.White
    );

    if (Collider != null && GameManager.DebugMode)
    {
      DrawRectangleV(Position + Collider.Offset, Collider.Size, Collider.DebugColor);
    }
  }
}