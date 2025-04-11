using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class SpellHandler : MonoBehaviour
{
    public GameObject currentSpellPrefab;
    public List<GameObject> spellPrefabList;
    
    [SerializeField] private GameObject attachPoint;
    [SerializeField] private InputActionReference[] castActionList;
    [SerializeField] private InputActionReference[] exemptedActionList;

    private int exemptedButtonsPressed;

    void Start()
    {
        currentSpellPrefab = spellPrefabList[0];
    }
    
    
    // Input References
    void OnEnable()
    {
        for (int i = 0; i < castActionList.Length; i++)
        {
            castActionList[i].action.performed += OnCastAction;
        }

        for (int i = 0; i < exemptedActionList.Length; i++)
        {
            exemptedActionList[i].action.performed += AddExemptions;
            exemptedActionList[i].action.canceled += SubtractExemptions;
        }
    }

    void OnCastAction(InputAction.CallbackContext obj)
    {
        if (exemptedButtonsPressed < 1)
        {
            Instantiate(currentSpellPrefab, attachPoint.transform.position, attachPoint.transform.rotation);
        }
    }

    void AddExemptions(InputAction.CallbackContext obj) { exemptedButtonsPressed += 1;}
    void SubtractExemptions(InputAction.CallbackContext obj) { exemptedButtonsPressed -= 1;}
    
    // Get/Set Functions
    public void SetCurrentSpell(GameObject spell) { currentSpellPrefab = spell; }
    public GameObject GetCurrentSpell() { return currentSpellPrefab; }
}
