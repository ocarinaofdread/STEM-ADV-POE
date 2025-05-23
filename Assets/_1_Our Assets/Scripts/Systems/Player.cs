using System;
using System.Collections;
using Leguar.LowHealth;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int mana;
    [SerializeField] private float manaRechargeInterval = 0.1f;
    [SerializeField] private int manaRechargeIncrement = 1;
    // Why is this here? - [SerializeField] private GameObject leftSpellHandler;
    
    [SerializeField] private InputActionProperty rightHandContTurnAction;
    [SerializeField] private InputActionProperty rightHandSnapTurnAction;
    [SerializeField] private InputActionProperty leftHandContTurnAction;
    [SerializeField] private InputActionProperty leftHandSnapTurnAction;
    [SerializeField] private InputActionProperty rightHandMoveAction;
    [SerializeField] private InputActionProperty leftHandMoveAction;
    [SerializeField] private LowHealthController lowHealthController;

    private GameManager _gameManager;
    private DominantHand _currentDominantHand;
    private ActionBasedContinuousMoveProvider _continuousMoveProvider;
    private ActionBasedSnapTurnProvider _snapTurnProvider;
    private ActionBasedContinuousTurnProvider _continuousTurnProvider;
    
    private HealthSystemForDummies _leftManaSystem;
    private HealthSystemForDummies _leftHealthSystem;
    private HealthSystemForDummies _rightManaSystem;
    private HealthSystemForDummies _rightHealthSystem;
    private bool _currentlyRecharging;
    private int _rechargeRequests;

    private bool _isDead;
    private int _maxMana;
    private int _maxHealth;
    private float _rechargeTimer;
    private bool _foundSystems;
    private float _previousHealthPercentage;

    private readonly InputActionProperty nullProperty = new(null);

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _continuousMoveProvider = GetComponent<ActionBasedContinuousMoveProvider>();
        _snapTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
        _continuousTurnProvider = GetComponent<ActionBasedContinuousTurnProvider>();
        SetProviderActions(_gameManager.GetDominantHand());
        
        _maxMana = mana;
        _maxHealth = health;

        _rechargeTimer = manaRechargeInterval;
        _currentlyRecharging = true;
    }

    // Exclusively to find the mana systems because they cannot be found during
    // Start()
    private void FixedUpdate()
    {
        if (_foundSystems) { return; }

        try
        {
            _leftManaSystem ??= GameObject.FindGameObjectWithTag("LeftMana").GetComponent<HealthSystemForDummies>();
            _leftHealthSystem ??= GameObject.FindGameObjectWithTag("LeftHealth").GetComponent<HealthSystemForDummies>();
            _rightManaSystem ??= GameObject.FindGameObjectWithTag("RightMana").GetComponent<HealthSystemForDummies>();
            _rightHealthSystem ??= GameObject.FindGameObjectWithTag("RightHealth").GetComponent<HealthSystemForDummies>();
        }
        catch (NullReferenceException e)
        {
            return;
        }

        if (!_leftManaSystem) return;
        
        _foundSystems = true;
        SetManaHealths(_maxMana);
        SetHealths(health);
    }
    
    private void Update()
    {
        // Dies if health <= 0
        if (health <= 0 && !_isDead)
        {
            // _gameManager.something
            // teleport to game over screen baby
            // return;
        }

        if (_previousHealthPercentage != GetHealthPercentage())
        {
            _previousHealthPercentage = GetHealthPercentage();
            lowHealthController.SetPlayerHealthInstantly(_previousHealthPercentage);
        }
        
        // Changes dominant hand and subsequent provider attributes
        if (_currentDominantHand != _gameManager.GetDominantHand())
        {
            _currentDominantHand = _gameManager.GetDominantHand();
            SetProviderActions(_currentDominantHand);
        }

        // Checks if currently recharging or max capacity reached
        if (!_currentlyRecharging || mana >= _maxMana) return;
        
        // Regenerates mana
        _rechargeTimer -= Time.deltaTime;
        if (_rechargeTimer <= 0)
        {
            mana += manaRechargeIncrement;
            ChangeManaHealths(manaRechargeIncrement);
                
            _rechargeTimer = manaRechargeInterval;
        }
    }

    // Get, set mana
    public void IncrementMana(int increment)
    {
        mana += increment;
        if (mana < 0)
        {
            mana = 0;
        }
        else if (mana > _maxMana)
        {
            mana = _maxMana;
        }

        ChangeManaHealths(increment);
    }
    public int GetMana() => mana;

    // Delay following a spell cast until recharge begins
    public IEnumerator RechargeDelay(float delay, float intervalAfter)
    {
        _rechargeRequests++;
        _currentlyRecharging = false;
        manaRechargeInterval = intervalAfter;

        yield return new WaitForSeconds(delay);

        _rechargeRequests--;
        if (_rechargeRequests == 0)
        {
            _currentlyRecharging = true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hazard")) return;
        
        var otherHazard = other.GetComponent<Hazard>();
        health -= otherHazard.GetDamage();
        ChangeHealths(-otherHazard.GetDamage());
    }

    private void SetManaHealths(int newMana)
    {
        _rightManaSystem.CurrentHealth = newMana;
        _leftManaSystem.CurrentHealth = newMana;
        _rightManaSystem.MaximumHealth = newMana;
        _leftManaSystem.MaximumHealth = newMana;
    }

    private void SetHealths(int newHealth)
    {
        _rightHealthSystem.CurrentHealth = newHealth;
        _leftHealthSystem.CurrentHealth = newHealth;
        _rightHealthSystem.MaximumHealth = newHealth;
        _leftHealthSystem.MaximumHealth = newHealth;
    }

    private void ChangeManaHealths(int newIncrement)
    {
        _rightManaSystem.AddToCurrentHealth(newIncrement);
        _leftManaSystem.AddToCurrentHealth(newIncrement);
    }

    private void ChangeHealths(int increment)
    {
        _rightHealthSystem.AddToCurrentHealth(increment);
        _leftHealthSystem.AddToCurrentHealth(increment);
    }

    private float GetHealthPercentage()
    {
        return (float) health / _maxHealth;
    }

    private void SetProviderActions(DominantHand hand)
    {
        // Ex. Right Hand
        // LeftHand = Move, RightHand = Turn
        
        if (hand == DominantHand.RightHanded)
        {
            _continuousMoveProvider.leftHandMoveAction = leftHandMoveAction;
            _snapTurnProvider.rightHandSnapTurnAction = rightHandSnapTurnAction;
            _continuousTurnProvider.rightHandTurnAction = rightHandContTurnAction;

            _continuousMoveProvider.rightHandMoveAction = nullProperty;
            _snapTurnProvider.leftHandSnapTurnAction = nullProperty;
            _continuousTurnProvider.leftHandTurnAction = nullProperty;
        }
        else
        {
            _continuousMoveProvider.rightHandMoveAction = rightHandMoveAction;
            _snapTurnProvider.leftHandSnapTurnAction = leftHandSnapTurnAction;
            _continuousTurnProvider.leftHandTurnAction = leftHandContTurnAction;

            _continuousMoveProvider.leftHandMoveAction = nullProperty;
            _snapTurnProvider.rightHandSnapTurnAction = nullProperty;
            _continuousTurnProvider.rightHandTurnAction = nullProperty;
        }
    }
    
    public void ResetDeath() { _isDead = false; }
}
