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
  Vector2 GlobalPosition { get; }
  GameObject? Parent { get; }
  IReadOnlyList<GameObject> Children { get; }
  Collider? Collider { get; }

  Rectangle GetBounds();

  T Get<T>() where T : Component;
  bool Has<T>() where T : Component;
  bool TryGet<T>(out T? component) where T : Component;
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

  public List<Component> Components { get; } = [];
  private readonly Dictionary<Type, List<Delegate>> listeners = [];
  private bool terminated = false;
  public bool Initialized { get; private set; } = false;

  public virtual void Initialize()
  {
    Components.ForEach(c => c.Initialize());
    // Initialize all children
    foreach (var child in Children)
    {
      child.Initialize();
    }
    Initialized = true;
  }

  public virtual void Update(float deltaTime)
  {
    Components.ForEach(c => c.Update(deltaTime));
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
    Components.ForEach(c => c.Draw());

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

    // Terminate all children first
    foreach (var child in Children.ToList())
    {
      child.Terminate();
    }
    Children.Clear();

    // Remove from parent
    Parent?.RemoveChild(this);

    Components.ForEach(c => c.Terminate());
    Components.Clear();
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

  public T Get<T>() where T : Component
  {
    Component? component = Components.FirstOrDefault(c => c is T) ?? throw new InvalidOperationException($"Component of type {typeof(T).Name} not found on GameObject.");
    return (T)component;
  }

  public bool Has<T>() where T : Component
  {
    return Components.Any(c => c is T);
  }

  public bool TryGet<T>(out T? component) where T : Component
  {
    Component? foundComponent = Components.FirstOrDefault(c => c is T);
    if (foundComponent != null)
    {
      component = (T)foundComponent;
      return true;
    }
    component = default;
    return false;
  }


  public void Add<T>(T component) where T : Component
  {
    component.Attach(this);
    Components.Add(component);
  }

  public void Emit<T>(T evt) where T : GameEvent
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


  public void Handle(GameAction action)
  {
    foreach (var component in Components.ToList())
    {
      TryHandle(component, action);
    }
  }

  private static void TryHandle(Component component, GameAction action)
  {
    var actionType = action.GetType();

    var handlerInterfaces = component.GetType()
      .GetInterfaces()
      .Where(i =>
        i.IsGenericType &&
        i.GetGenericTypeDefinition() == typeof(IHandle<>) &&
        i.GenericTypeArguments[0].IsAssignableFrom(actionType)
      );

    foreach (var handler in handlerInterfaces)
    {
      handler.GetMethod("Handle")!.Invoke(component, [action]);
    }
  }
}