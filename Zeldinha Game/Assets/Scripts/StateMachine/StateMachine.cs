using UnityEngine;

public class StateMachine
{
	private State _currentState;
	public string CurrentStateName {  get; private set; }
	
	public void Update()
	{
		_currentState?.Update();
	}
    public void LateUpdate()
    {
        _currentState?.LateUpdate();
    }
    public void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }


    public void ChangeState(State newState)
	{
		_currentState?.Exit();

		_currentState = newState;
		CurrentStateName = _currentState.name;
		newState?.Enter();
	}
}
