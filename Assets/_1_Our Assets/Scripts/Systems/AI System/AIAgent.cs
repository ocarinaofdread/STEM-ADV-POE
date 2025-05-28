using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public AIAgentConfig config;
    public Enemy enemy;
    
    [SerializeField] private AIStateID initialState;
    
    private AIStateMachine stateMachine;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemy ??= GetComponent<Enemy>();
        navMeshAgent.stoppingDistance = config.maxDistance;
        
        stateMachine = new AIStateMachine(this);
        switch (enemy)
        {
            case Skeleton:
                stateMachine.RegisterState(new AIChasePlayerState());
                stateMachine.RegisterState(new AIIdleState());
                stateMachine.RegisterState(new AIAttackState());
                stateMachine.RegisterState(new AIRangedAttackState());
                stateMachine.RegisterState(new AIDeathState());
                stateMachine.RegisterState(new AIRoamState());
                stateMachine.RegisterState(new AIDamageStopState());
                break;
            case Goblin:
                stateMachine.RegisterState(new AIChasePlayerState());
                stateMachine.RegisterState(new AIIdleState());
                stateMachine.RegisterState(new AIAttackState());
                stateMachine.RegisterState(new AIDeathState());
                stateMachine.RegisterState(new AIRoamState());
                stateMachine.RegisterState(new AIDamageStopState());
                break;
            case Golem:
                stateMachine.RegisterState(new AIChasePlayerState());
                stateMachine.RegisterState(new AIChasePlayerState());
                stateMachine.RegisterState(new AIIdleState());
                stateMachine.RegisterState(new AIAttackState());
                stateMachine.RegisterState(new AIDeathState());
                stateMachine.RegisterState(new AIRangedAttackState());
                stateMachine.RegisterState(new AIDamageStopState());
                stateMachine.RegisterState(new AICutsceneState());
                break;
        }
        
        
        stateMachine.ChangeState(initialState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void ChangeState(AIStateID newState)
    {
        stateMachine.ChangeState(newState);
    }

    public AIState GetState(AIStateID stateToGet)
    {
        return stateMachine.GetState(stateToGet);
    }
}
