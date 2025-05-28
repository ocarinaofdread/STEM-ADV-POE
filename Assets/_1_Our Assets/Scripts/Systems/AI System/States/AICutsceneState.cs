using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICutsceneState : AIState
{
    
    public AIStateID GetID()
    {
        return AIStateID.Cutscene;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void EnterState(AIAgent agent)
    {
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Update(AIAgent agent)
    {
        
    }

    public void ExitState(AIAgent agent)
    {
        
    }

    public void BeginBattle()
    {
        
    }
}
