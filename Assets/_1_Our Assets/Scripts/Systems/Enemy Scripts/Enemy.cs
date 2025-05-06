using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int health;
    public bool isDead;
    public bool isAttacking;
    
    [SerializeField] float deathDestroyDelay = 5.0f;
    [SerializeField] private bool damagesAdditive;

    private AIAgent _agent;
    private int _additiveDamages = 0;
    private readonly int _damageAdditiveHash = Animator.StringToHash("DamageAdditive");

    private void Start()
    {
        _agent = GetComponent<AIAgent>();
    }
    
    private void Update()
    {
        if (health <= 0 && !isDead)
        {
            _agent.ChangeState(AIStateID.Death);
            isDead = true;
            Destroy(gameObject, deathDestroyDelay);
        }

        if (isDead) return;
        
        var damageWeight = animator.GetLayerWeight(animator.GetLayerIndex("Damage"));
        
        switch (_additiveDamages)
        {
            case 0 when damageWeight > 0.0f:
                animator.SetLayerWeight(animator.GetLayerIndex("Damage"), 0);
                //Debug.Log("damage weight set to 0");
                break;
            case > 0 when damageWeight < 1.0f:
                animator.SetLayerWeight(animator.GetLayerIndex("Damage"), 1);
                break;
        }
    }

    public void Damage()
    {
        if (_agent.enemy.isDead) return;
        
        if (damagesAdditive)
        {
            DamageAdditive();
        }
        else
        {
            _agent.ChangeState(AIStateID.DamageStop);
        }
    }

    private void DamageAdditive()
    {
        if (!isAttacking && !isDead)
        {
            _additiveDamages++;
            animator.SetTrigger(_damageAdditiveHash);
            StartCoroutine(WaitThenReduce(_agent));
        }
    }
    
    private IEnumerator WaitThenReduce(AIAgent agent)
    {
        //var animLength = agent.enemy.animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(0.35f);//animLength);
        
        if (agent.enemy.isDead) yield break;
        
        _additiveDamages--;
        //agent.ChangeState(AIStateID.Idle);
    }
}
