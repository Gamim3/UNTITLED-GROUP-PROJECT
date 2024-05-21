using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EnemyBrain;

public class Enemy : Entity
{
    [Header("Stats")]
    [SerializeField] float exhaustionSpeed;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileDamage;

    [Header("Distanc/Detection")]
    [SerializeField] Transform playerPosition;
    [SerializeField] float detectionRange;
    [SerializeField] float distance;

    [Header("RangedAttackRequirements")]
    [SerializeField] Transform parent;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileSpawnPosition;

    [Header ("Dependancy")]
    [SerializeField] EnemyBrain brain;
    [SerializeField] FuzzyLogic logic;

    [NonSerialized] public bool detectedPlayer;

    [Header("AttackTimes")]
    [SerializeField] float engageTimer;
    [SerializeField] float disengageTimer;
    [SerializeField] float energyRegainTimer;
    [SerializeField] float throwingSpikeTimer;

    [Header("AttackStartTimes")]
    [NonSerialized] public float startEngageTimer;
    [NonSerialized] public float startDisengageTimer;
    [NonSerialized] public float startRegainEnergyTimer;
    [NonSerialized] public float startThrowingSpikeTimer;

    [Header("AttackCheck")]
    [NonSerialized] public bool engaging;
    [NonSerialized] public bool disengaging;
    [NonSerialized] public bool regainingEnergy;
    [NonSerialized] public bool throwingSpike;

    public override void Start()
    {
        base.Start();

        startEngageTimer = engageTimer;
        startDisengageTimer = disengageTimer;
        startRegainEnergyTimer = energyRegainTimer;
        startThrowingSpikeTimer = throwingSpikeTimer;
    }

    public override void Update()
    {
        base.Update();

        distance = Vector3.Distance(transform.position, playerPosition.position);

        logic.enemyHealth = healthPoints / maxHealth * 100;
        logic.energy = energy / maxEnergy * 100;
        logic.distance = distance;

        DetectPlayer();

        switch (engaging)
        {
            case bool attacking when attacking == true && disengaging == false && regainingEnergy == false && throwingSpike == false:
                engageTimer -= Time.deltaTime;
                break;

            case bool attacking when attacking == false && disengaging == true && regainingEnergy == false && throwingSpike == false:
                disengageTimer -= Time.deltaTime;
                break;

            case bool attacking when attacking == false && regainingEnergy == true && disengaging == false && throwingSpike == false:
                energyRegainTimer -= Time.deltaTime;
                break;

            case bool attacking when attacking == false && regainingEnergy == false && disengaging == false && throwingSpike == true:
                throwingSpikeTimer -= Time.deltaTime;
                break;
        }
    }

    public void DetectPlayer()
    {
        if(distance <= detectionRange)
        {
            detectedPlayer = true;
        }
        else
        {
            detectedPlayer = false;
            brain.attackQueue.Clear();
        }
    }

    //Attacks

    public void Engage()
    {
        if(engageTimer >= 0 && engaging)
        {
            if (distance > 4)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerPosition.position, moveSpeed * Time.deltaTime / 10);
                Exhaustion(exhaustionSpeed / 2);
            }
        }
        else
        {
            engaging = false;
            engageTimer = startEngageTimer;

            if (brain.attackQueue.Peek() == Attacks.Engage)
            {
                brain.attackQueue.Dequeue();
            }
        }
    }

    public void Disengage()
    {
        if (disengageTimer >= 0 && disengaging)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPosition.position, -moveSpeed * Time.deltaTime / 10);
            Exhaustion(exhaustionSpeed / 2);
        }
        else
        {
            disengaging = false;
            disengageTimer = startDisengageTimer;
            
            if(brain.attackQueue.Peek() == Attacks.DisengageDash)
            {
                brain.attackQueue.Dequeue();
            }
        }
    }

    public void RegainEnergy()
    {
        if (energyRegainTimer >= 0 && regainingEnergy)
        {

        }
        else
        {
            energy = maxEnergy;

            regainingEnergy = false;
            energyRegainTimer = startRegainEnergyTimer;

            if (brain.attackQueue.Peek() == Attacks.RegainEnergy)
            {
                brain.attackQueue.Dequeue();
            }
        }
    }

    public void SpikeThrow()
    {
        if (throwingSpikeTimer >= 0 && throwingSpike)
        {
            Exhaustion(exhaustionSpeed * 750);

            GameObject thrownProjectile = Instantiate(projectile, projectileSpawnPosition.position, projectileSpawnPosition.rotation, parent);

            thrownProjectile.GetComponent<EnemyProjectile>().projectileSpeed = projectileSpeed;
            thrownProjectile.GetComponent<EnemyProjectile>().projectileDamage = projectileDamage;

            throwingSpike = false;
            throwingSpikeTimer = startThrowingSpikeTimer;

            if (brain.attackQueue.Peek() == Attacks.SpikeThrow)
            {
                brain.attackQueue.Dequeue();
            }
        }
    }

}
