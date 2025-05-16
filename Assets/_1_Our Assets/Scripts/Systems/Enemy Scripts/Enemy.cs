using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public Collider hitboxCollider;
    
    public int health;
    public bool isDead;
    public bool isAttacking;

    [SerializeField] float deathDestroyDelay = 5.0f;
    [SerializeField] private bool damagesAdditive;
    [SerializeField] private float additiveDamageDelay = 0.35f;

    [SerializeField] private AIAgent _agent;
    private int _additiveDamages;
    private readonly int _damageAdditiveHash = Animator.StringToHash("DamageAdditive");

    private void Start()
    {
        _agent ??= GetComponent<AIAgent>();
        hitboxCollider ??= GetComponent<Collider>();
    }

    private void Update()
    {
        if (health <= 0 && !isDead)
        {
            _agent.ChangeState(AIStateID.Death);
            isDead = true;
            Destroy(gameObject, deathDestroyDelay);
        }

        if (isDead || !damagesAdditive) return;

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

    private void Damage(bool animate)
    {
        if (health <= 0) return;

        // Health management insert here
        
        if (!animate) return;
        
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
        if (isAttacking || isDead) { return; }
        
        
        _additiveDamages++;
        animator.SetTrigger(_damageAdditiveHash);
        StartCoroutine(WaitThenReduce(_agent));
    }
    
    private IEnumerator WaitThenReduce(AIAgent agent)
    {
        yield return new WaitForSeconds(additiveDamageDelay);
        
        if (agent.enemy.isDead) yield break;
        
        _additiveDamages--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Spell")) return;
        
        var otherSpell = other.GetComponent<Spell>();
        health -= otherSpell.GetDamage();
        otherSpell.End();
        Damage(true);
    }

    public void LogPosition()
    {
        Debug.Log(transform.position);
    }
}
