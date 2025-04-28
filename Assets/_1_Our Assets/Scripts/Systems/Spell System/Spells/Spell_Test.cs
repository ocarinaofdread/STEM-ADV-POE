using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spell_Test : Spell
{
    [SerializeField] private float launchSpeed = 1;
    private Rigidbody rb;
    
    protected override void RunCastAction()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * launchSpeed, ForceMode.Impulse);
    }

    protected override void AfterDuration()
    {
        Destroy(gameObject);
    }
}
