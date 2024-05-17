using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EnemyBrain;

public class Enemy : Entity
{
    [SerializeField] Transform playerPosition;
    [SerializeField] float distance;
    [SerializeField] float detectionRange;

    [Header ("Dependancy")]
    [SerializeField] EnemyBrain brain;
    [SerializeField] FuzzyLogic logic;

    [NonSerialized] public bool detectedPlayer;

    [Header("AttackTimes")]
    [SerializeField] float engageTimer;
    [SerializeField] float disengageTimer;

    [Header("AttackStartTimes")]
    [NonSerialized] public float startEngageTimer;
    [NonSerialized] public float startDisengageTimer;

    [Header("AttackCheck")]
    [NonSerialized] public bool engaging;
    [NonSerialized] public bool disengaging;

    public override void Start()
    {
        base.Start();

        startEngageTimer = engageTimer;
        startDisengageTimer = disengageTimer;
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
            case bool attacking when attacking == true:
                engageTimer -= Time.deltaTime;
                break;

            case bool attacking when attacking == false && disengaging == true:
                disengageTimer -= Time.deltaTime;
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
            transform.position = Vector3.MoveTowards(transform.position, playerPosition.position, 0.03f);
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
            transform.position = Vector3.MoveTowards(transform.position, playerPosition.position, -0.03f);
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
}
