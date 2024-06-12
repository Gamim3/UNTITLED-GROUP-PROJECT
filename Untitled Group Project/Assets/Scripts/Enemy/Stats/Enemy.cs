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
    #region Variables

    [SerializeField] float _attackSpeed;

    [SerializeField] EnemyType _enemyType;
    public List<ItemInfo> DroppableItems = new();

    [SerializeField] float _exhaustedTime;
    [SerializeField] float _exhaustionSpeed;
    [SerializeField] float _meleeRange;
    [SerializeField] float _projectileSpeed;
    [SerializeField] float _projectileDamage;

    [Header("AnimationSettings")]
    [SerializeField] float neckMoveAmount;

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
    [Range(0, 360)]
    public float throwAngle;

    [Header("Dependancy")]
    [SerializeField] EnemyBrain _brain;
    [SerializeField] QuestManager _questManager;
    [SerializeField] GameObject _fuzzyLogicVisuals;
    [SerializeField] GameObject otherCanvas;
    [SerializeField] InventoryManager invManager;

    //Private variables

    private float startSpeed;
    private bool hasNotEnteredCombat;

    private Animator animator;

    private GameObject thrownProjectile;

    //checkst to see if that bodypart can deal damage
    [NonSerialized] public bool isLeftClawAtacking;
    [NonSerialized] public bool isRightClawAttacking;
    [NonSerialized] public bool isHeadAtacking;

    #endregion

    #region MonoBehaviours

    public override void Start()
    {
        hasNotEnteredCombat = true;

        StartCoroutine(FieldOfViewRoutine());

        if (GameObject.Find("QuestManager") != null)
        {
            _questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        }

        animator = GetComponent<Animator>();

        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _moveSpeed;
        startSpeed = _moveSpeed;

        base.Start();
    }

    public override void Update()
    {
        if (playerInSight && hasNotEnteredCombat && _brain.attackQueue.Count != 0)
        {
            hasNotEnteredCombat = false;
            ExecuteAttack();
        }

        //CheckIfPlayerIsLeftOrRight();

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

            if (DataPersistenceManager.instance != null)
            {
                DataPersistenceManager.instance.SaveManualGame();
            }

            SceneManager.LoadScene("GuildHall");

            //GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>().OnSceneLoaded(SceneManager.GetSceneByName("GuildHall"), LoadSceneMode.Single);
        }

        _distance = Vector3.Distance(transform.position, player.transform.position);

        _logic.enemyHealth = _healthPoints / _maxHealth * 100;
        _logic.energy = _energy / _maxEnergy * 100;
        _logic.distance = _distance;
    }

    #endregion

    #region AnimationVoids

    //these voids get called with animation events
    public void IsAttacking(string bodypart)
    {
        if (bodypart == "LeftClaw")
        {
            isLeftClawAtacking = true;
        }
        else if (bodypart == "RightClaw")
        {
            isRightClawAttacking = true;
        }
        else if (bodypart == "Head")
        {
            isHeadAtacking = true;
        }
    }

    public void StoppedAttacking(string bodypart)
    {
        if (bodypart == "LeftClaw")
        {
            isLeftClawAtacking = false;
        }
        else if (bodypart == "RightClaw")
        {
            isRightClawAttacking = false;
        }
        else if (bodypart == "Head")
        {
            isHeadAtacking = false;
        }
    }

    public void QueueAttack()
    {
        if (animator.GetInteger("WalkDir") == 0)
        {
            _brain.MakeDesicion();
        }
        else if (animator.GetInteger("WalkDir") == 1)
        {
            Engage();
        }
        else if (animator.GetInteger("WalkDir") == -1)
        {
            Disengage();
        }
    }

    public void ExecuteAttack()
    {
        if (animator.GetInteger("WalkDir") == 0 && playerInSight && _brain.attackQueue.Count != 0)
        {
            _agent.ResetPath();

            switch (_brain.attackQueue.Peek())
            {
                case Attacks attack when attack == Attacks.Engage:
                    Engage();
                    break;
                case Attacks attack when attack == Attacks.DisengageDash:
                    Disengage();
                    break;
                case Attacks attack when attack == Attacks.RegainEnergy:
                    StartCoroutine(RegainEnergy());
                    break;
                case Attacks attack when attack == Attacks.SpikeThrow:
                    SpikeThrow();
                    break;
                case Attacks attack when attack == Attacks.LeftClaw:
                    LeftClawAttack();
                    break;
                case Attacks attack when attack == Attacks.RightClaw:
                    RightClawAttack();
                    break;
                case Attacks attack when attack == Attacks.Bite:
                    BiteAttack();
                    break;
                case Attacks attack when attack == Attacks.Charge:
                    ChargeAttack();
                    break;
                case Attacks attack when attack == Attacks.SpikeSlamAttack:
                    _brain.attackQueue.Clear();
                    _brain.MakeDesicion();
                    if (_brain.attackQueue.Peek() != Attacks.SpikeSlamAttack)
                    {
                        ExecuteAttack();
                    }
                    else
                    {
                        _brain.attackQueue.Clear();
                        _brain.attackQueue.Enqueue(Attacks.LeftClaw);
                        ExecuteAttack();
                    }
                    break;

            }
        }
        else if (animator.GetInteger("WalkDir") == 1 && animator.GetInteger("WalkDir") != -1)
        {
            Engage();
        }
        else if (animator.GetInteger("WalkDir") == -1 && animator.GetInteger("WalkDir") != 0)
        {
            Disengage();
        }
    }

    public void SpawnSpike()
    {
        thrownProjectile = Instantiate(_projectile, _projectileSpawnPosition.position, _projectileSpawnPosition.rotation, _parent);
    }

    public void MoveSpike()
    {
        thrownProjectile.GetComponent<EnemyProjectile>().AimAtPlayer();

        thrownProjectile.transform.SetParent(null);

        thrownProjectile.GetComponent<EnemyProjectile>().projectileSpeed = _projectileSpeed;
        thrownProjectile.GetComponent<EnemyProjectile>().projectileDamage = _projectileDamage;
    }

    #endregion

    #region AttackVoids

    //Voids that correspond to every attack the enemy does
    //These voids get called in ExecuteAttack() inside the AnimationVoids Region

    public void Engage()
    {
        if (_distance > 5f)
        {
            animator.SetInteger("WalkDir", 1);

            Vector3 directionToPlayer = transform.position - player.transform.position;
            Vector3 meleeDistance = player.transform.position + directionToPlayer.normalized * _meleeRange;

            _agent.destination = meleeDistance;
            Exhaustion(_exhaustionSpeed / 2);
        }
        else
        {
            animator.SetInteger("WalkDir", 0);
            _brain.attackQueue.Dequeue();
            _brain.attackQueue.Clear();

            _brain.MakeDesicion();

            _brain.attacksCurrentlyInQueue = _brain.attackQueue.ToArray();
        }
    }

    public void Disengage()
    {
        if (_distance < 20)
        {
            animator.SetInteger("WalkDir", -1);

            Vector3 directionToPlayer = transform.position - player.transform.position;

            Vector3 newPosition = transform.position + directionToPlayer;

            _agent.SetDestination(newPosition);

            Exhaustion(_exhaustionSpeed / 4);

            if (_brain.attackQueue.Count != 0)
            {
                _brain.attackQueue.Dequeue();
            }
        }
        else
        {
            animator.SetInteger("WalkDir", 0);
            if (_brain.attackQueue.Count != 0)
            {
                _brain.attackQueue.Dequeue();
                _brain.attackQueue.Clear();
            }
            _brain.MakeDesicion();

            _brain.attacksCurrentlyInQueue = _brain.attackQueue.ToArray();
        }
    }

    public IEnumerator RegainEnergy()
    {
        animator.SetBool("Exhousted", true);
        _energy = _maxEnergy;

        yield return new WaitForSeconds(_exhaustedTime);

        animator.SetBool("Exhousted", false);


        if (_brain.attackQueue.Count != 0)
        {
            _brain.attackQueue.Dequeue();
        }

        ExecuteAttack();
    }

    public void SpikeThrow()
    {
        animator.SetTrigger("SpikeThrow");

        Exhaustion(_exhaustionSpeed * 750);

        if (_brain.attackQueue.Peek() == Attacks.SpikeThrow)
        {
            _brain.attackQueue.Dequeue();
        }
    }

    public void LeftClawAttack()
    {
        animator.SetTrigger("LeftClawAttack");

        Exhaustion(_exhaustionSpeed * 1000);

        if (_brain.attackQueue.Peek() == Attacks.LeftClaw)
        {
            _brain.attackQueue.Dequeue();
        }
    }

    public void RightClawAttack()
    {
        animator.SetTrigger("RightClawAttack");

        Exhaustion(_exhaustionSpeed * 1000);

        if (_brain.attackQueue.Peek() == Attacks.RightClaw)
        {
            _brain.attackQueue.Dequeue();
        }
    }

    public void BiteAttack()
    {
        animator.SetTrigger("BiteAttack");

        Exhaustion(_exhaustionSpeed * 1000);

        if (_brain.attackQueue.Peek() == Attacks.Bite)
        {
            _brain.attackQueue.Dequeue();
        }
    }

    public void ChargeAttack()
    {
        animator.SetTrigger("DashAttack");

        Exhaustion(_exhaustionSpeed * 1000);

        if (_brain.attackQueue.Peek() == Attacks.RightClaw)
        {
            _brain.attackQueue.Dequeue();
        }
    }

    #endregion

    #region Checks

    //Checks if player is at the left or right so it can move its head that way with a animator blend tree
    public void CheckIfPlayerIsLeftOrRight()
    {
        float animatorParameter = animator.GetFloat("PlayerLoc");

        if (animatorParameter <= 1 && animatorParameter >= 0 && playerInSight)
        {
            Vector3 relativePoint = transform.InverseTransformPoint(player.transform.position);
            relativePoint = transform.InverseTransformPoint(player.transform.position);
            if (relativePoint.x < 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y))
            {
                animator.SetFloat("PlayerLoc", animatorParameter + neckMoveAmount);
            }
            if (relativePoint.x > 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y))
            {
                animator.SetFloat("PlayerLoc", animatorParameter - neckMoveAmount);
            }
        }
        else if (animatorParameter > 1)
        {
            animator.SetFloat("PlayerLoc", animatorParameter - neckMoveAmount);
        }
        else if (animatorParameter < 0)
        {
            animator.SetFloat("PlayerLoc", animatorParameter + neckMoveAmount);
        }
    }


    //Ienumerator that loops an calls FieldOfViewCheck() every loop
    private IEnumerator FieldOfViewRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    //Checks if the player is inside a triangular point infront of the enemy using a layermask for onstructions and one for the targets collider
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

    #endregion

    #region Enums
    public enum EnemyType
    {
        ANY,
        SPIKEBEAR
    }

    #endregion
}
