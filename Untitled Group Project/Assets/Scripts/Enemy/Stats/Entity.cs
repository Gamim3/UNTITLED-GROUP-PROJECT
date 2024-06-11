using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entity : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float _healthPoints;
    [SerializeField] protected float _energy;
    [SerializeField] protected float _moveSpeed;

    [SerializeField] Transform _damageSpawnLocation;
    [SerializeField] GameObject _damageCanvas;
    [NonSerialized] protected FuzzyLogic _logic;
    protected float _maxHealth;
    protected float _maxEnergy;

    public virtual void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            _logic = GameObject.FindWithTag("Enemy").GetComponent<FuzzyLogic>();
        }

        _maxHealth = _healthPoints;
        _maxEnergy = _energy;
    }

    public virtual void Update()
    {

    }

    public virtual void Exhaustion(float Energy)
    {
        _energy -= (Energy / 50);
    }

    public virtual void TakeDamage(float damage)
    {
        _healthPoints -= damage;

        GameObject canvas = Instantiate(_damageCanvas, _damageSpawnLocation);
        canvas.GetComponent<DamageCanvas>().DamageTxt.text = damage.ToString();
    }

    public virtual float GetHealth()
    {
        return _healthPoints;
    }

    public virtual float GetMaxHealth()
    {
        return _maxHealth;
    }
}
