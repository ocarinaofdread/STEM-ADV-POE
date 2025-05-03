using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DrawNavMeshPath : MonoBehaviour
{
    [SerializeField] private bool showVelocity;
    [SerializeField] private Color velocityColor;
    [SerializeField] private bool showDesiredVelocity;
    [SerializeField] private Color desiredVelocityColor;
    [SerializeField] private bool showPath;
    [SerializeField] private Color pathColor;
    
    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || !_navMeshAgent.hasPath) { return; }

        if (showVelocity)
        {
            Gizmos.color = velocityColor;
            Gizmos.DrawLine(transform.position, transform.position + _navMeshAgent.velocity);
        }

        if (showDesiredVelocity)
        {
            Gizmos.color = desiredVelocityColor;
            Gizmos.DrawLine(transform.position, transform.position + _navMeshAgent.desiredVelocity);
        }

        if (showPath)
        {
            Gizmos.color = pathColor;
            var path = _navMeshAgent.path;
            Vector3 previousCorner = transform.position;
            foreach (var corner in path.corners)
            {
                Gizmos.DrawLine(previousCorner, corner);
                Gizmos.DrawSphere(corner, 0.1f);
                previousCorner = corner;
            }
        }
    }
}
