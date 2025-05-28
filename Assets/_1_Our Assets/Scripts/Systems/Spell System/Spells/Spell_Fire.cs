using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spell_Fire : Spell
{
    [SerializeField] private float launchSpeed = 1;
    [SerializeField] private AudioClip castSound;
    private Rigidbody rb;
    
    protected override void RunCastAction()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * launchSpeed, ForceMode.Impulse);
    }

    public override void End()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Hazard"))
        {
            if (other.transform.root.GetComponentInChildren<Enemy>())
            {
                return;
            }
        }
        End();
    }

    public AudioClip GetCastAudio()
    {
        return castSound;
    }
}
