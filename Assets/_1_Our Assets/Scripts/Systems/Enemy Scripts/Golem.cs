using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    [SerializeField] private Collider attackCollider;
    [SerializeField] private Collider footAttackCollider;
    
    [SerializeField] private GameObject animRockObject;
    [SerializeField] private GameObject throwRockPrefab;
    [SerializeField] private Transform throwStartPoint;
    [SerializeField] private Transform throwTarget;
    
    // [ID of attack animation, number of chances to be selected at random]
    // 1 = punch, 2 = slash, 3 = stomp
    [SerializeField] private Vector2Int[] attackAnimationRates;
    // 1 = boulder, 2 = boulder x3, 3 = jump
    [SerializeField] private Vector2Int[] rangedAnimationRates;

    public float jumpAttackDistance = 5.19f;
    public float jumpAttackDeviation = 0.10f;
    
    private List<int> _attackAnimRatePool;
    private List<int> _rangedAnimRatePool;
    private AIAgent _agent;
    private Transform _playerTransform;
    
    private readonly int _speedAnimHash = Animator.StringToHash("Speed");
    
    private void Start()
    {
        _attackAnimRatePool = CreateAnimationPool(attackAnimationRates);
        _rangedAnimRatePool = CreateAnimationPool(rangedAnimationRates);
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    public void ChangeSpeed(float speed)
    {
        animator ??= GetComponent<Animator>();
        animator.SetFloat(_speedAnimHash, speed);
    }
    
    public void EnableAttackCollider() { attackCollider.enabled = true; }
    public void DisableAttackCollider() { attackCollider.enabled = false; }
    
    public void EnableFootAttackCollider() { footAttackCollider.enabled = true; }
    public void DisableFootAttackCollider() { footAttackCollider.enabled = false; }
    
    public void EnableAnimationRock() { animRockObject.SetActive(true); }
    public void DisableAnimationRock() { animRockObject.SetActive(false); }

    public void LaunchRock()
    {
        var projectile = Instantiate(throwRockPrefab, animRockObject.transform.position, 
                                                throwStartPoint.transform.rotation);
        projectile.GetComponent<LaunchRockProjectile>().Launch(animRockObject.transform, 
                                                            throwTarget); 
    }

    private List<int> CreateAnimationPool(Vector2Int[] rates)
    {
        var animationPool = new List<int>();
        
        foreach (var attackAnimRate in rates)
        {
            var thisAnim = new int[attackAnimRate.y];
            for (var i = 0; i < thisAnim.Length; i++) {
                thisAnim[i] = attackAnimRate.x;
            }
            animationPool.AddRange(thisAnim);
        }
        
        return animationPool;
    }

    public int GetRandomAttackType()
    {
        var index = Random.Range(0, _attackAnimRatePool.Count - 1);

        return _attackAnimRatePool[index];
    }

    public int GetRandomRangedType()
    {
        var index = Random.Range(0, _rangedAnimRatePool.Count - 1);

        return _rangedAnimRatePool[index];
    }
    
    
    // For Ranged Attack - multiple throws
    public void EndRangedAttack()
    {
        _agent ??= GetComponent<AIAgent>();
        (_agent.GetState(AIStateID.RangedAttack) as AIRangedAttackState).SwitchToChasePlayer(_agent);
    }

    public void LookAtPlayer()
    {
        // insert code here
    }
}
