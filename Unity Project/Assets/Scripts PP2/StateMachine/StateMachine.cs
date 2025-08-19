using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public virtual void OnStarted() { }
    public virtual void OnStopped() { }

    public virtual void OnUpdate() { }
}

public class StateMachine<TStateID> where TStateID : Enum
{
    public State CurrentState { get; private set; }

    private readonly Dictionary<TStateID, State> states = new Dictionary<TStateID, State>();

    public void AddState(TStateID id, State state)
    {
        states.Add(id, state);
    }

    public void SetState(TStateID id)
    {
        CurrentState?.OnStopped();

        if(!states.TryGetValue(id, out State state))
        {
            throw new Exception($"State not found: {state}");
        }

        CurrentState = state;
        CurrentState.OnStarted();
        Debug.Log($"State {state} is set");
    }

    public void Update()
    {
        CurrentState.OnUpdate();
    }

}


