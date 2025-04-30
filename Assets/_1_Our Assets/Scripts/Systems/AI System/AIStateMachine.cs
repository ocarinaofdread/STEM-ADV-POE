using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    private AIState[] states;
    private AIAgent agent;
    private AIStateID currentState;

    // Constructor
    public AIStateMachine(AIAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(AIStateID)).Length;
        states = new AIState[numStates];
    }

    // State Management
    public void RegisterState(AIState state)
    {
        int index = (int)state.GetID();
        states[index] = state;
    }

    public AIState GetState(AIStateID id)
    {
        int index = (int)id;
        return states[index];
    }
    
    // State Machine Utility
    public void Update()
    {
        GetState(currentState)?.Update(agent );
    }

    public void ChangeState(AIStateID newState)
    {
        GetState(currentState)?.ExitState(agent);
        currentState = newState;
        GetState(currentState)?.EnterState(agent);
    }
}
