using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayerState : AIState
{
    private Transform _playerTransform;
    private float _timer;
    
    public AIStateID GetID()
    {
        return AIStateID.ChasePlayer;
    }

    public void EnterState(AIAgent agent)
    {
        _playerTransform ??= GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update(AIAgent agent)
    {
        if (!agent.navMeshAgent.enabled) { return; }

        _timer -= Time.deltaTime;
        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = _playerTransform.position;
        }

        if (_timer >= 0.0f) { return; }

        Vector3 direction = _playerTransform.position - agent.navMeshAgent.destination;
        direction.y = 0;
        // Squared to avoid direction.magnitude square root function
        float maxDistance = agent.config.maxDistance;
        if (direction.sqrMagnitude > maxDistance * maxDistance) {
            if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial) {
                agent.navMeshAgent.destination = _playerTransform.position;
            }
        }
        _timer = agent.config.maxTime;
    }

    public void ExitState(AIAgent agent)
    {
        
    }
}
