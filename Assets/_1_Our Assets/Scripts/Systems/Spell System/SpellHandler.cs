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

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _player = FindObjectOfType<Player>();
        
        _spellPrefabList = _gameManager.GetSpellPrefabList();
        _currentSpellPrefab = _spellPrefabList[_currentSpellPrefabIndex];
    }


    // Input References
    private void OnEnable()
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



    private void OnCastAction(InputAction.CallbackContext obj)
    {
        if (_exemptedButtonsPressed < 1 && _isHoldingGrimoire && HasEnoughMana())
        {
            Instantiate(_currentSpellPrefab, attachPoint.transform.position, attachPoint.transform.rotation);
            _player.IncrementMana(-CurrentSpell().GetManaCost());
            StartCoroutine(_player.RechargeDelay(CurrentSpell().GetRechargeDelay()));
        }
    }

    private void OnCastEnd(InputAction.CallbackContext obj)
    {
        if (_exemptedButtonsPressed < 1 && _isHoldingGrimoire)
        {
            var currentSpell = CurrentSpell();
            if (currentSpell.GetIsContinuous())
            {
                currentSpell.End();
            }
        }
    }

    private void OnNavigationAction(InputAction.CallbackContext obj)
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
        grimoire.UpdateGrimoirePages(CurrentSpell().GetName(), 1);
    }

    private bool HasEnoughMana()
    {
        var manaCost = CurrentSpell().GetManaCost();
        var currentMana = _player.GetMana();

        return currentMana - manaCost >= 0;
    }

    private void EnableGrimoireNavigation(InputAction.CallbackContext obj)
    {
        _canSwitchSpells = true;
        continuousProvider.enabled = false;
    }

    private void DisableGrimoireNavigation(InputAction.CallbackContext obj)
    {
        _canSwitchSpells = false;
        continuousProvider.enabled = true;
    }

    private void AddExemptions(InputAction.CallbackContext obj)
    {
        _exemptedButtonsPressed += 1;
    }

    private void SubtractExemptions(InputAction.CallbackContext obj)
    {
        _exemptedButtonsPressed -= 1;
    }

    public void StartHoldingGrimoire() { _isHoldingGrimoire = true; }
    public void StopHoldingGrimoire() { _isHoldingGrimoire = false; }
    
    private Spell CurrentSpell() { return _currentSpellPrefab.GetComponent<Spell>(); }

    // Get/Set Functions
    //public void SetCurrentSpell(GameObject spell) { _currentSpellPrefab = spell; }
    //public GameObject GetCurrentSpell() { return _currentSpellPrefab; }
}
