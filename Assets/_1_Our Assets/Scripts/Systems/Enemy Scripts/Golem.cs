using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Golem : Enemy
{
    [SerializeField] private Collider rightAttackCollider;
    [SerializeField] private Collider leftAttackCollider;
    [SerializeField] private Collider footAttackCollider;
    
    [SerializeField] private GameObject animRockObject;
    [SerializeField] private GameObject throwRockPrefab;
    [SerializeField] private Transform throwStartPoint;
    [SerializeField] private float throwTurnSpeed = 2.0f;
    
    // [ID of attack animation, number of chances to be selected at random]
    // 1 = punch, 2 = slash, 3 = stomp
    [SerializeField] private Vector2Int[] attackAnimationRates;
    // 1 = boulder, 2 = boulder x3, 3 = jump
    [SerializeField] private Vector2Int[] rangedAnimationRates;
    
    public float jumpAttackDistance = 5.19f;
    public float jumpAttackDeviation = 0.10f;

    [SerializeField] private float destroyDelayAfterAnim;
    
    private GameManager _gameManager;
    private List<int> _attackAnimRatePool;
    private List<int> _rangedAnimRatePool;
    private AIAgent _aiAgent;
    private Transform _playerTransform;

    private bool _lookAtDuringThrow;
    
    private readonly int _speedAnimHash = Animator.StringToHash("Speed");
    
    public new void Start()
    {
        base.Start();
        _attackAnimRatePool = CreateAnimationPool(attackAnimationRates);
        _rangedAnimRatePool = CreateAnimationPool(rangedAnimationRates);
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    public void Victory()
    {
        StartCoroutine(DeathDestroy());
    }

    private IEnumerator DeathDestroy()
    {
        _gameManager = FindObjectOfType<GameManager>(true);
        _gameManager.EndGame(false);
        
        yield return new WaitForSeconds(destroyDelayAfterAnim);
        Destroy(gameObject);
    }
    
    public void ChangeSpeed(float speed)
    {
        animator ??= GetComponent<Animator>();
        animator.SetFloat(_speedAnimHash, speed);
    }

    private void LateUpdate()
    {
        if (!_lookAtDuringThrow) return;
        
        var positionToLookAt = _playerTransform.position;
        positionToLookAt.y = transform.position.y;
        
        var targetRotation =
            Quaternion.LookRotation(positionToLookAt - transform.position);
        
        transform.rotation = 
            Quaternion.Lerp(transform.rotation, targetRotation, 
                            throwTurnSpeed * Time.deltaTime);
    }
    
    public void LaunchRock()
    {
        var projectile = Instantiate(throwRockPrefab, animRockObject.transform.position, 
                                                throwStartPoint.transform.rotation);
        projectile.GetComponent<LaunchRockProjectile>().Launch(animRockObject.transform, 
                                                                _playerTransform); 
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
        _aiAgent ??= GetComponent<AIAgent>();
        (_aiAgent.GetState(AIStateID.RangedAttack) as AIRangedAttackState)?.SwitchToChasePlayer(_aiAgent);
    }

    public void EnableLook() { _lookAtDuringThrow = true; }
    public void DisableLook() { _lookAtDuringThrow = false; }
    
    public void EnableRightAttackCollider() { rightAttackCollider.enabled = true; }
    public void DisableRightAttackCollider() { rightAttackCollider.enabled = false; }
    
    public void EnableLeftAttackCollider() { leftAttackCollider.enabled = true; }
    public void DisableLeftAttackCollider() { leftAttackCollider.enabled = false; }
    
    public void EnableFootAttackCollider() { footAttackCollider.enabled = true; }
    public void DisableFootAttackCollider() { footAttackCollider.enabled = false; }
    
    public void EnableAnimationRock() { animRockObject.SetActive(true); }
    public void DisableAnimationRock() { animRockObject.SetActive(false); }
}
