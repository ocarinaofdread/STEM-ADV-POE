using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public Collider hitboxCollider;
    
    public int health;
    public bool isDead;
    public bool isAttacking;

    [SerializeField] private float deathDestroyDelay = 5.0f;
    [SerializeField] private int damageStopThreshold;
    [SerializeField] private float additiveDamageDelay = 0.35f;

    [SerializeField] private AIAgent agent;
    [SerializeField] private HealthSystemForDummies healthSystem;

    private Rigidbody _rigidbody;
    private int _additiveDamages;
    private bool _damageAdditiveNow;
    private readonly int _damageAdditiveHash = Animator.StringToHash("DamageAdditive");
    private List<int> _lastFiveSpellObjects;

    public void Start()
    {
        agent ??= GetComponent<AIAgent>();
        healthSystem ??= GetComponent<HealthSystemForDummies>();
        hitboxCollider ??= GetComponent<Collider>();
        _rigidbody = transform.root.GetComponentInChildren<Rigidbody>();
        
        healthSystem.MaximumHealth = health;
        healthSystem.CurrentHealth = health;

        _lastFiveSpellObjects = new List<int>(0);
    }

    private void Update()
    {
        if (health <= 0 && !isDead)
        {
            agent.ChangeState(AIStateID.Death);
            healthSystem.CurrentHealth = 0;
            isDead = true;
            Destroy(gameObject, deathDestroyDelay);
        }

        if (isDead) return; // || !_damageAdditiveNow) return;

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

    private void Damage(bool animate, Spell otherSpell)
    {
        if (health <= 0) return;

        // Health management insert here
        
        if (!animate) return;
        
        
        if (SpellDamagesAdditive(otherSpell))
        {
            DamageAdditive();
        }
        else
        {
            agent.ChangeState(AIStateID.DamageStop);
        }
    }

    private void DamageAdditive()
    {
        if (isAttacking || isDead) { return; }
        
        
        _additiveDamages++;
        animator.SetTrigger(_damageAdditiveHash);
        StartCoroutine(WaitThenReduce(agent));
    }
    
    private IEnumerator WaitThenReduce(AIAgent agent)
    {
        yield return new WaitForSeconds(additiveDamageDelay);
        
        if (agent.enemy.isDead) yield break;
        
        _additiveDamages--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Spell") || isDead
            || SpellAlreadyEncountered(other.gameObject)) return;
        
        AddSpellInstance(other.gameObject);
        
        var otherSpell = other.GetComponent<Spell>();
        health -= otherSpell.GetDamage();
        healthSystem.AddToCurrentHealth(-otherSpell.GetDamage());
        
        otherSpell.End();
        Damage(true, otherSpell);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        _rigidbody.velocity = Vector3.zero;
    //    }
    //}
    
    private void AddSpellInstance(GameObject newSpell)
    {
        _lastFiveSpellObjects.Add(newSpell.GetInstanceID());
        
        if (_lastFiveSpellObjects.Count <= 5) return;
            
        _lastFiveSpellObjects.Remove(0);
    }

    private bool SpellAlreadyEncountered(GameObject other)
    {
        // returns true if the spell has already been encountered
        // (prevents enemies from going in and out of spells to accumulate massive damage)
        return _lastFiveSpellObjects.Count != 0 && _lastFiveSpellObjects.Any(instanceId => other.GetInstanceID() 
                                                                             == instanceId);
    }

    private bool SpellDamagesAdditive(Spell spell)
    {
        // true if the additive level is less than the stop threshold
        return spell.GetDamageAdditiveLevel() < damageStopThreshold;
    }
    
}
