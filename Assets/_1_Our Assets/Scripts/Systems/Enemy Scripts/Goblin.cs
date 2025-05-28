using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    [SerializeField] private Collider attackCollider;
    [SerializeField] private AudioSource faceAudioSource;
    [SerializeField] private AudioSource swordAudioSource;
    
    private readonly int _speedAnimHash = Animator.StringToHash("Speed");
    
    public void ChangeSpeed(float speed)
    {
        animator ??= GetComponent<Animator>();
        animator.SetFloat(_speedAnimHash, speed);
    }
    
    public void EnableAttackCollider() { attackCollider.enabled = true; }
    public void DisableAttackCollider() { attackCollider.enabled = false; }
    public void PlayFaceSoundEffect(AudioClip clip) { faceAudioSource.PlayOneShot(clip); }
    public void PlaySwordSoundEffect(AudioClip clip) { swordAudioSource.PlayOneShot(clip); }
    
}
