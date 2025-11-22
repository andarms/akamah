using Raylib_cs;

namespace Akamah.Engine;

/// <summary>
/// Abstract base class for all game scenes
/// </summary>
public abstract class Scene : IDisposable
{
  /// <summary>
  /// Gets whether this scene has been initialized
  /// </summary>
  public bool IsInitialized { get; private set; }

  /// <summary>
  /// Gets whether this scene is currently active
  /// </summary>
  public bool IsActive { get; internal set; }

  /// <summary>
  /// Gets the background color for this scene
  /// </summary>
  public virtual Color BackgroundColor => Color.Black;

  /// <summary>
  /// Initialize the scene - called once when scene is first created
  /// </summary>
  public virtual void Initialize()
  {
    IsInitialized = true;
  }

  /// <summary>
  /// Called when the scene becomes active
  /// </summary>
  public virtual void OnEnter()
  {
    // Override in derived classes for scene entry logic
  }

  /// <summary>
  /// Called when the scene becomes inactive
  /// </summary>
  public virtual void OnExit()
  {
    // Override in derived classes for scene exit logic
  }

  /// <summary>
  /// Called when the scene is paused (pushed to stack)
  /// </summary>
  public virtual void OnPause()
  {
    // Override in derived classes for scene pause logic
  }

  /// <summary>
  /// Called when the scene is resumed (popped from stack)
  /// </summary>
  public virtual void OnResume()
  {
    // Override in derived classes for scene resume logic
  }

  /// <summary>
  /// Update the scene logic - called every frame when scene is active
  /// </summary>
  /// <param name="deltaTime">Time elapsed since last frame in seconds</param>
  public virtual void Update(float deltaTime)
  {
    // Override in derived classes for scene-specific update logic
  }

  /// <summary>
  /// Draw the scene content - called every frame when scene is active
  /// </summary>
  public virtual void Draw()
  {
    // Override in derived classes for scene-specific rendering
  }

  /// <summary>
  /// Handle input events - called when scene is active
  /// </summary>
  public virtual void HandleInput()
  {
    // Override in derived classes for scene-specific input handling
  }

  /// <summary>
  /// Dispose scene resources - called when scene is being destroyed
  /// </summary>
  public virtual void Dispose()
  {
    // Override in derived classes for cleanup
    IsInitialized = false;
    GC.SuppressFinalize(this);
  }

  /// <summary>
  /// Unload scene resources - kept for backward compatibility
  /// </summary>
  public virtual void Unload()
  {
    Dispose();
  }
}