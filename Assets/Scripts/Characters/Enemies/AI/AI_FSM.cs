using System;
using System.Collections.Generic;

public class AI_FSM
{
    private Dictionary<Type,AI_FSMState> states = new Dictionary<Type,AI_FSMState>();
    public AI_FSMState currentState { get; private set; }
    public void AddState(AI_FSMState state)
    {
        states.Add(state.GetType(), state);
    }
    public void SetState<T>() where T : AI_FSMState
    {
        var type = typeof(T);
        if(currentState?.GetType() == type)
        {
            return;
        }
        if(states.TryGetValue(type, out var newState))
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
    public AI_FSMState GetCurrentState()
    {
        return currentState;
    }
    public void Update()
    {
        currentState?.Update();
    }
}
