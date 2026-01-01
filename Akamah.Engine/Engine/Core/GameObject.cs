using System.Reflection;
using Akamah.Engine.Core.Engine;
using Akamah.Engine.Engine.Camera;
using Akamah.Engine.Systems.Collision;
using Akamah.Engine.World;

namespace Akamah.Engine.Engine.Core;

public record GameEvent();
public record GameAction();

public class GameObject
{
  public Vector2 Position { get; set; } = Vector2.Zero;
  public Collider? Collider { get; set; }
  public bool Visible { get; set; } = true;
  public int Layer { get; set; } = 0;
  public Vector2 Anchor { get; set; } = Vector2.Zero;

  // Parent-Child relationship
  public GameObject? Parent { get; private set; }
  public List<GameObject> Children { get; } = [];

  // Global position calculated from parent hierarchy
  public Vector2 GlobalPosition => Parent != null ? Parent.GlobalPosition + Position : Position;

  public bool FlipX { get; set; } = false;

  public Vector2 RenderPosition => GlobalPosition - Anchor;

  private readonly Dictionary<Type, List<Delegate>> eventListeners = [];
  private readonly Dictionary<Type, List<Delegate>> actionHandlers = [];

  private bool terminated = false;
  public bool Initialized { get; private set; } = false;

  #region  Lifecycle Methods
  public virtual void Initialize()
  {
    // Initialize all children
    foreach (var child in Children)
    {
      child.Initialize();
    }
    Initialized = true;
  }

  public virtual void Update(float deltaTime)
  {
    // Update children and remove terminated ones
    for (int i = Children.Count - 1; i >= 0; i--)
    {
      var child = Children[i];
      if (child.terminated)
      {
        Children.RemoveAt(i);
        continue;
      }
      child.Update(deltaTime);
    }
  }

  public virtual void Draw()
  {
    // Spatial system now handles visibility culling more efficiently
    // Individual objects don't need to check visibility anymore
    // The spatial manager filters objects before calling Draw()
    Visible = true;

    // Draw children and remove terminated ones
    for (int i = Children.Count - 1; i >= 0; i--)
    {
      var child = Children[i];
      if (child.terminated)
      {
        Children.RemoveAt(i);
        continue;
      }
      if (child.Visible) { child.Draw(); }
    }
  }


  public virtual void Terminate()
  {
    if (terminated) return;
    terminated = true;

    // Terminate all children first
    foreach (var child in Children.ToList())
    {
      child.Terminate();
    }
    Children.Clear();

    // Remove from parent
    Parent?.Remove(this);

    // Clear local listeners and try to clean up root listeners
    eventListeners.Clear();

    // Remove from game
    Game.Remove(this);
    Initialized = false;
  }


  public virtual void Debug()
  {
    Collider?.Debug(GlobalPosition, Anchor);
    DrawCircleV(GlobalPosition, 2, Color.Lime);

    // Debug children
    foreach (var child in Children)
    {
      child.Debug();
    }
  }

  protected virtual bool IsInCameraView()
  {
    // Default implementation uses point-based visibility with small margin
    return Game.Viewport.IsPointInView(GlobalPosition, 32f);
  }

  protected bool IsInCameraView(Vector2 size)
  {
    return Game.Viewport.IsRectInView(GlobalPosition, size);
  }

  #endregion

  public virtual Rectangle GetBounds()
  {
    if (Collider == null) return new Rectangle(GlobalPosition.X, GlobalPosition.Y, 0, 0);
    return Collider.GetBounds(GlobalPosition, Anchor);
  }


  #region Hierarchy Methods

  public void Add(GameObject child)
  {
    if (child == null) return;
    if (child == this) throw new InvalidOperationException("Cannot add self as child");

    // Remove from current parent if it has one
    child.Parent?.Remove(child);

    // Convert child's global position to local position relative to this parent
    var childGlobalPos = child.GlobalPosition;
    child.Parent = this;
    child.Position = childGlobalPos - GlobalPosition;
    Children.Add(child);
  }

  public void Remove(GameObject child)
  {
    if (child == null || !Children.Contains(child)) return;

    // Convert child's local position back to global position
    var childGlobalPos = child.GlobalPosition;
    child.Parent = null;
    child.Position = childGlobalPos;

    Children.Remove(child);
  }

  public GameObject GetRoot()
  {
    var current = this;
    while (current.Parent != null)
    {
      current = current.Parent;
    }
    return current;
  }

  public T Get<T>() where T : GameObject
  {
    var child = Children.OfType<T>().FirstOrDefault() ?? throw new InvalidOperationException($"GameObject of type {typeof(T).Name} not found as child.");
    return child;
  }

  public bool Has<T>() where T : GameObject
  {
    return Children.OfType<T>().Any();
  }

  public bool TryGet<T>(out T? gameObject) where T : GameObject
  {
    var child = Children.OfType<T>().FirstOrDefault();
    if (child != null)
    {
      gameObject = child;
      return true;
    }
    gameObject = default;
    return false;
  }
  #endregion


  #region Event System

  public void When<T>(Action<T> listener) where T : GameEvent
  {
    var type = typeof(T);
    if (!eventListeners.TryGetValue(type, out List<Delegate>? eventHandlers))
    {
      eventHandlers = [];
      eventListeners[type] = eventHandlers;
    }

    eventHandlers.Add(listener);
  }

  public void Emit<T>(T evt) where T : GameEvent
  {
    if (terminated) return;

    Parent?.ReceiveEvent(evt, source: this);
  }

  private void ReceiveEvent<T>(T evt, GameObject source) where T : GameEvent
  {
    if (terminated) return;

    // Fan out to children (siblings)
    foreach (var child in Children.ToList())
    {
      if (!child.terminated) child.Notify(evt);
    }
  }

  private void Notify<T>(T evt) where T : GameEvent
  {
    if (!eventListeners.TryGetValue(typeof(T), out var eventHandlers)) { return; }

    foreach (var del in eventHandlers) { ((Action<T>)del)(evt); }
  }



  public void Handle<T>(Func<T, bool> handler) where T : GameAction
  {
    var type = typeof(T);
    if (!actionHandlers.TryGetValue(type, out List<Delegate>? handlers))
    {
      handlers = [];
      actionHandlers[type] = handlers;
    }

    handlers.Add(handler);
  }

  public void Trigger<T>(T gameAction) where T : GameAction
  {
    if (terminated) return;

    // First, try to handle locally
    if (actionHandlers.TryGetValue(typeof(T), out var handlers))
    {
      foreach (var del in handlers)
      {
        var handled = ((Func<T, bool>)del)(gameAction);
        if (handled) return; // Stop if handled
      }
    }

    // If not handled, propagate
    ReceiveAction(gameAction);
  }

  private void ReceiveAction<T>(T gameAction) where T : GameAction
  {
    // Fan out to children
    foreach (var child in Children.ToList())
    {
      if (!child.terminated) child.Trigger(gameAction);
    }
  }


  #endregion
}