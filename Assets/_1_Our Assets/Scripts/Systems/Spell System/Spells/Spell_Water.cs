using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spell_Water : Spell
{
    [SerializeField] private float launchSpeed = 1;
    [SerializeField] private float pushForce = 1;
    [SerializeField] private ForceMode pushForceMode;
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
    
    private void OnCollisionEnter(Collision collision)
    {
        var otherGameObject = collision.gameObject;
        if (otherGameObject.CompareTag("Hazard"))
        {
            if (otherGameObject.GetComponent<Enemy>())
            {
                var pushDirection = otherGameObject.transform.position - transform.position;
                var otherRb = otherGameObject.GetComponentInChildren<Rigidbody>();
                
                otherRb.AddForce(pushDirection * pushForce, pushForceMode);
            }

            End();
        }
    }
}
