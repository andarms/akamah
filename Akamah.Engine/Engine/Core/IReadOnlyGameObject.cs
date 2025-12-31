using Akamah.Engine.Systems.Collision;

namespace Akamah.Engine.Engine.Core;

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
  void Dispatch(GameAction action);
  void Emit<T>(T evt) where T : GameEvent;
  void When<T>(Action<T> callback) where T : GameEvent;
  void Terminate();
  Vector2 LocalToGlobal(Vector2 localPosition);
  Vector2 GlobalToLocal(Vector2 globalPosition);
}
