using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public AIAgentConfig config;
    public Enemy enemy;
    
    [SerializeField] private AIStateID initialState;
    
    private AIStateMachine stateMachine;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemy ??= GetComponent<Enemy>();
        navMeshAgent.stoppingDistance = config.maxDistance;
        
        stateMachine = new AIStateMachine(this);
        if (enemy is Skeleton)
        {
            stateMachine.RegisterState(new AIChasePlayerState());
            stateMachine.RegisterState(new AIIdleState());
            stateMachine.RegisterState(new AIAttackState());
            stateMachine.RegisterState(new AIRangedAttackState());
            stateMachine.RegisterState(new AIDeathState());
            stateMachine.RegisterState(new AIRoamState());
        }
        
        stateMachine.ChangeState(initialState);
    }

    void Update()
    {
        stateMachine.Update();
    }

    public void ChangeState(AIStateID newState)
    {
        stateMachine.ChangeState(newState);
    }
}
