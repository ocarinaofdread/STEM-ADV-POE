using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIRangedAttackState : AIState
{
    private Transform _playerTransform;
    private float _timer;
    private float _lookAtSpeed;
    private float _sqrMaxDistance;
    private bool _endAfterLength;
    
    private readonly int _jumpHash = Animator.StringToHash("Jump");
    private readonly int _throwHash = Animator.StringToHash("Throw");
    private readonly int _throwTypeHash = Animator.StringToHash("ThrowType");
    
    public AIStateID GetID()
    {
        return AIStateID.RangedAttack;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void EnterState(AIAgent agent)
    {
        _endAfterLength = true;
        _playerTransform ??= GameObject.FindGameObjectWithTag("Player").transform;

        if (agent.enemy is Golem agentGolem)
        {
            var attackType = agentGolem.GetRandomAttackType();

            if (attackType == 3) { 
                agentGolem.animator.SetTrigger(_jumpHash);
            }
            else
            {

                var jumpMinDistance = agentGolem.jumpAttackDistance - agentGolem.jumpAttackDeviation;
                var jumpMaxDistance = agentGolem.jumpAttackDistance + agentGolem.jumpAttackDeviation;
                var sqrDistance = CalculateSqrDistance(agent);

                if (sqrDistance >= jumpMinDistance * jumpMinDistance &&
                    sqrDistance <= jumpMaxDistance * jumpMaxDistance)
                {
                    agentGolem.animator.SetTrigger(_jumpHash);
                }
                else
                {
                    agentGolem.animator.SetInteger(_throwTypeHash, attackType);
                    agentGolem.animator.SetTrigger(_throwHash);
                    
                    if (attackType == 2) _endAfterLength = false;
                }
            }
        }
        
        agent.enemy.isAttacking = true;
        
        if (_endAfterLength) agent.StartCoroutine(WaitThenChasePlayer(agent));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Update(AIAgent agent)
    {
        
    }

    public void ExitState(AIAgent agent)
    {
        agent.enemy.isAttacking = false;
    }

    private float CalculateSqrDistance(AIAgent agent)
    {
        var playerPosition = _playerTransform.position;

        return (playerPosition - agent.gameObject.transform.position).sqrMagnitude;
    }
    
    private IEnumerator WaitThenChasePlayer(AIAgent agent)
    {
        var animLength = agent.enemy.animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animLength);
        if (!agent.enemy.isDead)
        {
            agent.ChangeState(AIStateID.ChasePlayer);
        }
    }

    public void SwitchToChasePlayer(AIAgent agent)
    {
        agent.ChangeState(AIStateID.ChasePlayer);
    }
}
