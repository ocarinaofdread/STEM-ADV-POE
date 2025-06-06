using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIIdleState : AIState
{
    private Transform _playerTransform;
    private float _timer;
    private float _lookAtSpeed;
    private float _sqrMaxDistance;
    private float _closeAttackRange;
    private bool _isGolem;

    private readonly int _attackTypeHash = Animator.StringToHash("AttackType");
    
    public AIStateID GetID()
    {
        return AIStateID.Idle;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void EnterState(AIAgent agent)
    {
        _playerTransform ??= GameObject.FindGameObjectWithTag("Player").transform;
        _sqrMaxDistance = agent.config.maxDistance * agent.config.maxDistance;
        _closeAttackRange = agent.config.closeAttackRange;
        
        switch (agent.enemy)
        {
            case Skeleton agentSkeleton:
                agentSkeleton.ChangeSpeed(0.0f);
                break;
            case Goblin agentGoblin:
                agentGoblin.ChangeSpeed(0.0f);
                break;
            case Golem agentGolem:
                agentGolem.ChangeSpeed(0.0f);
                _isGolem = true;
                break;
        }

        _timer = Random.Range(agent.config.minAttackWaitTime, agent.config.maxAttackWaitTime);
        _lookAtSpeed = agent.config.idleLookSpeed;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Update(AIAgent agent)
    {
        var sqrDistance = CalculateSqrDistance(agent);
        if (sqrDistance > _sqrMaxDistance)
        {
            agent.ChangeState(AIStateID.ChasePlayer);
        }

        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            //Debug.Log("Changing to Attack state");s
            if (_isGolem)
            {
                var agentGolem = agent.enemy as Golem;
                //Debug.Log("Distance squared: " + sqrDistance + " vs. Close Squared " + Mathf.Pow(_closeAttackRange, 2));
                agentGolem.animator.SetInteger(_attackTypeHash,
                    sqrDistance <= _closeAttackRange * _closeAttackRange ? agentGolem.GetRandomAttackType() : 2);
                //Debug.Log("Golem square distance is " + sqrDistance + " which is related to sqr close atk range "
                //          + Mathf.Pow(_closeAttackRange, 2));
            }

            agent.ChangeState(AIStateID.Attack);
        }

        var thisPosition = agent.transform.position;
        var playerPosition = _playerTransform.position;
        playerPosition.y = thisPosition.y;
        var targetRotation =
            Quaternion.LookRotation(playerPosition - thisPosition);
        
        agent.transform.rotation = 
            Quaternion.Lerp(agent.transform.rotation, targetRotation, _lookAtSpeed * Time.deltaTime);
    }

    public void ExitState(AIAgent agent)
    {
        
    }

    private float CalculateSqrDistance(AIAgent agent)
    {
        var playerPosition = _playerTransform.position;

        return (playerPosition - agent.gameObject.transform.position).sqrMagnitude;
    }
}
