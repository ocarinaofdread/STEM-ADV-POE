using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : AIState
{
    private Transform _playerTransform;
    private float _timer;
    private readonly int _attackHash = Animator.StringToHash("Attack");
    private readonly int _attackTypeHash = Animator.StringToHash("AttackType");
    
    public AIStateID GetID()
    {
        return AIStateID.Attack;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void EnterState(AIAgent agent)
    {
        _playerTransform ??= GameObject.FindGameObjectWithTag("Player").transform;
        
        switch (agent.enemy)
        {
            case Skeleton agentSkeleton:
                agentSkeleton.animator.SetTrigger(_attackHash);
                break;
            case Goblin agentGoblin:
                agentGoblin.animator.SetTrigger(_attackHash);
                break;
            case Golem agentGolem:
                // deprecated random attack type, now based on distance
                //agentGolem.animator.SetInteger(_attackTypeHash, 
                //                agentGolem.GetRandomAttackType());
                agentGolem.animator.SetTrigger(_attackHash);
                break;
        }

        agent.enemy.isAttacking = true;

        agent.StartCoroutine(WaitThenIdle(agent));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Update(AIAgent agent)
    {
        
        
    }

    public void ExitState(AIAgent agent)
    {
        agent.enemy.isAttacking = false;
    }

    private IEnumerator WaitThenIdle(AIAgent agent)
    {
        var animLength = agent.enemy.animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animLength);
        if (!agent.enemy.isDead)
        {
            agent.ChangeState(AIStateID.Idle);
        }
    }
}
