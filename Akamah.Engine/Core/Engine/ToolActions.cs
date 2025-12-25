namespace Akamah.Engine.Core.Engine;

public record Slash(int Damage) : GameAction;

public record Chop(int Damage) : GameAction;

public record Mine(int Damage) : GameAction;

public record Dig(int Damage) : GameAction;