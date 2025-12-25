using Akamah.Engine.Systems;

namespace Akamah.Engine.Core.Engine;

public class Component
{
  private GameObject? owner;

  protected IReadOnlyGameObject Owner => owner ?? throw new InvalidOperationException("Owner not set");

  public void Attach(GameObject owner) => this.owner = owner;
  public void Detach() => owner = null;

  public virtual void Initialize() { }

  public virtual void Update(float deltaTime) { }

  public virtual void Draw() { }

  public virtual void Terminate() { }

  public virtual void Handle(GameAction action) { }
}



