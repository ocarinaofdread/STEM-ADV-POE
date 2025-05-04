using System.Collections;
using UnityEngine;

public class AIDamageStopState : AIState
{
    private readonly int _damageHash = Animator.StringToHash("DamageStop");
    
    public AIStateID GetID()
    {
        return AIStateID.DamageStop;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void EnterState(AIAgent agent)
    {
        if (agent.enemy is Skeleton agentSkeleton)
        {
            agentSkeleton.animator.SetTrigger(_damageHash);    
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
