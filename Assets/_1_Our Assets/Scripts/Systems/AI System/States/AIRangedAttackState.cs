using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIRangedAttackState : AIState
{
    private Transform _playerTransform;
    private float _timer;
    private float _lookAtSpeed;
    private float _sqrMaxDistance;
    
    public AIStateID GetID()
    {
        return AIStateID.RangedAttack;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void EnterState(AIAgent agent)
    {
        _playerTransform ??= GameObject.FindGameObjectWithTag("Player").transform;

        if (agent.enemy is Golem agentGolem)
        {
            var distanceToJump = Random.Range(agentGolem.jumpAttackDistance - agentGolem.jumpAttackDeviation,
                                              agentGolem.jumpAttackDistance + agentGolem.jumpAttackDeviation);

            if (CalculateSqrDistance(agent) >= (agentGolem.jumpAttackDistance - agentGolem.jumpAttackDeviation) ||
                CalculateSqrDistance(agent) >= (agentGolem.jumpAttackDistance - agentGolem.jumpAttackDeviation))
            {
                // Jump
            }
            else
            {
                // Boulder
            }
        }

        agent.enemy.isAttacking = true;
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
            Debug.Log("Changing to Attack state");
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
