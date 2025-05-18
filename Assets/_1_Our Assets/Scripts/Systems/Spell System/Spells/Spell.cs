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
    [SerializeField] private int manaCost;
    [SerializeField] private float manaRechargeDelay;
    [SerializeField] private bool isContinuous;
    [SerializeField] private bool hasDuration;
    [SerializeField] private float duration;
    

    private void Awake()
    {
        RunCastAction();
        if (hasDuration && !isContinuous)
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
        if (collision.gameObject.CompareTag("Hazard"))
        {
            // Enter Code Here
        }
    }

    public string GetName() => spellName;
    public string GetDescription() => spellDescription;
    public int GetManaCost() => manaCost;
    public float GetRechargeDelay() => manaRechargeDelay;
    public bool GetIsContinuous() => isContinuous;
    
    public int GetDamage()
    {
        return Random.Range(damageMin, damageMax);
    }

}
