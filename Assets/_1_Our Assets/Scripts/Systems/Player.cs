using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int mana;
    [SerializeField] private float manaRechargeInterval = 0.1f;
    [SerializeField] private int manaRechargeIncrement = 1;

    private GameManager _gameManager;
    private HealthSystemForDummies _leftManaSystem;
    private HealthSystemForDummies _leftHealthSystem;
    private HealthSystemForDummies _rightManaSystem;
    private HealthSystemForDummies _rightHealthSystem;
    private bool _currentlyRecharging;
    private int _rechargeRequests;

    private bool _isDead;
    private int _maxMana;
    private float _rechargeTimer;
    private bool _foundSystems;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        
        _maxMana = mana;

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
        if (health <= 0 && !_isDead)
        {
            // _gameManager.something
            // teleport to game over screen baby
            // return;
        }
        
        if (_currentlyRecharging && mana < _maxMana)
        {
            _rechargeTimer -= Time.deltaTime;
            if (_rechargeTimer <= 0)
            {
                mana += manaRechargeIncrement;
                ChangeManaHealths(manaRechargeIncrement);
                
                _rechargeTimer = manaRechargeInterval;
            }
        }
    }

    public void IncrementMana(int increment)
    {
        int ogMana = mana;
        
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

    public IEnumerator RechargeDelay(float delay)
    {
        _rechargeRequests++;
        _currentlyRecharging = false;

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
    
    public void ResetDeath() { _isDead = false; }
}
