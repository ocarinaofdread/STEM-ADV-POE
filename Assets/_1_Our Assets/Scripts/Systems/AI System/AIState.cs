using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateID
{
    ChasePlayer,
    Idle
}

public interface AIState
{
    AIStateID GetID();
    void EnterState(AIAgent agent);
    void Update(AIAgent agent);
    void ExitState(AIAgent agent);
}
