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

  private readonly Dictionary<Type, List<Delegate>> listeners = [];


  private readonly Dictionary<Type, List<Delegate>> handlers = [];

  private bool terminated = false;
  public bool Initialized { get; private set; } = false;


  public GameObject Root => GetRoot();

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
    Parent?.RemoveChild(this);

    // Clear local listeners and try to clean up root listeners
    listeners.Clear();

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


  public virtual Rectangle GetBounds()
  {
    if (Collider == null) return new Rectangle(GlobalPosition.X, GlobalPosition.Y, 0, 0);
    return Collider.GetBounds(GlobalPosition, Anchor);
  }

  public void Add(GameObject child)
  {
    if (child == null) return;
    if (child == this) throw new InvalidOperationException("Cannot add self as child");

    // Remove from current parent if it has one
    child.Parent?.RemoveChild(child);

    // Convert child's global position to local position relative to this parent
    var childGlobalPos = child.GlobalPosition;
    child.Parent = this;
    child.Position = childGlobalPos - this.GlobalPosition;

    Children.Add(child);
  }

  public void RemoveChild(GameObject child)
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

  public Vector2 LocalToGlobal(Vector2 localPosition)
  {
    return GlobalPosition + localPosition;
  }

  public Vector2 GlobalToLocal(Vector2 globalPosition)
  {
    return globalPosition - GlobalPosition;
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

  public void Emit<T>(T evt) where T : GameEvent
  {
    // Always emit events at the root parent level so all descendants can listen
    var root = GetRoot();
    root.EmitToHierarchy(evt);
  }

  private void EmitLocal<T>(T evt) where T : GameEvent
  {
    if (!listeners.TryGetValue(typeof(T), out var delegates)) return;

    foreach (var del in delegates)
    {
      ((Action<T>)del)(evt);
    }
  }

  private void EmitToHierarchy<T>(T evt) where T : GameEvent
  {
    // Don't emit if this object is terminated
    if (terminated) return;

    // Emit to self first
    EmitLocal(evt);

    // Then emit to all non-terminated children recursively
    foreach (var child in Children.ToList())
    {
      if (!child.terminated)
      {
        child.EmitToHierarchy(evt);
      }
    }
  }

  public void When<T>(Action<T> callback) where T : GameEvent
  {
    if (terminated)
      return;

    // Always subscribe at the root level so all hierarchy events can be received
    var root = GetRoot();
    root.WhenLocal(callback);
  }

  private void WhenLocal<T>(Action<T> callback) where T : GameEvent
  {
    if (terminated)
      return;

    var type = typeof(T);

    if (!listeners.TryGetValue(type, out var handlers))
    {
      handlers = [];
      listeners[type] = handlers;
    }

    handlers.Add(callback);
  }

  protected void Handle<T>(Action<T> handler) where T : GameAction
  {
    var type = typeof(T);
    if (!Root.handlers.TryGetValue(type, out var list))
    {
      Root.handlers[type] = list = [];
    }
    list.Add(handler);
  }

  public void Dispatch<T>(T action) where T : GameAction
  {
    var type = typeof(T);
    if (!Root.handlers.TryGetValue(type, out var delegates)) return;
    foreach (var del in delegates)
    {
      ((Action<T>)del)(action);
    }
  }
}