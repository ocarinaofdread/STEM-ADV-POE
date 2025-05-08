using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    [SerializeField] private Collider attackCollider;
    [SerializeField] private GameObject animRockObject;
    [SerializeField] private GameObject throwRockPrefab;
    [SerializeField] private float throwAngle = 45.0f;
    
    private readonly int _speedAnimHash = Animator.StringToHash("Speed");
    
    
    public void ChangeSpeed(float speed)
    {
        animator ??= GetComponent<Animator>();
        animator.SetFloat(_speedAnimHash, speed);
    }
    
    public void EnableAttackCollider() { attackCollider.enabled = true; }
    public void DisableAttackCollider() { attackCollider.enabled = false; }
    
    public void EnableAnimationRock() { animRockObject.SetActive(true); }
    public void DisableAnimationRock() { animRockObject.SetActive(false); }

    public void LaunchRock()
    {
        var projectile = Instantiate(throwRockPrefab, animRockObject.transform.position, 
                                            animRockObject.transform.rotation);
        projectile.GetComponent<LaunchRockProjectile>().Launch(); //throwAngle, 
        //    GameObject.FindGameObjectWithTag("Player").transform);
    }
    
    
    
}
