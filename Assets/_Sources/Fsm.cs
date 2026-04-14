using System;
using System.Collections.Generic;
using Assets.FSM;
using UnityEngine;

public abstract class Fsm
{
    private Dictionary<Type, FsmState> _states = new();
    
    private FsmState StateCurrent { get; set; }

    public void Update()
    {
        StateCurrent?.Update();
    }

    public void AddState(FsmState state)
    {
        _states.Add(state.GetType(), state);
    }

    public void SetState<T>() where T : FsmState
    {
        var type = typeof(T);
        
        if(StateCurrent != null && StateCurrent.GetType() == type)
            return;

        if (_states.TryGetValue(type, out var newState))
        {
            StateCurrent?.Exit();
            StateCurrent = newState;
            StateCurrent.Enter();
        }
    }
}

