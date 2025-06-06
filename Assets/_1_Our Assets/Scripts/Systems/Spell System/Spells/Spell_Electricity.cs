using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spell_Electricity : Spell
{
    [SerializeField] private GameObject rootGameObject;
    [SerializeField] private GameObject particleGameObject;
    [SerializeField] private float destroyAfterEndDelay = 1.0f;
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] private int maxHits = 3;
    [SerializeField] private bool isRoot;

    private AudioSource _audioSource;
    private int _hits;
    
    protected override void RunCastAction()
    {
        rootGameObject ??= gameObject;
        if (isRoot)
        {
            _audioSource = GetComponentInChildren<AudioSource>();
            _audioSource.PlayOneShot(soundEffect);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void End()
    {
        var particleSystems = particleGameObject.GetComponents<ParticleSystem>();
        foreach (var particleSys in particleSystems)
        {
            particleSys.Stop();
        }

        GetComponentInChildren<Collider>().enabled = false;
        
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyAfterEndDelay);
        Destroy(rootGameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hazard"))
        {
            //End();
        }
    }
}
