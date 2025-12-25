namespace Akamah.Engine.Engine.Core;

public interface IHandle<in TAction> where TAction : GameAction
{
  void Handle(TAction action);
}