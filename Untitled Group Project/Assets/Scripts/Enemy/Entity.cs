using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float healthPoints;
    public float energy;
    public float moveSpeed;

    protected float maxHealth;
    protected float maxEnergy;

    public virtual void Start()
    {
        maxHealth = healthPoints;
        maxEnergy = energy;
    }

    public virtual void Update()
    {
        if(healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Exhaustion(float Energy)
    {
        energy -= Energy;
    }

    public void TakeDamage(float damage)
    {
        healthPoints -= damage;
    }
}