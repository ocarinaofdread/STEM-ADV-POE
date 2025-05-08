using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class LaunchRockProjectile : MonoBehaviour
{
    [SerializeField] private float launchAngle;
    [SerializeField] private Transform targetTransform;
    // [SerializeField] private Transform playerTransform;
    [SerializeField] private bool launchLocal;

    private Rigidbody _rigidbody;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Launch();
    }

    private void Launch()
    {
        _rigidbody.velocity = BallisticVel(targetTransform, launchAngle);
    }
    
    
    // Code taken from aldonaletto on Unity Discussions
    
    Vector3 BallisticVel(Transform target, float angle){
        Vector3 dir = target.position - transform.position;  // get target direction
        float h = dir.y;  // get height difference
        
        dir.y = 0;  // retain only the horizontal direction
        float dist = dir.magnitude ;  // get horizontal distance
        float a = angle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
        dist += h / Mathf.Tan(a);  // correct for small height differences
        // calculate the velocity magnitude
        float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return vel * dir.normalized;
    }

}
