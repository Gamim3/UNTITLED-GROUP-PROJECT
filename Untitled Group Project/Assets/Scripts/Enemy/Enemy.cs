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

    [Header("TempMelee")]
    private Transform startLeftClaw;
    private Transform startRightClaw;
    private GameObject leftClaw;
    private GameObject rightClaw;
    private Transform leftClawDest;
    private Transform rightClawDest;

    [NonSerialized] public bool detectedPlayer;

    [Header("AttackTimes")]
    [SerializeField] float engageTimer;
    [SerializeField] float disengageTimer;
    [SerializeField] float energyRegainTimer;
    [SerializeField] float throwingSpikeTimer;
    [SerializeField] float leftClawAttackTimer;
    [SerializeField] float rightClawAttackTimer;

    [Header("AttackStartTimes")]
    [NonSerialized] public float startEngageTimer;
    [NonSerialized] public float startDisengageTimer;
    [NonSerialized] public float startRegainEnergyTimer;
    [NonSerialized] public float startThrowingSpikeTimer;
    [NonSerialized] public float startLeftClawAttackTimer;
    [NonSerialized] public float startRightClawAttackTimer;

    [Header("AttackCheck")]
    [NonSerialized] public bool engaging;
    [NonSerialized] public bool disengaging;
    [NonSerialized] public bool regainingEnergy;
    [NonSerialized] public bool throwingSpike;
    [NonSerialized] public bool leftClawAttack;
    [NonSerialized] public bool rightClawAttack;

    public override void Start()
    {
        leftClaw = GameObject.Find("TempLeftClaw");
        rightClaw = GameObject.Find("TempRightClaw");
        leftClawDest = GameObject.Find("TempLeftClawDest").transform;
        rightClawDest = GameObject.Find("TempRightClawDest").transform;
        startLeftClaw = GameObject.Find("TempLeftClawStart").transform;
        startRightClaw = GameObject.Find("TempRightClawStart").transform;

        base.Start();

        startEngageTimer = engageTimer;
        startDisengageTimer = disengageTimer;
        startRegainEnergyTimer = energyRegainTimer;
        startThrowingSpikeTimer = throwingSpikeTimer;
        startLeftClawAttackTimer = leftClawAttackTimer;
        startRightClawAttackTimer = rightClawAttackTimer;
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
            case bool attacking when attacking == true && disengaging == false && regainingEnergy == false && throwingSpike == false && leftClawAttack == false && rightClawAttack == false:
                engageTimer -= Time.deltaTime;
                break;

            case bool attacking when attacking == false && disengaging == true && regainingEnergy == false && throwingSpike == false && leftClawAttack == false && rightClawAttack == false:
                disengageTimer -= Time.deltaTime;
                break;

            case bool attacking when attacking == false && regainingEnergy == true && disengaging == false && throwingSpike == false && leftClawAttack == false && rightClawAttack == false:
                energyRegainTimer -= Time.deltaTime;
                break;

            case bool attacking when attacking == false && regainingEnergy == false && disengaging == false && throwingSpike == true && leftClawAttack == false && rightClawAttack == false:
                throwingSpikeTimer -= Time.deltaTime;
                break;

            case bool attacking when attacking == false && regainingEnergy == false && disengaging == false && throwingSpike == false && leftClawAttack == true && rightClawAttack == false:
                leftClawAttackTimer -= Time.deltaTime;
                break;

            case bool attacking when attacking == false && regainingEnergy == false && disengaging == false && throwingSpike == false && leftClawAttack == false && rightClawAttack == true:
                rightClawAttackTimer -= Time.deltaTime;
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
            if (distance > 3.5f)
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
            Exhaustion(exhaustionSpeed * 1000);

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

    public void LeftClawAttack()
    {
        if (leftClawAttackTimer >= 0 && leftClawAttack)
        {
            Vector3 relativePos = playerPosition.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = new Quaternion(transform.rotation.x, rotation.y, transform.rotation.z, transform.rotation.w);
            leftClaw.GetComponent<Collider>().enabled = true;
            leftClaw.transform.position = Vector3.MoveTowards(leftClaw.transform.position, leftClawDest.position, moveSpeed / 50);
        }
        else
        {
            Exhaustion(exhaustionSpeed * 1000);
            leftClaw.transform.position = Vector3.MoveTowards(leftClaw.transform.position, startLeftClaw.position, moveSpeed / 50);

            leftClawAttack = false;
            leftClawAttackTimer = startLeftClawAttackTimer;

            if (brain.attackQueue.Peek() == Attacks.LeftClaw)
            {
                leftClaw.GetComponent<Collider>().enabled = false;
                brain.attackQueue.Dequeue();
            }
        }
    }

    public void RightClawAttack()
    {
        if (rightClawAttackTimer > 0 && rightClawAttack)
        {
            Vector3 relativePos = playerPosition.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = new Quaternion(transform.rotation.x, rotation.y, transform.rotation.z, transform.rotation.w);
            rightClaw.GetComponent<Collider>().enabled = true;
            rightClaw.transform.position = Vector3.MoveTowards(rightClaw.transform.position, rightClawDest.position, moveSpeed / 50);
        }
        else
        {
            Exhaustion(exhaustionSpeed * 1000);
            rightClaw.transform.position = Vector3.MoveTowards(rightClaw.transform.position, startRightClaw.position, moveSpeed / 50);

            rightClawAttack = false;
            rightClawAttackTimer = startRightClawAttackTimer;

            if (brain.attackQueue.Peek() == Attacks.RightClaw)
            {
                rightClaw.GetComponent<Collider>().enabled = false;
                brain.attackQueue.Dequeue();
            }
        }
    }

}
