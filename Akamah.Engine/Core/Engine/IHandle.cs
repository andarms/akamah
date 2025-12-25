namespace Akamah.Engine.Core.Engine;

public interface IHandle<in TAction> where TAction : GameAction
{
  void Handle(TAction action);
}