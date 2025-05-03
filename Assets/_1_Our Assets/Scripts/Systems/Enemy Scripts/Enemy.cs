using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int health;
    public bool isDead;
    
    [SerializeField] float deathDestroyDelay = 5.0f;

    private AIAgent agent;

    private void Start()
    {
        agent = GetComponent<AIAgent>();
    }
    
    private void Update()
    {
        if (health <= 0 && !isDead)
        {
            agent.ChangeState(AIStateID.Death);
            isDead = true;
            Destroy(gameObject, deathDestroyDelay);
        }
        
    }
}
