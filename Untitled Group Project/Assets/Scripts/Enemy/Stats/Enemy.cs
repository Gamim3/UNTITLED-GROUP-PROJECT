using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static EnemyBrain;
using Random = UnityEngine.Random;

public class Enemy : Entity
{
    [SerializeField] float _attackSpeed;

    [SerializeField] EnemyType _enemyType;
    public List<ItemInfo> DroppableItems = new();

    [SerializeField] float _exhaustionSpeed;
    [SerializeField] float _meleeRange;
    [SerializeField] float _projectileSpeed;
    [SerializeField] float _projectileDamage;

    [Header("Distanc/Detection")]
    [SerializeField] float _distance;

    [Range(0, 100)]
    public float radius;
    [Range(0, 360)]
    public float angle;
    [SerializeField] float _delay = 0.2f;

    public GameObject player;

    [SerializeField] LayerMask _targetMask;
    [SerializeField] LayerMask _obstructionMask;

    [NonSerialized] public bool playerInSight;

    [Header("Movement")]
    private NavMeshAgent _agent;

    [Header("RangedAttackRequirements")]
    [SerializeField] Transform _parent;
    [SerializeField] GameObject _projectile;
    [SerializeField] Transform _projectileSpawnPosition;

    [Header("Dependancy")]
    [SerializeField] EnemyBrain _brain;
    [SerializeField] FuzzyLogic _logic;
    [SerializeField] QuestManager _questManager;
    [SerializeField] GameObject _fuzzyLogicVisuals;
    [SerializeField] GameObject otherCanvas;
    [SerializeField] InventoryManager invManager;

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
            _questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        }

        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _moveSpeed;
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            _fuzzyLogicVisuals.SetActive(!_fuzzyLogicVisuals.activeSelf);
            otherCanvas.SetActive(!otherCanvas.activeSelf);
        }

        base.Update();

        if (_healthPoints <= 0)
        {
            if (_questManager != null)
            {
                for (int i = 0; i < _questManager.activeQuests.Count; i++)
                {
                    if (_questManager.activeQuests[i].enemyToHunt == _enemyType || _questManager.activeQuests[i].enemyToHunt == EnemyType.ANY)
                    {
                        _questManager.TypeCheck(_questManager.activeQuests[i].questId);
                    }
                }
            }

            InventoryManager.Instance.AddItem(DroppableItems[0].item.itemID, Random.Range(1, 3));

            //GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>().SaveAutoGame();

            SceneManager.LoadScene("GuildHall");

            //GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>().OnSceneLoaded(SceneManager.GetSceneByName("GuildHall"), LoadSceneMode.Single);
        }

        _distance = Vector3.Distance(transform.position, player.transform.position);

        _logic.enemyHealth = _healthPoints / _maxHealth * 100;
        _logic.energy = _energy / _maxEnergy * 100;
        _logic.distance = _distance;

        if (!playerInSight)
        {
            _brain.attackQueue.Clear();
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
            if (_distance > 3.5f)
            {
                Vector3 directionToPlayer = transform.position - player.transform.position;
                Vector3 meleeDistance = player.transform.position + directionToPlayer.normalized * _meleeRange;

                _agent.destination = meleeDistance;
                Exhaustion(_exhaustionSpeed / 2);
            }
        }
        else
        {
            engaging = false;
            engageTimer = startEngageTimer;

            if (_brain.attackQueue.Peek() == Attacks.Engage)
            {
                _brain.attackQueue.Dequeue();
            }
        }
    }

    public void Disengage()
    {
        if (disengageTimer >= 0 && disengaging)
        {
            Vector3 directionToPlayer = transform.position - player.transform.position;

            Vector3 newPosition = transform.position + directionToPlayer;

            _agent.SetDestination(newPosition);

            Exhaustion(_exhaustionSpeed / 4);
        }
        else
        {
            disengaging = false;
            disengageTimer = startDisengageTimer;

            if (_brain.attackQueue.Peek() == Attacks.DisengageDash)
            {
                _brain.attackQueue.Dequeue();
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

            if (_brain.attackQueue.Peek() == Attacks.RegainEnergy)
            {
                _brain.attackQueue.Dequeue();
            }
        }
    }

    public void SpikeThrow()
    {
        if (throwingSpikeTimer >= 0 && throwingSpike)
        {
            transform.LookAt(player.transform);

            Exhaustion(_exhaustionSpeed * 750);


            //add visual line of sight
            GameObject thrownProjectile = Instantiate(_projectile, _projectileSpawnPosition.position, _projectileSpawnPosition.rotation, _parent);

            thrownProjectile.GetComponent<EnemyProjectile>().projectileSpeed = _projectileSpeed;
            thrownProjectile.GetComponent<EnemyProjectile>().projectileDamage = _projectileDamage;

            throwingSpike = false;
            throwingSpikeTimer = startThrowingSpikeTimer;

            if (_brain.attackQueue.Peek() == Attacks.SpikeThrow)
            {
                _brain.attackQueue.Dequeue();
            }
        }
    }

    public void LeftClawAttack()
    {
        transform.LookAt(player.transform);

        if (leftClawAttackTimer >= 0 && leftClawAttack)
        {
            leftClaw.GetComponent<Collider>().enabled = true;
            leftClaw.transform.position = Vector3.MoveTowards(leftClaw.transform.position, leftClawDest.position, _attackSpeed);
        }
        else
        {
            Exhaustion(_exhaustionSpeed * 1000);
            leftClaw.transform.position = Vector3.MoveTowards(leftClaw.transform.position, startLeftClaw.position, _attackSpeed);

            leftClawAttack = false;
            leftClawAttackTimer = startLeftClawAttackTimer;

            if (_brain.attackQueue.Peek() == Attacks.LeftClaw)
            {
                leftClaw.GetComponent<Collider>().enabled = false;
                _brain.attackQueue.Dequeue();
            }
        }
    }

    public void RightClawAttack()
    {
        transform.LookAt(player.transform);

        if (rightClawAttackTimer > 0 && rightClawAttack)
        {
            rightClaw.GetComponent<Collider>().enabled = true;
            rightClaw.transform.position = Vector3.MoveTowards(rightClaw.transform.position, rightClawDest.position, _attackSpeed);
        }
        else
        {
            Exhaustion(_exhaustionSpeed * 1000);
            rightClaw.transform.position = Vector3.MoveTowards(rightClaw.transform.position, startRightClaw.position, _attackSpeed);

            rightClawAttack = false;
            rightClawAttackTimer = startRightClawAttackTimer;

            if (_brain.attackQueue.Peek() == Attacks.RightClaw)
            {
                rightClaw.GetComponent<Collider>().enabled = false;
                _brain.attackQueue.Dequeue();
            }
        }
    }

    private IEnumerator FieldOfViewRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, _targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                if (!Physics.Raycast(transform.position, directionToTarget, _distance, _obstructionMask))
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
                //playerInSight = false;
            }
        }
        else if (playerInSight)
        {
            //playerInSight = false;
        }
    }

    public enum EnemyType
    {
        ANY,
        SPIKEBEAR
    }

}
