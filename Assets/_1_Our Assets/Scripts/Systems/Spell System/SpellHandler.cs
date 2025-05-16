using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class SpellHandler : MonoBehaviour
{
    [SerializeField] private GameObject attachPoint;
    [SerializeField] private GrimoireHandler grimoire;
    [SerializeField] private InputActionReference[] castActionList;
    [SerializeField] private InputActionReference[] exemptedActionList;

    [SerializeField] private InputActionReference navigationEnableAction;
    [SerializeField] private InputActionReference navigationInputAction;
    [SerializeField] private ActionBasedContinuousMoveProvider continuousProvider;

    private GameManager _gameManager;
    private Player _player;
    private GameObject[] _spellPrefabList;
    private GameObject _currentSpellPrefab;
    private int _currentSpellPrefabIndex;
    private int _exemptedButtonsPressed;
    private bool _canSwitchSpells;
    private bool _isHoldingGrimoire;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _player = FindObjectOfType<Player>();
        _spellPrefabList = _gameManager.GetSpellPrefabList();
        _currentSpellPrefab = _spellPrefabList[_currentSpellPrefabIndex];
    }


    // Input References
    void OnEnable()
    {
        foreach (var thisCastAction in castActionList)
        {
            thisCastAction.action.performed += OnCastAction;
            thisCastAction.action.canceled += OnCastEnd;
        }

        foreach (var thisExemptedAction in exemptedActionList)
        {
            thisExemptedAction.action.performed += AddExemptions;
            thisExemptedAction.action.canceled += SubtractExemptions;
        }

        // As long as navigationEnableAction is selected, spells can be cast
        navigationEnableAction.action.performed += EnableGrimoireNavigation;
        navigationEnableAction.action.canceled += DisableGrimoireNavigation;

        navigationInputAction.action.performed += OnNavigationAction;
    }



    void OnCastAction(InputAction.CallbackContext obj)
    {
        
        if (_exemptedButtonsPressed < 1 && _isHoldingGrimoire && HasEnoughMana())
        {
            Instantiate(_currentSpellPrefab, attachPoint.transform.position, attachPoint.transform.rotation);
            
        }
    }

    void OnCastEnd(InputAction.CallbackContext obj)
    {
        if (_exemptedButtonsPressed < 1 && _isHoldingGrimoire)
        {
            Spell currentSpell = _currentSpellPrefab.GetComponent<Spell>();
            if (currentSpell.GetIsContinuous())
            {
                currentSpell.End();
            }
        }
    }

    void OnNavigationAction(InputAction.CallbackContext obj)
    {
        if (!_canSwitchSpells || !_isHoldingGrimoire)
        {
            return;
        }

        var incrementValue = (int)Math.Round(obj.ReadValue<Vector2>().x);

        _currentSpellPrefabIndex += incrementValue;
        if (_currentSpellPrefabIndex >= _spellPrefabList.Length)
        {
            _currentSpellPrefabIndex = 0;
        }
        else if (_currentSpellPrefabIndex < 0)
        {
            _currentSpellPrefabIndex = _spellPrefabList.Length - 1;
        }

        _currentSpellPrefab = _spellPrefabList[_currentSpellPrefabIndex];
        grimoire.UpdateGrimoirePages(_currentSpellPrefab.GetComponent<Spell>().GetName(), 1);
    }

    private bool HasEnoughMana()
    {
        var manaCost = _currentSpellPrefab.GetComponent<Spell>().GetManaCost();
        var currentMana = _player.GetMana();

        return currentMana - manaCost >= 0;
    }

    void EnableGrimoireNavigation(InputAction.CallbackContext obj)
    {
        _canSwitchSpells = true;
        continuousProvider.enabled = false;
    }

    void DisableGrimoireNavigation(InputAction.CallbackContext obj)
    {
        _canSwitchSpells = false;
        continuousProvider.enabled = true;
    }

    void AddExemptions(InputAction.CallbackContext obj)
    {
        _exemptedButtonsPressed += 1;
    }

    void SubtractExemptions(InputAction.CallbackContext obj)
    {
        _exemptedButtonsPressed -= 1;
    }

    public void StartHoldingGrimoire() { _isHoldingGrimoire = true; }
    public void StopHoldingGrimoire() { _isHoldingGrimoire = false; }

    // Get/Set Functions
    //public void SetCurrentSpell(GameObject spell) { _currentSpellPrefab = spell; }
    //public GameObject GetCurrentSpell() { return _currentSpellPrefab; }
}
