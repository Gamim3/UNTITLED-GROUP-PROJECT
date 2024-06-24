using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    [SerializeField] float chargeJumpRange = 4;
    [SerializeField] float _projectileSpeed;
    [SerializeField] float _projectileDamage;

    [Header("AnimationSettings")]
    [SerializeField] float neckMoveAmount;

    [SerializeField] GameObject[] DamageLimb;

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

    [Header("Roaming")]
    [SerializeField] float wanderTimer;
    [SerializeField] float wanderRadius;

    [Header("RangedAttackRequirements")]
    [SerializeField] Transform _parent;
    [SerializeField] GameObject _projectile;
    [SerializeField] Transform _projectileSpawnPosition;
    [Range(0, 360)]
    public float throwAngle;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] EnemyAudioClips enemyAudio;

    [Header("Dependancy")]
    [SerializeField] EnemyBrain _brain;
    [SerializeField] QuestManager _questManager;
    [SerializeField] GameObject _fuzzyLogicVisuals;
    [SerializeField] GameObject otherCanvas;
    [SerializeField] InventoryManager invManager;

    //Private/not serialized variables

    private float startSpeed;
    private float timer;

    private float timeSinceLastAction;

    private bool hasNotEnteredCombat;
    private bool aimingForPlayer;

    //bools to set sertain states during the chargin animation
    private bool chargingAtPlayer;
    private bool jumpAtPlayer;
    private bool landed;

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
        _moveSpeed = _moveSpeed * Time.deltaTime;

        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.speed = _moveSpeed;
        startSpeed = _moveSpeed;

        timer = wanderTimer;

        hasNotEnteredCombat = true;

        StartCoroutine(FieldOfViewRoutine());

        if (GameObject.Find("QuestManager") != null)
        {
            _questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        }

        animator = GetComponent<Animator>();

        base.Start();
    }

    public override void Update()
    {
        //Checks if player is insight hasent enterd combat and que is not empty if so begins the combat chain by executing a attack
        if (playerInSight && hasNotEnteredCombat && _brain.attackQueue.Count != 0)
        {
            hasNotEnteredCombat = false;
            ExecuteAttack();
        }

        //CheckIfPlayerIsLeftOrRight();

        if (_agent.destination != null)
        {
            InstantlyTurn(_agent.destination);
        }

        if (aimingForPlayer)
        {
            InstantlyTurn(player.transform.position);
        }

        ChargeMovement();

        if (!playerInSight)
        {
            timer += Time.deltaTime;

            if (pathComplete() == true)
            {
                if (animator.GetInteger("WalkDir") != 0)
                {
                    animator.SetInteger("WalkDir", 0);
                }
            }
            else
            {
                if (animator.GetInteger("WalkDir") != 1)
                {
                    animator.SetInteger("WalkDir", 1);
                }
            }

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, 3);
                _agent.SetDestination(newPos);
                timer = 0;
            }
        }
        else
        {
            timeSinceLastAction += Time.deltaTime;
        }

        if (timeSinceLastAction > 20)
        {
            timeSinceLastAction = 0;

            if(_brain.attackQueue.Count == 0)
            {
                _brain.MakeDesicion();
                ExecuteAttack();
            }
            else
            {
                ExecuteAttack();
            }
        }

        //debug to see some visualized fuzzy logic
        if (Input.GetKeyDown(KeyCode.F1))
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

            if (DataPersistenceManager.instance != null)
            {
                DataPersistenceManager.instance.SaveManualGame();
            }

            SceneFader.Instance.FadeTo("GuildHall");
        }

        _distance = Vector3.Distance(transform.position, player.transform.position);

        //sets the vallue's in fuzzy logic script so they can be used for action calculation
        _logic.enemyHealth = _healthPoints / _maxHealth * 100;
        _logic.energy = _energy / _maxEnergy * 100;
        _logic.distance = _distance;
    }

    #endregion

    #region AnimationVoids

    //these voids get called with animation events

    #region Hit/Damage Timing
    //these voids get used to enable/disable the damage dealing part of limbs so that only the left claw deals damage during left claw attack etc.

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
        else if (bodypart == "All")
        {
            isLeftClawAtacking = true;
            isRightClawAttacking = true;
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
        else if (bodypart == "All")
        {
            isLeftClawAtacking = false;
            isRightClawAttacking = false;
            isHeadAtacking = false;
        }
    }

    public void StopAiming()
    {
        aimingForPlayer = false;
    }

    public void SetDamage(float dmg)
    {
        for (int i = 0; i < DamageLimb.Length; i++)
        {
            DamageLimb[i].GetComponent<MeleeCollision>().damage = dmg;
        }
    }

    #endregion

    #region AnimationQueuing

    public void QueueAttack()
    {
        timeSinceLastAction = 0;

        //makes sure the enemy queue's an attack when standing stil or continues walking to/away from the player when in that naimation state.

        if (animator.GetInteger("WalkDir") == 0 && playerInSight)
        {
            _brain.MakeDesicion();
        }
        else if (animator.GetInteger("WalkDir") == 1 && playerInSight)
        {
            if (_distance >= _logic.PeakMiddleDistance && _distance <= _logic.EndMiddleDistance && Random.Range(1, 11) == 5)
            {
                animator.SetInteger("WalkDir", 0);

                _brain.MakeDesicion();

                _brain.attackQueue.Dequeue();

                ExecuteAttack();
            }
            else
            {
                Engage();
            }
        }
        else if (animator.GetInteger("WalkDir") == -1 && playerInSight)
        {
            Disengage();
        }
    }

    public void ExecuteAttack()
    {
        timeSinceLastAction = 0;

        // this void gets called at the end of a animation to execute the next queued attack

        if (playerInSight && _brain.attackQueue.Count != 0)
        {
            _agent.ResetPath();

            landed = false;
            jumpAtPlayer = false;
            chargingAtPlayer = false;

            aimingForPlayer = false;

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
                    aimingForPlayer = true;
                    SpikeThrow();
                    break;
                case Attacks attack when attack == Attacks.LeftClaw:
                    aimingForPlayer = true;
                    LeftClawAttack();
                    break;
                case Attacks attack when attack == Attacks.RightClaw:
                    aimingForPlayer = true;
                    RightClawAttack();
                    break;
                case Attacks attack when attack == Attacks.Bite:
                    aimingForPlayer = true;
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
        else if (animator.GetInteger("WalkDir") == 1 && animator.GetInteger("WalkDir") != -1 && playerInSight)
        {
            Engage();
        }
        else if (animator.GetInteger("WalkDir") == -1 && animator.GetInteger("WalkDir") != 0 && playerInSight)
        {
            Disengage();
        }
    }

    #endregion

    #region SpikeThrowTiming
    //these voids get called at specific times during the spikethrow animation making sure that the spike spawns in its hand an that its get proppeled forward when thrown.

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

    #region ChargeTimings

    //this void makes the enemy not be able to change its charge direction
    public void ChargeJump()
    {
        timeSinceLastAction = 0;

        landed = false;
        chargingAtPlayer = false;
        jumpAtPlayer = true;
    }

    //this fully stops the enemy's charge
    public void JumpLand()
    {
        timeSinceLastAction = 0;

        chargingAtPlayer = false;
        jumpAtPlayer = false;
        landed = true;
    }

    public void ChargingAtPlayer()
    {
        timeSinceLastAction = 0;

        if (_distance < chargeJumpRange)
        {
            animator.SetTrigger("DashAttack");
        }
    }

    #endregion

    #region Sound Timing

    public void PlaySound(string soundToPlay)
    {
        switch (soundToPlay)
        {
            case string soundName when soundName == "Walk":
                audioSource.clip = enemyAudio.walkSound;
                audioSource.Play();
                break;
            case string soundName when soundName == "Bite":
                audioSource.clip = enemyAudio.biteAttackSound;
                audioSource.Play();
                break;
            case string soundName when soundName == "ClawL":
                audioSource.clip = enemyAudio.leftAttackSound;
                audioSource.Play();
                break;
            case string soundName when soundName == "ClawR":
                audioSource.clip = enemyAudio.rightAttackSound;
                audioSource.Play();
                break;
            default:
                Debug.LogError("AAAAAAAAAAAAAAAAAAAAAAAAA");
                break;
        }
    }

    #endregion

    #endregion

    #region AttackVoids

    //Voids that correspond to every attack the enemy does
    //These voids get called in ExecuteAttack() inside the AnimationVoids Region

    #region Movement

    public void Engage()
    {
        timeSinceLastAction = 0;

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
            if (_brain.attackQueue.Count != 0)
            {
                _brain.attackQueue.Dequeue();
                _brain.attackQueue.Clear();
            }

            _brain.MakeDesicion();

            _brain.attacksCurrentlyInQueue = _brain.attackQueue.ToArray();
        }
    }

    public void Disengage()
    {
        timeSinceLastAction = 0;

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

    #endregion

    #region StatusRegainingAction
    public IEnumerator RegainEnergy()
    {
        timeSinceLastAction = 0;

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

    #endregion

    #region RangedAttacks

    public void SpikeThrow()
    {
        timeSinceLastAction = 0;

        animator.SetTrigger("SpikeThrow");

        Exhaustion(_exhaustionSpeed * 750);

        if (_brain.attackQueue.Peek() == Attacks.SpikeThrow)
        {
            _brain.attackQueue.Dequeue();
        }
    }

    public void ChargeAttack()
    {
        timeSinceLastAction = 0;

        animator.SetTrigger("DashAttack");

        chargingAtPlayer = true;

        Exhaustion(_exhaustionSpeed * 1000);

        if (_brain.attackQueue.Peek() == Attacks.Charge)
        {
            _brain.attackQueue.Dequeue();
        }
    }

    #endregion

    #region MeleeAttacks

    public void LeftClawAttack()
    {
        timeSinceLastAction = 0;

        animator.SetTrigger("LeftClawAttack");

        Exhaustion(_exhaustionSpeed * 1000);

        if (_brain.attackQueue.Peek() == Attacks.LeftClaw)
        {
            _brain.attackQueue.Dequeue();
        }
    }

    public void RightClawAttack()
    {
        timeSinceLastAction = 0;

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

    #endregion


    #endregion

    #region Checks

    //checks in what state the charge is when the enemy is charging
    public void ChargeMovement()
    {
        if (chargingAtPlayer && !jumpAtPlayer && !landed)
        {
            //charging
            _agent.speed = startSpeed * 1.4f;
            _agent.destination = player.transform.position;
        }
        else if (!chargingAtPlayer && jumpAtPlayer && !landed)
        {
            //Jumped
            _agent.destination = transform.position + transform.forward * 2f;
        }
        else if (!chargingAtPlayer && !jumpAtPlayer && landed)
        {
            //landed
            _agent.speed = startSpeed;
        }
    }

    private void InstantlyTurn(Vector3 destination)
    {
        //When on target -> dont rotate!
        if ((destination - transform.position).magnitude < 0.1f) return;

        Vector3 direction = (destination - transform.position).normalized;
        Quaternion qDir = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, qDir, Time.deltaTime * _agent.angularSpeed);
    }

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

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    protected bool pathComplete()
    {
        if (Vector3.Distance(_agent.destination, _agent.transform.position) <= _agent.stoppingDistance)
        {
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }

        return false;
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
                    //playerInSight = false;
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
