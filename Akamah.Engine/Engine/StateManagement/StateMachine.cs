namespace Akamah.Engine.Core.StateManagement;

public class StateMachine
{
  private State? currentState;
  private State? nextState;

  public State? CurrentState => currentState;

  public void ChangeState(State newState)
  {
    nextState = newState;
  }

  public void Update(float deltaTime)
  {
    // Handle state transition
    if (nextState != null)
    {
      currentState?.Exit();
      currentState = nextState;
      currentState.Enter();
      nextState = null;
    }

    // Update current state
    currentState?.Update(deltaTime);
  }

  public void Start(State initialState)
  {
    currentState = initialState;
    currentState.Enter();
  }

  public void Stop()
  {
    currentState?.Exit();
    currentState = null;
    nextState = null;
  }
}