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
    private bool _currentlyRecharging;
    
    private int _maxMana;
    private float _rechargeTimer;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _maxMana = mana;

        _rechargeTimer = manaRechargeInterval;
        _currentlyRecharging = true;
    }

    private void Update()
    {
        if (_currentlyRecharging && mana < _maxMana)
        {
            _rechargeTimer -= Time.deltaTime;
            if (_rechargeTimer <= 0)
            {
                mana += manaRechargeIncrement;
                _rechargeTimer = manaRechargeInterval;
            }
        }
    }

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
    }
    public int GetMana() => mana;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hazard")) return;
        
        var otherHazard = other.GetComponent<Hazard>();
        health -= otherHazard.GetDamage();
    }
}
