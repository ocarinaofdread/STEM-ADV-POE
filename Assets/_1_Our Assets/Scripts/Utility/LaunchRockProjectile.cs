using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class LaunchRockProjectile : MonoBehaviour
{
    //[SerializeField] private Transform targetTransform;
    // [SerializeField] private Transform playerTransform;
    [SerializeField] private bool launchLocal;
    [SerializeField] private Vector3 launchDirection;
    [SerializeField] private float launchSpeed;
    [SerializeField] private float calculationFactorMin = 2.2705f;
    [SerializeField] private float calculationFactorMax = 2.2705f;

    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Launch();
        Debug.Log("Starting Position: " + transform.position);
    }

    public void Launch(Transform endRotationTransform)//float launchAngle, Transform targetTransform)
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddRelativeForce(launchDirection * launchSpeed, ForceMode.VelocityChange);
        transform.rotation = endRotationTransform.rotation;
    }

    public void Launch(Transform endRotationTransform, Transform endPositionTransform)
    {
        var startTransformZero = transform.position;
        var endTransformZero = endPositionTransform.position;
        startTransformZero.y = 0;
        endTransformZero.y = 0;
        
        var distance = (endTransformZero - startTransformZero).magnitude;
        var speed = distance / Random.Range(calculationFactorMin, calculationFactorMax);
        
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddRelativeForce(launchDirection * speed, ForceMode.VelocityChange);
        transform.rotation = endRotationTransform.rotation;

    }

    private void Launch()//float launchAngle, Transform targetTransform)
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddRelativeForce(launchDirection * launchSpeed, ForceMode.VelocityChange);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Ending Position: " + transform.position);
    }

}
