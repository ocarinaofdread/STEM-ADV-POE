using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class SpellHandler : MonoBehaviour
{
    public GameObject currentSpellPrefab;
    public List<GameObject> spellPrefabList;
    
    [SerializeField] private GameObject attachPoint;
    [SerializeField] private InputActionReference castAction;

    void Start()
    {
        currentSpellPrefab = spellPrefabList[0];
    }
    
    
    // Input References
    void OnEnable()
    {
        castAction.action.performed += OnCastAction;
    }

    void OnCastAction(InputAction.CallbackContext obj)
    {
        Instantiate(currentSpellPrefab, attachPoint.transform);
    }
    
    // Get/Set Functions
    public void SetCurrentSpell(GameObject spell) { currentSpellPrefab = spell; }
    public GameObject GetCurrentSpell() { return currentSpellPrefab; }
}
