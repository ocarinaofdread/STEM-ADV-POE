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

    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Launch();
    }

    public void Launch(Transform startTransform)//float launchAngle, Transform targetTransform)
    {
        _rigidbody.velocity = Vector3.zero;

        Vector3 localDirection = startTransform.InverseTransformDirection(launchDirection);
        _rigidbody.AddRelativeForce(localDirection * launchSpeed, ForceMode.VelocityChange);
    }

    private void Launch()//float launchAngle, Transform targetTransform)
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddRelativeForce(launchDirection * launchSpeed, ForceMode.VelocityChange);
    }

}
