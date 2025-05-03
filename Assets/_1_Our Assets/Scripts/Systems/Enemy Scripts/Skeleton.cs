using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    [SerializeField] private Collider attackCollider;
    
    private readonly int _speedAnimHash = Animator.StringToHash("Speed");
    
    public void ChangeSpeed(float speed)
    {
        animator ??= GetComponent<Animator>();
        animator.SetFloat(_speedAnimHash, speed);
    }
    
    public void EnableAttackCollider() { attackCollider.enabled = true; }
    public void DisableAttackCollider() { attackCollider.enabled = false; }
}
