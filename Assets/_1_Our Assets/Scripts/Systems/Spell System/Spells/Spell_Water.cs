using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Spell_Water : Spell
{
    [SerializeField] private float launchSpeed = 1;
    [SerializeField] private float pushForce = 1;
    [SerializeField] private ForceMode pushForceMode;
    [SerializeField] private float pushTime = 2.0f;
    private Rigidbody _rigidbody;
    
    protected override void RunCastAction()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward * launchSpeed, ForceMode.Impulse);
    }

    public override void End()
    {
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var otherGameObject = other.gameObject;
        Debug.Log("SpellWater: Collision has been entered (" + otherGameObject.name + ").");
        if (otherGameObject.CompareTag("Hazard"))
        {
            Debug.Log("SpellWater: Other object has Hazard tag.");
            if (otherGameObject.transform.root.GetComponentInChildren<Enemy>())
            {
                Debug.Log("SpellWater: Other object has Enemy script.");
                StartCoroutine(Timer(otherGameObject));
            }

            End();
        }
    }

    private IEnumerator Timer(GameObject otherGameObject)
    {
        var pushDirection = _rigidbody.velocity.normalized;
        var otherRb = otherGameObject.transform.root.GetComponentInChildren<Rigidbody>();
        //var otherNm = otherGameObject.transform.root.GetComponentInChildren<NavMeshAgent>();

        //otherNm.enabled = false;
        Debug.Log("SpellWater: Pushing other object with " + pushDirection + " vector.");
        otherRb.AddForce(pushDirection * pushForce, pushForceMode);
        yield return new WaitForSeconds(pushTime);
        //otherNm.enabled = true;
    }
}
