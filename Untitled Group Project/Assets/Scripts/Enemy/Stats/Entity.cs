using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float _healthPoints;
    [SerializeField] protected float _energy;
    [SerializeField] protected float _moveSpeed;

    protected float _maxHealth;
    protected float _maxEnergy;

    [NonSerialized] FuzzyLogic _logic;

    public virtual void Start()
    {
        _maxHealth = _healthPoints;
        _maxEnergy = _energy;

        _logic = GameObject.FindWithTag("Enemy").GetComponent<FuzzyLogic>();
    }

    public virtual void Update()
    {
        if (GetComponent<CharStateMachine>() != null)
        {
            _logic.playerHealth = _healthPoints / _maxHealth * 100;
        }
    }

    public virtual void Exhaustion(float Energy)
    {
        _energy -= (Energy / 50);
    }

    public virtual void TakeDamage(float damage)
    {
        _healthPoints -= damage;
    }
}
