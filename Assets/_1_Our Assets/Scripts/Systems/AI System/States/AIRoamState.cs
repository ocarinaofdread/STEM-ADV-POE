using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIRoamState : AIState
{
    private Transform _playerTransform;
    private Vector3 _centerPoint;
    private float _sightDistance;
    private float _minRoamDeviation;
    private float _maxRoamDeviation;
    private float _waitTimer;
    private float _minWaitTime;
    private float _maxWaitTime;
    private float _resetTime;
    
    public AIStateID GetID()
    {
        return AIStateID.Roam;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void EnterState(AIAgent agent)
    {
        _playerTransform ??= GameObject.FindGameObjectWithTag("Player").transform;
        _centerPoint = agent.transform.position;
        _sightDistance = agent.config.maxSightRange;
        _minRoamDeviation = agent.config.minRoamDeviation;
        _maxRoamDeviation = agent.config.maxRoamDeviation;
        _minWaitTime = agent.config.minRoamWaitTime;
        _maxWaitTime = agent.config.maxRoamWaitTime;
        _resetTime = agent.config.roamResetTime;

        _waitTimer = Random.Range(_minWaitTime, _maxWaitTime);
        
        agent.navMeshAgent.stoppingDistance = 0;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Update(AIAgent agent)
    {
        if (CalculateSqrDistance(agent) < _sightDistance * _sightDistance)
        {
            agent.ChangeState(AIStateID.ChasePlayer);
            return;
        }
        
        if (!agent.navMeshAgent.hasPath //|| agent.navMeshAgent.velocity.sqrMagnitude == 0f
            || _playerTransform == null)
        {
            ChangeSpeed(agent, 0.0f);
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0)
            {
                agent.navMeshAgent.destination = CalculateRandomPos();
                var randomWaitTime = Random.Range(_minWaitTime, _maxWaitTime);
                _waitTimer = randomWaitTime;
                ChangeSpeed(agent, 0.5f);
                agent.StartCoroutine(StopIfBugged(agent));
            }
        }
    }

    private IEnumerator StopIfBugged(AIAgent agent)
    {
        yield return new WaitForSeconds(_resetTime);
        if (agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.ResetPath();
        }
    }

    public void ExitState(AIAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = agent.config.maxDistance - 0.1f;
    }

    private float CalculateSqrDistance(AIAgent agent)
    {
        var playerPosition = _playerTransform.position;

        return (playerPosition - agent.transform.position).sqrMagnitude;
    }

    private Vector3 CalculateRandomPos()
    {
        var randomDegree = Random.Range(0.0f, 360.0f);
        var randomRadian = randomDegree * Mathf.PI / 180.0f;
        var randomDistance = Random.Range(_minRoamDeviation, _maxRoamDeviation);
        
        var randomX = randomDistance * Mathf.Cos(randomRadian);
        var randomZ = randomDistance * Mathf.Sin(randomRadian);
        
        return new Vector3(randomX + _centerPoint.x, _centerPoint.y, randomZ + _centerPoint.z);
    }
    
    

    private static void ChangeSpeed(AIAgent agent, float newSpeed)
    {
        switch (agent.enemy)
        {
            case Skeleton agentSkeleton:
                agentSkeleton.ChangeSpeed(newSpeed);
                break;
            case Goblin agentGoblin:
                agentGoblin.ChangeSpeed(newSpeed);
                break;
        }
    }
    
}
