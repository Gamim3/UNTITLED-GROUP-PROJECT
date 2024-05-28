using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static EnemyBrain;

public class Enemy : Entity
{
    [SerializeField] float attackSpeed;

    [SerializeField] EnemyType enemyType;

    [SerializeField] float exhaustionSpeed;
    [SerializeField] float meleeRange;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileDamage;

    [Header("Distanc/Detection")]
    [SerializeField] float distance;

    [Range(0, 100)]
    public float radius;
    [Range(0, 360)]
    public float angle;
    [SerializeField] float delay = 0.2f;

    public GameObject player;

    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;

    [NonSerialized] public bool playerInSight;

    [Header("Movement")]
    private NavMeshAgent agent;

    [Header("RangedAttackRequirements")]
    [SerializeField] Transform parent;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileSpawnPosition;

    [Header("Dependancy")]
    [SerializeField] EnemyBrain brain;
    [SerializeField] FuzzyLogic logic;
    [SerializeField] QuestManager questManager;

    private float startSpeed;

    //alles hieronder is temporary en word verwijderd wanneer animations er in zitten
    [Header("TempMelee")]
    private Transform startLeftClaw;
    private Transform startRightClaw;
    private GameObject leftClaw;
    private GameObject rightClaw;
    private Transform leftClawDest;
    private Transform rightClawDest;

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
        StartCoroutine(FieldOfViewRoutine());

        if (GameObject.Find("QuestManager") != null)
        {
            questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        }

        agent = GetComponent<NavMeshAgent>();
        agent.speed = _moveSpeed;
        startSpeed = _moveSpeed;

        //temporary word verwijderd wanneer animations er in zitten
        leftClaw = GameObject.Find("TempLeftClaw");
        rightClaw = GameObject.Find("TempRightClaw");
        leftClawDest = GameObject.Find("TempLeftClawDest").transform;
        rightClawDest = GameObject.Find("TempRightClawDest").transform;
        startLeftClaw = GameObject.Find("TempLeftClawStart").transform;
        startRightClaw = GameObject.Find("TempRightClawStart").transform;

        base.Start();

        //temporary word verwijderd wanneer animations er in zitten
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

        if (_healthPoints <= 0)
        {
            if (questManager != null)
            {
                if (questManager.activeQuest.enemyToHunt == enemyType)
                {
                    questManager.TypeCheck();
                }
            }

            Destroy(gameObject);
        }

        distance = Vector3.Distance(transform.position, player.transform.position);

        logic.enemyHealth = _healthPoints / _maxHealth * 100;
        logic.energy = _energy / _maxEnergy * 100;
        logic.distance = distance;

        if (!playerInSight)
        {
            brain.attackQueue.Clear();
        }

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

    //Attacks

    public void Engage()
    {
        if (engageTimer >= 0 && engaging)
        {
            if (distance > 3.5f)
            {
                Vector3 directionToPlayer = transform.position - player.transform.position;
                Vector3 meleeDistance = player.transform.position + directionToPlayer.normalized * meleeRange;

                agent.destination = meleeDistance;
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
            Vector3 directionToPlayer = transform.position - player.transform.position;

            Vector3 newPosition = transform.position + directionToPlayer;

            agent.SetDestination(newPosition);

            Exhaustion(exhaustionSpeed / 4);
        }
        else
        {
            disengaging = false;
            disengageTimer = startDisengageTimer;

            if (brain.attackQueue.Peek() == Attacks.DisengageDash)
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
            _energy = _maxEnergy;

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
            transform.LookAt(player.transform);

            Exhaustion(exhaustionSpeed * 750);


            //add visual line of sight
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
            leftClaw.GetComponent<Collider>().enabled = true;
            leftClaw.transform.position = Vector3.MoveTowards(leftClaw.transform.position, leftClawDest.position, attackSpeed);
        }
        else
        {
            Exhaustion(exhaustionSpeed * 1000);
            leftClaw.transform.position = Vector3.MoveTowards(leftClaw.transform.position, startLeftClaw.position, attackSpeed);

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
            rightClaw.GetComponent<Collider>().enabled = true;
            rightClaw.transform.position = Vector3.MoveTowards(rightClaw.transform.position, rightClawDest.position, attackSpeed);
        }
        else
        {
            Exhaustion(exhaustionSpeed * 1000);
            rightClaw.transform.position = Vector3.MoveTowards(rightClaw.transform.position, startRightClaw.position, attackSpeed);

            rightClawAttack = false;
            rightClawAttackTimer = startRightClawAttackTimer;

            if (brain.attackQueue.Peek() == Attacks.RightClaw)
            {
                rightClaw.GetComponent<Collider>().enabled = false;
                brain.attackQueue.Dequeue();
            }
        }
    }

    private IEnumerator FieldOfViewRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                if (!Physics.Raycast(transform.position, directionToTarget, distance, obstructionMask))
                {
                    playerInSight = true;
                }
                else
                {
                    playerInSight = false;
                }
            }
            else
            {
                playerInSight = false;
            }
        }
        else if (playerInSight)
        {
            playerInSight = false;
        }
    }

    public enum EnemyType
    {
        ANY,
        SPIKEBEAR
    }

}
