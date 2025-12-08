using Akamah.Engine.Assets;
using Akamah.Engine.Collisions;
using Akamah.Engine.Core;
using Akamah.Engine.Systems;

namespace Akamah.Engine.Entities;

public class Weapon : GameObject
{
  public readonly StateMachine stateMachine = new();

  // Pivot properties for reliable weapon tracking
  public Vector2 PivotPosition { get; set; } = Vector2.Zero;
  public float PivotRotation { get; set; } = 0f;
  public Vector2 PivotScale { get; set; } = Vector2.One;

  // Animation properties
  internal bool isSwinging = false;

  // Subtle floating effect
  private float floatOffset = 0f;

  public Weapon()
  {
    InitializeWeapon();
    InitializeStateMachine();
  }

  private void InitializeWeapon()
  {
    Anchor = new Vector2(8, 16);
    Collider = new Collider
    {
      Size = new Vector2(16),
      Offset = Vector2.Zero
    };

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
    UpdateFloatingEffect(dt);
    stateMachine.Update(dt);
  }

  private void UpdatePivotTracking(float dt)
  {
    var mousePosition = GetScreenToWorld2D(GetMousePosition(), ViewportManager.Camera);

    // Calculate angle to mouse (like Godot's look_at)
    Vector2 direction = mousePosition - PivotPosition;
    if (direction.LengthSquared() > 0.01f) // Avoid zero direction
    {
      // Add 90 degrees to account for weapon sprite pointing downward by default
      PivotRotation = MathF.Atan2(direction.Y, direction.X) * (180f / MathF.PI) + 90f;
    }

    // Handle flipping for left-side attacks (like Godot's scale.y = -1)
    if (mousePosition.X - PivotPosition.X < 0)
    {
      PivotScale = new Vector2(1, -1);
    }
    else
    {
      PivotScale = new Vector2(1, 1);
    }
  }

  private void UpdateFloatingEffect(float dt)
  {
    // Subtle floating effect like in Godot demo
    // Use a simple timer approach since GetTime() may not be available
    floatOffset = MathF.Sin((float)GetTime() * 5.0f) * 2.0f;
  }



  public override void Draw()
  {
    if (!Visible) return;

    base.Draw();
    RenderWeapon();
  }

  private void RenderWeapon()
  {
    // Apply pivot-based rendering similar to Godot approach
    Vector2 renderPosition = PivotPosition;
    renderPosition.Y += floatOffset; // Add floating effect

    // Get mouse direction to determine sprite orientation using angle ranges
    var mousePosition = GetScreenToWorld2D(GetMousePosition(), ViewportManager.Camera);
    Vector2 direction = mousePosition - PivotPosition;

    // Basic flip based on mouse X position relative to weapon position
    bool shouldFlip = mousePosition.X > PivotPosition.X;

    int spriteWidth = shouldFlip ? -16 : 16;
    Rectangle source = new(176, 144, spriteWidth, 16);

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


public class WeaponIdleState : State
{
  private readonly Weapon weapon;

  public WeaponIdleState(Weapon weapon)
  {
    this.weapon = weapon;
  }

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

public class WeaponAttackingState : State
{
  private readonly Weapon weapon;
  private float swingTimer = 0f;
  private const float SwingDuration = 0.3f;

  // Store base rotation to animate around
  private float baseRotation = 0f;
  private const float SwingArc = 90f; // Total swing arc in degrees

  public WeaponAttackingState(Weapon weapon)
  {
    this.weapon = weapon;
  }

  public override void Enter()
  {
    swingTimer = 0f;
    weapon.isSwinging = true;


    // Capture the current pivot rotation as our base
    baseRotation = weapon.PivotRotation;

    // Play attack sound effect with pitch variation (like Godot demo)
    // TODO: Add sound effects when audio system is implemented
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