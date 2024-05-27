using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected float healthPoints;
    [SerializeField] protected float energy;
    [SerializeField] protected float moveSpeed;

    protected float maxHealth;
    protected float maxEnergy;

    public virtual void Start()
    {
        maxHealth = healthPoints;
        maxEnergy = energy;
    }

    public virtual void Update()
    {
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Exhaustion(float Energy)
    {
        energy -= (Energy / 50);
    }

    public void TakeDamage(float damage)
    {
        healthPoints -= damage;
    }
}
