using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private bool hasDuration;
    [SerializeField] private float damage;
    [SerializeField] private float manaCost;
    [SerializeField] private float duration;

    private void Awake()
    {
        RunCastAction();
        if (hasDuration)
        {
            StartCoroutine(WaitDuration());
        }
    }

    IEnumerator WaitDuration()
    {
        yield return new WaitForSeconds(duration);
        AfterDuration();
    }
    
    // Overridable Methods
    protected virtual void RunCastAction()
    {
        // Insert action here for any following spell
    }

    protected virtual void AfterDuration()
    {
        // Insert death/fizzle out/etc. effect after duration ends
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Enter Code Here
        }
    }
    
    
    
}
