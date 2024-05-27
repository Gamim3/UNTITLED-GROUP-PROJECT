using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] protected float _healthPoints;
    [SerializeField] protected float _energy;
    [SerializeField] protected float _moveSpeed;

    protected float _maxHealth;
    protected float _maxEnergy;

    public virtual void Start()
    {
        _maxHealth = _healthPoints;
        _maxEnergy = _energy;
    }

    public virtual void Update()
    {
        if (_healthPoints <= 0)
        {
            Destroy(gameObject);
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
