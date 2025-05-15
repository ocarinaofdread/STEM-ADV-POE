using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIDeathState : AIState
{
    private readonly int deathHash = Animator.StringToHash("Death");
    
    public AIStateID GetID()
    {
        return AIStateID.Death;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void EnterState(AIAgent agent)
    {
        agent.enemy.animator.SetTrigger(deathHash);

        switch (agent.enemy)
        {
            case Skeleton agentSkeleton:
                agentSkeleton.animator.SetLayerWeight(agentSkeleton.animator.GetLayerIndex("Damage"), 0);
                break;
            case Golem agentGolem:
                agentGolem.animator.SetLayerWeight(agentGolem.animator.GetLayerIndex("Damage"), 0);
                break;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Update(AIAgent agent)
    {
        
    }

    public void ExitState(AIAgent agent)
    {
        
    }
}
