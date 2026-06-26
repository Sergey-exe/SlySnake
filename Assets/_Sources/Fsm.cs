using System;
using System.Collections.Generic;
using Assets.FSM;
using UnityEngine;

public abstract class Fsm
{
    private Dictionary<Type, FsmState> _states = new();
    
    public FsmState CurrentState { get; private set; }

    public void Update()
    {
        CurrentState?.Update();
    }

    public void AddState(FsmState state)
    {
        _states.Add(state.GetType(), state);
    }

    public void SetState<T>() where T : FsmState
    {
        var type = typeof(T);
        
        if(CurrentState != null && CurrentState.GetType() == type)
            return;

        if (_states.TryGetValue(type, out var newState))
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}

