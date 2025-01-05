using System;
using System.Collections.Generic;

public class StateMachine
{
    public State CurrentState => _currentState;
    private State _currentState;
    private readonly Dictionary<Type, State> _states = new Dictionary<Type, State>();

    public void AddState<T>(T state) where T : State
    {
        var type = typeof(T);
        if (!_states.ContainsKey(type))
        {
            _states[type] = state;
        }
    }

    public void ChangeState<T>() where T : State
    {
        var type = typeof(T);

        if (_currentState != null && _currentState.GetType() == type)
            return;

        if (_states.TryGetValue(type, out var newState))
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    }
}