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
    [SerializeField] private int damageAdditiveLevel;
    [SerializeField] private int manaCost;
    [SerializeField] private float manaRechargeDelay;
    [SerializeField] private float manaRechargeIntervalAfter = 0.1f;
    [SerializeField] private bool isContinuous;
    [SerializeField] private float continuousDecreaseInterval = 0.5f;
    [SerializeField] private bool hasDuration;
    [SerializeField] private float duration;
    [SerializeField] private bool spawnWithParent;
    

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            // Enter Code Here
        }
    }

    public string GetName() => spellName;
    public string GetDescription() => spellDescription;
    public int GetDamageAdditiveLevel() => damageAdditiveLevel;
    public int GetManaCost() => manaCost;
    public float GetRechargeDelay() => manaRechargeDelay;
    public float GetRechargeIntervalAfter() => manaRechargeIntervalAfter;
    public bool GetIsContinuous() => isContinuous;
    public float GetContinuousDecreaseInterval() => continuousDecreaseInterval;
    public bool GetSpawnWithParent() => spawnWithParent;
    
    public int GetDamage()
    {
        return Random.Range(damageMin, damageMax);
    }

}
