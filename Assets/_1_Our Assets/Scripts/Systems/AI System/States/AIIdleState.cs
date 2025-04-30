using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIIdleState : AIState
{
    private Transform _playerTransform;
    private float _timer;
    
    public AIStateID GetID()
    {
        return AIStateID.Idle;
    }

    public void EnterState(AIAgent agent)
    {
        
    }

    public void Update(AIAgent agent)
    {
        
    }

    public void ExitState(AIAgent agent)
    {
        
    }
}
