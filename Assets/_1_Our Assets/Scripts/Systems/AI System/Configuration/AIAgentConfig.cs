using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AIAgentConfig : ScriptableObject
{
    public float maxDistance = 1.0f;
    
    // Roam Settings
    public float maxSightRange = 4.0f;
    public float minRoamDeviation = 1.0f;
    public float maxRoamDeviation = 3.0f;
    public float minRoamWaitTime = 1.0f;
    public float maxRoamWaitTime = 3.0f;
    public float roamResetTime = 3.0f;
    
    // Idle Settings
    public float idleLookSpeed = 1.0f;
    
    // ChasePlayer Settings
    public float maxNavMeshRefreshTime = 1.0f;
    
    // Attack Settings
    public float minAttackWaitTime = 1.0f;
    public float maxAttackWaitTime = 5.0f;
    public float closeAttackRange = 1.5f;
    
    
    // Ranged Attack Settings
    public float minRangedWaitTime = 4.0f;
    public float maxRangedWaitTime = 6.0f;
}
