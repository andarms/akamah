using Akamah.Engine.Assets.Management;
using Akamah.Engine.Core.Camera;
using Akamah.Engine.Core.Engine;
using Akamah.Engine.Core.StateManagement;
using Akamah.Engine.Systems;

namespace Akamah.Engine.Gameplay.Interactions.Equipment;


public class Weapon : GameObject
{
  public readonly StateMachine stateMachine = new();

  // Pivot properties for reliable weapon tracking
  public Vector2 PivotPosition { get; set; } = Vector2.Zero;
  public float PivotRotation { get; set; } = 0f;
  public Vector2 PivotScale { get; set; } = Vector2.One;

  public Vector2 Direction { get; internal set; } = Vector2.Zero;

  internal bool isSwinging = false;

  public Weapon()
  {
    InitializeWeapon();
    InitializeStateMachine();
    Layer = 1;
  }

  private void InitializeWeapon()
  {
    Anchor = new Vector2(8, 16);
    PivotScale = Vector2.One;
  }

  private void InitializeStateMachine()
  {
    stateMachine.Start(new WeaponIdleState(this));
  }


  public bool TryAttack()
  {
    if (stateMachine.CurrentState is WeaponIdleState)
    {
      stateMachine.ChangeState(new WeaponAttackingState(this));
      return true;
    }
    return false;
  }

  public bool IsSwinging => isSwinging;

  public bool IsActive => Visible;

  public void UpdatePosition(Vector2 newPosition)
  {
    Position = newPosition;
    PivotPosition = newPosition;
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    UpdatePivotTracking(dt);
    stateMachine.Update(dt);
  }

  private void UpdatePivotTracking(float dt)
  {
    var mousePosition = GetScreenToWorld2D(GetMousePosition(), ViewportManager.Camera);
    Vector2 direction = mousePosition - PivotPosition;
    Direction = Vector2.Normalize(direction);
    if (direction.LengthSquared() > 0.01f)
    {
      // Calculate rotation - weapon sprite points up by default, so we need to adjust
      float angleRad = MathF.Atan2(direction.Y, direction.X);
      float angleDeg = angleRad * (180f / MathF.PI);

      // Adjust for weapon sprite default orientation (pointing up)
      PivotRotation = angleDeg + 90f;

      // Normalize angle to 0-360 range for consistency
      while (PivotRotation < 0) PivotRotation += 360f;
      while (PivotRotation >= 360f) PivotRotation -= 360f;
    }

    // Use X-axis flipping for horizontal orientation (not Y-axis)
    if (mousePosition.X - PivotPosition.X < 0)
    {
      PivotScale = new Vector2(-1, 1);  // Flip horizontally
    }
    else
    {
      PivotScale = new Vector2(1, 1);   // Normal orientation
    }
  }

  public override void Draw()
  {
    if (!Visible) return;

    base.Draw();
    RenderWeapon();
  }

  private void RenderWeapon()
  {
    Vector2 renderPosition = PivotPosition;
    bool shouldFlip = PivotScale.X > 0;

    int spriteWidth = shouldFlip ? -16 : 16;

    Rectangle sourceSprite = GameWorld.Player.Tool.SourceSprite;
    Rectangle source = new(sourceSprite.X, sourceSprite.Y, spriteWidth, sourceSprite.Height);

    DrawTexturePro(
      AssetsManager.Textures["TinyDungeon"],
      source,
      new Rectangle(renderPosition.X, renderPosition.Y, 16, 16),
      Anchor,
      PivotRotation,
      Color.White
    );
  }
}


public class WeaponIdleState(Weapon weapon) : State
{
  private readonly Weapon weapon = weapon;

  public override void Enter()
  {

    weapon.isSwinging = false;
  }

  public override void Update(float deltaTime)
  {
    // Weapon stays hidden, pivot tracking handled in main Update()
    // No manual rotation needed - pivot system handles positioning
  }

  public override void Exit()
  {
    // Prepare for attack - show weapon

  }
}

public class WeaponAttackingState(Weapon weapon) : State
{
  private readonly Weapon weapon = weapon;
  private float swingTimer = 0f;
  private const float SwingDuration = 0.3f;

  private readonly MeleeAttack meleeAttack = new();

  // Store base rotation to animate around
  private float baseRotation = 0f;
  private const float SwingArc = 90f; // Total swing arc in degrees

  public override void Enter()
  {
    swingTimer = 0f;
    weapon.isSwinging = true;
    GameWorld.AddGameObject(meleeAttack);
    baseRotation = weapon.PivotRotation;
    meleeAttack.Position = weapon.Position + weapon.Direction * 12f;
  }

  public override void Update(float deltaTime)
  {
    swingTimer += deltaTime;
    float progress = MathF.Min(swingTimer / SwingDuration, 1f);

    // Use easing for more natural swing feel - start fast, slow down at the end
    float easedProgress = EaseOutCubic(progress);

    // Create swing animation: start at -45°, end at +45° relative to base rotation
    float swingOffset = Lerp(-SwingArc / 2f, SwingArc / 2f, easedProgress);

    // Set the weapon rotation directly (don't add continuously)
    weapon.PivotRotation = baseRotation + swingOffset;

    // End swing when duration is reached
    if (progress >= 1f)
    {
      weapon.stateMachine.ChangeState(new WeaponIdleState(weapon));
    }
  }

  public override void Exit()
  {
    weapon.isSwinging = false;
    // Weapon visibility handled by idle state
    GameWorld.RemoveGameObject(meleeAttack);
  }

  private float EaseOutCubic(float t)
  {
    return 1f - MathF.Pow(1f - t, 3f);
  }

  private float Lerp(float a, float b, float t)
  {
    return a + (b - a) * t;
  }
}
