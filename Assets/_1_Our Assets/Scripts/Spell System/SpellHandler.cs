using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class SpellHandler : MonoBehaviour
{
    [SerializeField] private GameObject attachPoint;
    [SerializeField] private GrimoireHandler grimoire;
    [SerializeField] private InputActionReference[] castActionList;
    [SerializeField] private InputActionReference[] exemptedActionList;

    [SerializeField] private InputActionReference positiveNavigationAction;
    [SerializeField] private InputActionReference negativeNavigationAction;

    private GameManager _gameManager;
    private GameObject _currentSpellPrefab;
    private GameObject[] _spellPrefabList;
    private int _exemptedButtonsPressed;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _spellPrefabList = _gameManager.GetSpellPrefabList();
    }

    void Awake()
    {


        _currentSpellPrefab = _spellPrefabList[0];
        grimoire.UpdateGrimoirePages(_currentSpellPrefab.GetComponent<Spell>().GetName(), 1);
    }


    // Input References
    void OnEnable()
    {
        foreach (var thisCastAction in castActionList)
        {
            thisCastAction.action.performed += OnCastAction;
        }

        foreach (var thisExemptedAction in exemptedActionList)
        {
            thisExemptedAction.action.performed += AddExemptions;
            thisExemptedAction.action.canceled += SubtractExemptions;
        }
    }

    void OnDisable()
    {
        
    }

    void OnCastAction(InputAction.CallbackContext obj)
    {
        if (_exemptedButtonsPressed < 1)
        {
            Instantiate(_currentSpellPrefab, attachPoint.transform.position, attachPoint.transform.rotation);
        }
    }

    void AddExemptions(InputAction.CallbackContext obj) { _exemptedButtonsPressed += 1;}
    void SubtractExemptions(InputAction.CallbackContext obj) { _exemptedButtonsPressed -= 1;}
    
    // Get/Set Functions
    public void SetCurrentSpell(GameObject spell) { _currentSpellPrefab = spell; }
    public GameObject GetCurrentSpell() { return _currentSpellPrefab; }
}
