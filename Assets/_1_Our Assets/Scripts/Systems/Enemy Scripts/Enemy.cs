using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void EnableCollider(Collider collider) { collider.enabled = true; }
    public void DisableCollider(Collider collider) { collider.enabled = false; }
}
