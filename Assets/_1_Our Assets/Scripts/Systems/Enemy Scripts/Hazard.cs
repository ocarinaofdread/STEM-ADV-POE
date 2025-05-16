using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private int damageMin;
    [SerializeField] private int damageMax;
    
    public int GetDamage()
    {
        return damageMin == damageMax ? damageMin : Random.Range(damageMin, damageMax);
    }
}
