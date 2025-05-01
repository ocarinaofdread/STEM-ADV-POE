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

    // ReSharper disable Unity.PerformanceAnalysis
    public void EnterState(AIAgent agent)
    {
        _playerTransform ??= GameObject.FindGameObjectWithTag("Player").transform;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Update(AIAgent agent)
    {
        if (CalculateVector(agent).sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
        {
            agent.ChangeState(AIStateID.ChasePlayer);
        }
    }

    public void ExitState(AIAgent agent)
    {
        
    }

    private Vector3 CalculateVector(AIAgent agent)
    {
        Vector3 playerPosition = _playerTransform.position;

        return (playerPosition - agent.transform.position);
    }
}
