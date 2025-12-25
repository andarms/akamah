using Akamah.Engine.Core.Camera;
using Akamah.Engine.Systems;
using Akamah.Engine.Systems.Collision;

namespace Akamah.Engine.Core.Engine;

public interface IReadOnlyGameObject
{
  Vector2 Position { get; }
  Collider? Collider { get; }

  T Get<T>() where T : Component;
  bool Has<T>() where T : Component;
  bool TryGet<T>(out T? component) where T : Component;
  void Handle(GameAction action);
  void Emit<T>(T evt) where T : GameEvent;
  void When<T>(Action<T> callback) where T : GameEvent;

  void Terminate();
}

public class GameObject : IReadOnlyGameObject
{
  public Vector2 Position { get; set; } = Vector2.Zero;
  public Collider? Collider { get; set; }
  public bool Visible { get; set; } = true;
  public int Layer { get; set; } = 0;
  public Vector2 Anchor { get; set; } = Vector2.Zero;


  public bool FlipX { get; set; } = false;

  public Vector2 RenderPosition => Position - Anchor;


  public List<Component> Components { get; } = [];
  private readonly Dictionary<Type, List<Delegate>> listeners = [];
  private bool terminated = false;

  public virtual void Initialize()
  {
    Components.ForEach(c => c.Initialize());
  }

  public virtual void Update(float deltaTime)
  {
    Components.ForEach(c => c.Update(deltaTime));
  }

  public virtual void Draw()
  {
    // Spatial system now handles visibility culling more efficiently
    // Individual objects don't need to check visibility anymore
    // The spatial manager filters objects before calling Draw()
    Visible = true;
    Components.ForEach(c => c.Draw());
  }


  public virtual void Terminate()
  {
    if (terminated) return;
    terminated = true;
    Components.ForEach(c => c.Terminate());
    Components.Clear();
    listeners.Clear();
    GameWorld.RemoveGameObject(this);
  }


  public virtual void Debug()
  {
    Collider?.Debug(Position, Anchor);
    DrawCircleV(Position, 2, Color.Lime);
  }

  protected virtual bool IsInCameraView()
  {
    // Default implementation uses point-based visibility with small margin
    return ViewportManager.IsPointInView(Position, 32f);
  }

  protected bool IsInCameraView(Vector2 size)
  {
    return ViewportManager.IsRectInView(Position, size);
  }


  public virtual Rectangle GetBounds()
  {
    if (Collider == null) return new Rectangle(Position.X, Position.Y, 0, 0);
    return Collider.GetBounds(Position, Anchor);
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