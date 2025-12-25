namespace Akamah.Engine.Core.Engine;

public record HealthDepleted(IReadOnlyGameObject GameObject) : GameEvent;

