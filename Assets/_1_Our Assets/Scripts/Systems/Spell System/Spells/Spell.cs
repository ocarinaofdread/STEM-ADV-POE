using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private string spellName;
    [TextArea(1, 3)] [SerializeField] private string spellDescription;
    [SerializeField] private int damageMin;
    [SerializeField] private int damageMax;
    [SerializeField] private float manaCost;
    [SerializeField] private bool hasDuration;
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
        End();
    }
    
    // Overridable Methods
    protected virtual void RunCastAction()
    {
        // Insert action here for any following spell
    }

    public virtual void End()
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

    public string GetName() => spellName;
    public string GetDescription() => spellDescription;
    
    public int GetDamage()
    {
        return Random.Range(damageMin, damageMax);
    }

}
