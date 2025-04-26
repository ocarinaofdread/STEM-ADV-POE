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

    [SerializeField] private InputActionReference navigationEnableAction;
    [SerializeField] private InputActionReference positiveNavigationAction;
    [SerializeField] private InputActionReference negativeNavigationAction;

    private GameManager _gameManager;
    private GameObject[] _spellPrefabList;
    private GameObject _currentSpellPrefab;
    private int _currentSpellPrefabIndex;
    private int _exemptedButtonsPressed;
    private bool _canSwitchSpells;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _spellPrefabList = _gameManager.GetSpellPrefabList();
        _currentSpellPrefab = _spellPrefabList[_currentSpellPrefabIndex];
    }


    // Input References
    void OnEnable()
    {
        foreach (var thisCastAction in castActionList) {
            thisCastAction.action.performed += OnCastAction; }

        foreach (var thisExemptedAction in exemptedActionList) {
            thisExemptedAction.action.performed += AddExemptions;
            thisExemptedAction.action.canceled += SubtractExemptions; }

        // As long as navigationEnableAction is selected, spells can be cast
        navigationEnableAction.action.performed += EnableGrimoireNavigation;
        navigationEnableAction.action.canceled += DisableGrimoireNavigation;
        
        positiveNavigationAction.action.performed += sender => OnNavigationAction(sender, 1);
        negativeNavigationAction.action.performed += sender => OnNavigationAction(sender, -1);
    }

    

    void OnCastAction(InputAction.CallbackContext obj)
    {
        if (_exemptedButtonsPressed < 1)
        {
            Instantiate(_currentSpellPrefab, attachPoint.transform.position, attachPoint.transform.rotation);
        }
    }

    void OnNavigationAction(InputAction.CallbackContext obj, int incrementValue)
    {
        if (_canSwitchSpells)
        {
            _currentSpellPrefabIndex += incrementValue;
            if (_currentSpellPrefabIndex >= _spellPrefabList.Length) {
                _currentSpellPrefabIndex = 0; }
            else if (_currentSpellPrefabIndex < 0) {
                _currentSpellPrefabIndex = _spellPrefabList.Length - 1; }

            _currentSpellPrefab = _spellPrefabList[_currentSpellPrefabIndex];
            grimoire.UpdateGrimoirePages(_currentSpellPrefab.GetComponent<Spell>().GetName(), 1);
        }
    }
    
    void EnableGrimoireNavigation(InputAction.CallbackContext obj) { _canSwitchSpells = true; }
    void DisableGrimoireNavigation(InputAction.CallbackContext obj) { _canSwitchSpells = false; }

    void AddExemptions(InputAction.CallbackContext obj) { _exemptedButtonsPressed += 1;}
    void SubtractExemptions(InputAction.CallbackContext obj) { _exemptedButtonsPressed -= 1;}
    
    // Get/Set Functions
    public void SetCurrentSpell(GameObject spell) { _currentSpellPrefab = spell; }
    public GameObject GetCurrentSpell() { return _currentSpellPrefab; }
}
