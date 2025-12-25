namespace Akamah.Engine.Core.Engine;

public record HealthChanged(IReadOnlyGameObject GameObject, int Before, int After) : GameEvent;

