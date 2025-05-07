using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class LaunchRockProjectile : MonoBehaviour
{
    [SerializeField] private float launchSpeed;
    [SerializeField] private Vector3 launchDirection;
    [SerializeField] private bool launchLocal;

    private Rigidbody _rigidbody;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Launch();
    }

    private void Launch()
    {
        if (launchLocal)
        {
            //launchDirection = GetLocalDirection(launchDirection);
        }

        Debug.Log("Launch speed * Launch direction: " + (launchSpeed * launchDirection));
        _rigidbody.AddForce(launchSpeed * launchDirection, ForceMode.Impulse);
    }

    private Vector3 GetLocalDirection(Vector3 direction)
    {
        return transform.InverseTransformPoint(direction);
    }
}
