using Akamah.Engine.Core.Engine;
using Akamah.Engine.Engine.Camera;
using Akamah.Engine.Systems.Collision;
using Akamah.Engine.World;

namespace Akamah.Engine.Engine.Core;

public record GameEvent();
public record GameAction();

public interface IReadOnlyGameObject
{
  Vector2 Position { get; }
  Vector2 Anchor { get; }
  Vector2 GlobalPosition { get; }
  GameObject? Parent { get; }
  IReadOnlyList<GameObject> Children { get; }
  Collider? Collider { get; }
  bool Visible { get; }
  bool FlipX { get; }

  Rectangle GetBounds();

  T Get<T>() where T : GameObject;
  bool Has<T>() where T : GameObject;
  bool TryGet<T>(out T? gameObject) where T : GameObject;
  void Handle(GameAction action);
  void Emit<T>(T evt) where T : GameEvent;
  void When<T>(Action<T> callback) where T : GameEvent;
  void Terminate();
  Vector2 LocalToGlobal(Vector2 localPosition);
  Vector2 GlobalToLocal(Vector2 globalPosition);
}

public class GameObject : IReadOnlyGameObject
{
  public Vector2 Position { get; set; } = Vector2.Zero;
  public Collider? Collider { get; set; }
  public bool Visible { get; set; } = true;
  public int Layer { get; set; } = 0;
  public Vector2 Anchor { get; set; } = Vector2.Zero;

  // Parent-Child relationship
  public GameObject? Parent { get; private set; }
  public List<GameObject> Children { get; } = [];
  IReadOnlyList<GameObject> IReadOnlyGameObject.Children => Children;

  // Global position calculated from parent hierarchy
  public Vector2 GlobalPosition => Parent != null ? Parent.GlobalPosition + Position : Position;

  public bool FlipX { get; set; } = false;

  public Vector2 RenderPosition => GlobalPosition - Anchor;

  private readonly Dictionary<Type, List<Delegate>> listeners = [];
  private bool terminated = false;
  public bool Initialized { get; private set; } = false;

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
    foreach (var child in Children.ToList())
    {
      if (!child.terminated) { child.Update(deltaTime); }
    }
  }

  public virtual void Draw()
  {
    // Spatial system now handles visibility culling more efficiently
    // Individual objects don't need to check visibility anymore
    // The spatial manager filters objects before calling Draw()
    Visible = true;

    // Draw all children
    foreach (var child in Children)
    {
      if (child.Visible && !child.terminated) { child.Draw(); }
    }
  }


  public virtual void Terminate()
  {
    if (terminated) return;
    terminated = true;
    foreach (var child in Children.ToList())
    {
      child.Terminate();
    }
    Children.Clear();
    Parent?.RemoveChild(this);
    listeners.Clear();
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

  public void AddChild(GameObject child)
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
    // Emit on this GameObject
    EmitLocal(evt);

    // Also emit on parent and siblings for component-like behavior
    if (Parent != null)
    {
      Parent.EmitLocal(evt);
      foreach (var sibling in Parent.Children.ToArray())
      {
        if (sibling != this)
        {
          sibling.EmitLocal(evt);
        }
      }
    }
  }

  private void EmitLocal<T>(T evt) where T : GameEvent
  {
    if (!listeners.TryGetValue(typeof(T), out var delegates)) return;

    foreach (var del in delegates)
    {
      ((Action<T>)del)(evt);
    }
  }

  public void When<T>(Action<T> callback) where T : GameEvent
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


  public virtual void Handle(GameAction action)
  {
    // Handle the action on this GameObject first
    TryHandle(this, action);

    // Then forward to children that can handle it
    foreach (var child in Children.ToList())
    {
      TryHandle(child, action);
    }
  }

  private static void TryHandle(GameObject gameObject, GameAction action)
  {
    var actionType = action.GetType();

    var handlerInterfaces = gameObject.GetType()
      .GetInterfaces()
      .Where(i =>
        i.IsGenericType &&
        i.GetGenericTypeDefinition() == typeof(IHandle<>) &&
        i.GenericTypeArguments[0].IsAssignableFrom(actionType)
      );

    foreach (var handler in handlerInterfaces)
    {
      handler.GetMethod("Handle")!.Invoke(gameObject, [action]);
    }
  }
}