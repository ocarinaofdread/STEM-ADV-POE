using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    private Animator _animator;
    private readonly int _speedAnimHash = Animator.StringToHash("Speed");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void ChangeSpeed(float speed)
    {
        _animator.SetFloat(_speedAnimHash, speed);
    }
}
