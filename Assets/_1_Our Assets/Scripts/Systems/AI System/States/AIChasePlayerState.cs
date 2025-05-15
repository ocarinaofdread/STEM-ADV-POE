using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIChasePlayerState : AIState
{
    private Transform _playerTransform;
    private float _timer;
    private float _sqrMaxDistance;
    private readonly float _navMeshCheckDistance = 0.1f;

    private bool _doRanged;
    private float _rangedTimer;
    
    public AIStateID GetID()
    {
        return AIStateID.ChasePlayer;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void EnterState(AIAgent agent)
    {
        switch (agent.enemy)
        {
            // Skeleton
            case Skeleton agentSkeleton:
                agentSkeleton.ChangeSpeed(0.5f);
                break;
            case Goblin agentGoblin:
                agentGoblin.ChangeSpeed(0.5f);
                break;
            case Golem agentGolem:
                _doRanged = true;
                _rangedTimer = Random.Range(agent.config.minRangedWaitTime, agent.config.maxRangedWaitTime);
                agentGolem.ChangeSpeed(0.5f);
                break;
        }

        _timer = -0.1f;
        _sqrMaxDistance = agent.config.maxDistance * agent.config.maxDistance;
        
        //Debug.Log("Player Object: " + GameObject.FindGameObjectWithTag("Player").name);
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update(AIAgent agent)
    {
        if (!agent.navMeshAgent.enabled) { return; }
        
        _timer -= Time.deltaTime;
        if (!agent.navMeshAgent.hasPath)
        {
            if (!SwitchRoamIfNotOnNavMesh(agent, _playerTransform.position))
            {
                agent.navMeshAgent.destination = _playerTransform.position;
            }

        }

        // Ranged Attack Waiting - Only works on Golem
        if (_doRanged)
        {
            if (_rangedTimer <= 0)
            {
                _timer = 1.0f;
                agent.ChangeState(AIStateID.RangedAttack);
            }
            else
            {
                _rangedTimer -= Time.deltaTime;
            }
        }

        // Delays in between destination setting + checks for Idle
        if (_timer >= 0.0f) { return; }
        
        var sqrTFDistance = CalculateSqrTFDistance(agent);
        if (sqrTFDistance > _sqrMaxDistance) {
            //Debug.Log("NMDistance is greater than max distance");
            if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial) {
                if (!SwitchRoamIfNotOnNavMesh(agent, _playerTransform.position))
                {
                    agent.navMeshAgent.destination = _playerTransform.position;
                }
                //Debug.Log("New destination");
            }
        }
        else
        {
            //Debug.Log("TFDistance is greater than max distance");
            agent.ChangeState(AIStateID.Idle);
        }
        
         
        _timer = agent.config.maxNavMeshRefreshTime;
    }

    public void ExitState(AIAgent agent)
    {
        agent.navMeshAgent.ResetPath();
    }

    private float CalculateSqrTFDistance(AIAgent agent)
    {
        var direction = _playerTransform.position - agent.gameObject.transform.position;
        direction.y = 0;
        // Squared to avoid direction.magnitude square root function
        return direction.sqrMagnitude;
    }

    private bool SwitchRoamIfNotOnNavMesh(AIAgent agent, Vector3 testingPosition)
    {
        if (agent.GetState(AIStateID.Roam) == null) return false;
        
        var isOnNavMesh =
            NavMesh.SamplePosition(testingPosition, out var hit, _navMeshCheckDistance, NavMesh.AllAreas);

        if (isOnNavMesh && Vector3.Distance(testingPosition, hit.position) < _navMeshCheckDistance)
            return false;
        
        agent.ChangeState(AIStateID.Roam);
        return true;
    }
}
