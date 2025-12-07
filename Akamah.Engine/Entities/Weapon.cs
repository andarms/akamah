using Akamah.Engine.Assets;
using Akamah.Engine.Collisions;
using Akamah.Engine.Core;
using Akamah.Engine.Systems;

namespace Akamah.Engine.Entities;

public class Weapon : GameObject
{
  // Configuration constants
  private const float DefaultRotationSpeed = 1800f; // Even faster animation (degrees per second)
  private const float DefaultSwingDuration = 0.3f; // Shorter swing duration

  // State variables
  private bool isActive = false;

  // Rotation and animation
  private float currentRotation = 0f;
  private float targetRotation = 0f;
  private float rotationSpeed = DefaultRotationSpeed;
  private bool isSwinging = false;
  private float swingDuration = DefaultSwingDuration;
  private float swingTimer = 0f;

  public Weapon()
  {
    InitializeWeapon();
  }

  private void InitializeWeapon()
  {
    Anchor = new Vector2(8, 16);
    Collider = new Collider
    {
      Size = new Vector2(16),
      Offset = Vector2.Zero
    };
    Visible = true;
    currentRotation = 0f; // Start pointing down
  }

  /// <summary>
  /// Initiates an attack. Can be called repeatedly once swing finishes.
  /// </summary>
  public bool TryAttack()
  {
    // Allow new attacks when not swinging, or restart if swing is nearly complete
    if (isSwinging && swingTimer < (swingDuration * 0.7f))
    {
      return false; // Still in active swing, deny attack
    }

    StartAttack();
    return true;
  }

  private void StartAttack()
  {
    isActive = true;
    Visible = true;
    isSwinging = true;
    swingTimer = 0f;

    // Reset to base rotation before calculating target for visible animation
    ResetToBaseRotation();
    CalculateTargetRotation();
  }

  private void ResetToBaseRotation()
  {
    // Calculate base rotation (offset from target direction)
    var mousePosition = GetScreenToWorld2D(GetMousePosition(), ViewportManager.Camera);
    Vector2 direction = mousePosition - Position;
    float targetAngleRad = MathF.Atan2(direction.Y, direction.X);
    float targetAngleDeg = targetAngleRad * (180f / MathF.PI) + 90f;

    // Start swing from 45 degrees behind target direction for visible arc
    currentRotation = NormalizeAngle(targetAngleDeg - 90f);
  }

  private void CalculateTargetRotation()
  {
    var mousePosition = GetScreenToWorld2D(GetMousePosition(), ViewportManager.Camera);
    Vector2 direction = mousePosition - Position;

    // Calculate angle and convert to degrees
    float angleRad = MathF.Atan2(direction.Y, direction.X);
    targetRotation = angleRad * (180f / MathF.PI) + 90f;

    // Add swing arc - target 45 degrees ahead of cursor direction for full swing
    targetRotation = NormalizeAngle(targetRotation + 45f);
  }

  /// <summary>
  /// Returns true if the weapon is currently in swing animation
  /// </summary>
  public bool IsSwinging => isSwinging;

  /// <summary>
  /// Returns true if the weapon is active and visible
  /// </summary>
  public bool IsActive => isActive;

  /// <summary>
  /// Configures the weapon animation properties
  /// </summary>
  public void ConfigureAnimation(float rotationSpeed, float swingDuration)
  {
    this.rotationSpeed = MathF.Max(rotationSpeed, 1f);
    this.swingDuration = MathF.Max(swingDuration, 0.05f);
  }

  /// <summary>
  /// Updates weapon position (called by player)
  /// </summary>
  public void UpdatePosition(Vector2 newPosition)
  {
    Position = newPosition;
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    if (!isActive) return;

    UpdateSwingAnimation(dt);
  }

  private void UpdateSwingAnimation(float dt)
  {
    if (!isSwinging) return;

    swingTimer += dt;
    UpdateRotation(dt);

    // End swing animation when duration is reached
    if (swingTimer >= swingDuration)
    {
      EndSwing();
    }
  }

  private void UpdateRotation(float dt)
  {
    float rotationDifference = CalculateShortestRotationDifference();

    // Use easing for more natural swing motion - faster at start, slower at end
    float progress = swingTimer / swingDuration;
    float easedSpeed = rotationSpeed * (2f - progress); // Start fast, slow down

    float rotationStep = easedSpeed * dt;

    if (MathF.Abs(rotationDifference) <= rotationStep)
    {
      currentRotation = targetRotation;
    }
    else
    {
      currentRotation += MathF.Sign(rotationDifference) * rotationStep;
      currentRotation = NormalizeAngle(currentRotation);
    }
  }

  private float CalculateShortestRotationDifference()
  {
    float difference = targetRotation - currentRotation;

    // Choose shortest rotation path
    while (difference > 180f) difference -= 360f;
    while (difference < -180f) difference += 360f;

    return difference;
  }

  private float NormalizeAngle(float angle)
  {
    while (angle >= 360f) angle -= 360f;
    while (angle < 0f) angle += 360f;
    return angle;
  }

  private void EndSwing()
  {
    isSwinging = false;
    swingTimer = 0f;
    // Hide weapon immediately when swing animation finishes
    // Visible = false;
    isActive = false;
    currentRotation = 0f;
  }

  private void DeactivateWeapon()
  {
    // Visible = false;
    isActive = false;
    isSwinging = false;
    swingTimer = 0f;
  }

  public override void Draw()
  {
    if (!Visible) return;

    base.Draw();
    RenderWeapon();
  }

  private void RenderWeapon()
  {
    Rectangle source = new(176, 144, FlipX ? -16 : 16, 16);
    DrawTexturePro(
      AssetsManager.Textures["TinyDungeon"],
      source,
      new Rectangle(Position.X, Position.Y, 16, 16),
      Anchor,
      currentRotation,
      Color.White
    );
  }
}