using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : AIState
{
    private Transform _playerTransform;
    private float _timer;
    private readonly int _attackHash = Animator.StringToHash("Attack");
    
    public AIStateID GetID()
    {
        return AIStateID.Attack;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void EnterState(AIAgent agent)
    {
        _playerTransform ??= GameObject.FindGameObjectWithTag("Player").transform;
        
        if (agent.enemy is Skeleton agentSkeleton)
        {
            agentSkeleton.animator.SetTrigger(_attackHash);
        }

        agent.StartCoroutine(WaitThenIdle(agent));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Update(AIAgent agent)
    {
        
        
    }

    public void ExitState(AIAgent agent)
    {
        
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
