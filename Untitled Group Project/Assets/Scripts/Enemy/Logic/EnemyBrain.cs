using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] FuzzyLogic _fuzzyLogic;
    [SerializeField] int _decisionTime;

    Vector3 _playerHealthData;
    Vector3 _enemyHealthData;
    Vector3 _energyData;
    Vector3 _distanceData;

    public Queue<Attacks> attackQueue;

    [SerializeField] Attacks[] _notImplementedAttacks;
    [SerializeField] Attacks[] attacksCurrentlyInQueue;

    [NonSerialized] EnemyHealth _enemyHealth;
    [NonSerialized] PlayerHealth _playerHealth;
    [NonSerialized] Energy _energy;
    [NonSerialized] Distance _distance;

    [NonSerialized] Enemy _enemy;

    public void Start()
    {
        _enemy = GetComponent<Enemy>();

        attackQueue = new Queue<Attacks>();

        StartCoroutine(Think());
    }

    public void Update()
    {
        GetFuzzyData();

        //Queue debugging
        if (Input.GetKeyDown(KeyCode.E))
        {
            attackQueue.Clear();
        }

        if (_enemy.playerInSight && attackQueue.Count != 0)
        {
            switch (attackQueue.Peek())
            {
                case Attacks attack when attack == Attacks.Engage:
                    _enemy.engaging = true;
                    _enemy.Engage();
                    break;
                case Attacks attack when attack == Attacks.DisengageDash:
                    _enemy.disengaging = true;
                    _enemy.Disengage();
                    break;
                case Attacks attack when attack == Attacks.RegainEnergy:
                    _enemy.regainingEnergy = true;
                    _enemy.RegainEnergy();
                    break;
                case Attacks attack when attack == Attacks.SpikeThrow:
                    _enemy.throwingSpike = true;
                    _enemy.SpikeThrow();
                    break;
                case Attacks attack when attack == Attacks.LeftClaw:
                    _enemy.leftClawAttack = true;
                    _enemy.LeftClawAttack();
                    break;
                case Attacks attack when attack == Attacks.RightClaw:
                    _enemy.rightClawAttack = true;
                    _enemy.RightClawAttack();
                    break;
            }

            for (int i = 0; i < _notImplementedAttacks.Length; i++)
            {
                if (attackQueue.Count != 0)
                {
                    if (attackQueue.Peek() == _notImplementedAttacks[i])
                    {
                        attackQueue.Clear();
                    }
                }
            }
        }
    }

    public IEnumerator Think()
    {
        yield return new WaitForSeconds(_decisionTime);

        MakeDesicion();

        StartCoroutine(Think());
    }

    public void MakeDesicion()
    {
        float enemyHealthState = Mathf.Max(_enemyHealthData.x, Mathf.Max(_enemyHealthData.y, _enemyHealthData.z));
        float playerHealthState = Mathf.Max(_playerHealthData.x, Mathf.Max(_playerHealthData.y, _playerHealthData.z));
        float energyState = Mathf.Max(_energyData.x, Mathf.Max(_energyData.y, _energyData.z));
        float distanceState = Mathf.Max(_distanceData.x, Mathf.Max(_distanceData.y, _distanceData.z));

        ConvertToReadableEnum(enemyHealthState, playerHealthState, energyState, distanceState);

        // Switch case based on the highest value
        switch (_enemyHealth)
        {
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(Healthy) | player(Healthy) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(Healthy) | player(Healthy) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(Healthy) | player(Healthy) | energy(full) | distance(close)");
                switch (Random.Range(0, 3))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.SpikeSlamAttack);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.LeftClaw);
                        break;

                    case 2:
                        attackQueue.Enqueue(Attacks.RightClaw);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Medium && _distance == Distance.Far:
                //("enemy(Healthy) | player(Healthy) | energy(medium) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Medium && _distance == Distance.Medium:
                //("enemy(Healthy) | player(Healthy) | energy(medium) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Medium && _distance == Distance.Close:
                //("enemy(Healthy) | player(Healthy) | energy(medium) | distance(close)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.LeftClaw);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.RightClaw);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Low && _distance == Distance.Far:
                //("enemy(Healthy) | player(Healthy) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(Healthy) | player(Healthy) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(Healthy) | player(Healthy) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(Healthy) | player(hurt) | energy(full) | distance(far)");
                switch (Random.Range(0, 3))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.SpikeThrow);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.Engage);
                        break;

                    case 2:
                        attackQueue.Enqueue(Attacks.Engage);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(Healthy) | player(hurt) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(Healthy) | player(hurt) | energy(full) | distance(close)");
                switch (Random.Range(0, 3))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.SpikeSlamAttack);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.LeftClaw);
                        break;

                    case 2:
                        attackQueue.Enqueue(Attacks.RightClaw);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Medium && _distance == Distance.Far:
                //("enemy(Healthy) | player(hurt) | energy(medium) | distance(far)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.RegainEnergy);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.Engage);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Medium && _distance == Distance.Medium:
                //("enemy(Healthy) | player(hurt) | energy(medium) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Medium && _distance == Distance.Close:
                //("enemy(Healthy) | player(hurt) | energy(medium) | distance(close)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.LeftClaw);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.RightClaw);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Far:
                //("enemy(Healthy) | player(hurt) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(Healthy) | player(hurt) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(Healthy) | player(hurt) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(Healthy) | player(critical) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(Healthy) | player(critical) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(Healthy) | player(critical) | energy(full) | distance(close)");
                switch (Random.Range(0, 4))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.SpikeSlamAttack);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.LeftClaw);
                        break;

                    case 2:
                        attackQueue.Enqueue(Attacks.RightClaw);
                        break;

                    case 3:
                        attackQueue.Enqueue(Attacks.SpikeSlamAttack);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.Medium && _distance == Distance.Far:
                //("enemy(Healthy) | player(critical) | energy(medium) | distance(far)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.Medium && _distance == Distance.Medium:
                //("enemy(Healthy) | player(critical) | energy(medium) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.Medium && _distance == Distance.Close:
                //("enemy(Healthy) | player(critical) | energy(medium) | distance(close)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.RightClaw);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.LeftClaw);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.Low && _distance == Distance.Far:
                //("enemy(Healthy) | player(critical) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(Healthy) | player(critical) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(Healthy) | player(critical) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(hurt) | player(Healthy) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(hurt) | player(Healthy) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(hurt) | player(Healthy) | energy(full) | distance(close)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.LeftClaw);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.RightClaw);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Medium && _distance == Distance.Far:
                //("enemy(hurt) | player(Healthy) | energy(medium) | distance(far)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.SpikeThrow);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.RegainEnergy);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Medium && _distance == Distance.Medium:
                //("enemy(hurt) | player(Healthy) | energy(medium) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Medium && _distance == Distance.Close:
                //("enemy(hurt) | player(Healthy) | energy(medium) | distance(close)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.LeftClaw);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.RightClaw);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Low && _distance == Distance.Far:
                //("enemy(hurt) | player(Healthy) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(hurt) | player(Healthy) | energy(low) | distance(middle)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.RegainEnergy);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.DisengageDash);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(hurt) | player(Healthy) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(hurt) | player(hurt) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(hurt) | player(hurt) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(hurt) | player(hurt) | energy(full) | distance(close)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.LeftClaw);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.RightClaw);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Medium && _distance == Distance.Far:
                //("enemy(hurt) | player(hurt) | energy(medium) | distance(far)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.SpikeThrow);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.RegainEnergy);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Medium && _distance == Distance.Medium:
                //("enemy(hurt) | player(hurt) | energy(medium) | distance(middle)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.SpikeThrow);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.RegainEnergy);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Medium && _distance == Distance.Close:
                //("enemy(hurt) | player(hurt) | energy(medium) | distance(close)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.LeftClaw);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.RightClaw);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Far:
                //("enemy(hurt) | player(hurt) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(hurt) | player(hurt) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(hurt) | player(hurt) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(hurt) | player(critical) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(hurt) | player(critical) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(hurt) | player(critical) | energy(full) | distance(close)");
                switch (Random.Range(0, 4))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.SpikeSlamAttack);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.LeftClaw);
                        break;

                    case 2:
                        attackQueue.Enqueue(Attacks.RightClaw);
                        break;

                    case 3:
                        attackQueue.Enqueue(Attacks.SpikeSlamAttack);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Critical && _energy == Energy.Medium && _distance == Distance.Far:
                //("enemy(hurt) | player(critical) | energy(medium) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Critical && _energy == Energy.Medium && _distance == Distance.Medium:
                //("enemy(hurt) | player(critical) | energy(medium) | distance(middle)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.RegainEnergy);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.Engage);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Critical && _energy == Energy.Medium && _distance == Distance.Close:
                //("enemy(hurt) | player(critical) | energy(medium) | distance(close)");
                switch (Random.Range(0, 3))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.SpikeSlamAttack);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.LeftClaw);
                        break;

                    case 2:
                        attackQueue.Enqueue(Attacks.RightClaw);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Critical && _energy == Energy.Low && _distance == Distance.Far:
                //("enemy(hurt) | player(critical) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Critical && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(hurt) | player(critical) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Critical && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(hurt) | player(critical) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(critical) | player(Healthy) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(critical) | player(Healthy) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(critical) | player(Healthy) | energy(full) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Medium && _distance == Distance.Far:
                //("enemy(critical) | player(Healthy) | energy(medium) | distance(far)")
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Medium && _distance == Distance.Medium:
                //("enemy(critical) | player(Healthy) | energy(medium) | distance(middle)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Medium && _distance == Distance.Close:
                //("enemy(critical) | player(Healthy) | energy(medium) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Low && _distance == Distance.Far:
                //("enemy(critical) | player(Healthy) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(critical) | player(Healthy) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(critical) | player(Healthy) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(critical) | player(hurt) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(critical) | player(hurt) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(critical) | player(hurt) | energy(full) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Medium && _distance == Distance.Far:
                //("enemy(critical) | player(hurt) | energy(medium) | distance(far)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Medium && _distance == Distance.Medium:
                //("enemy(critical) | player(hurt) | energy(medium) | distance(middle)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.SpikeThrow);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.DisengageDash);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Medium && _distance == Distance.Close:
                //("enemy(critical) | player(hurt) | energy(medium) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Far:
                //("enemy(critical) | player(hurt) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(critical) | player(hurt) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(critical) | player(hurt) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(critical) | player(critical) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(critical) | player(critical) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(critical) | player(critical) | energy(full) | distance(close)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.LeftClaw);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.RightClaw);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.Medium && _distance == Distance.Far:
                //("enemy(critical) | player(critical) | energy(medium) | distance(far)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.Medium && _distance == Distance.Medium:
                //("enemy(critical) | player(critical) | energy(medium) | distance(middle)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.Medium && _distance == Distance.Close:
                //("enemy(critical) | player(critical) | energy(medium) | distance(close)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.LeftClaw);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.RightClaw);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.Low && _distance == Distance.Far:
                //("enemy(critical) | player(critical) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(critical) | player(critical) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(critical) | player(critical) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;

            default:
                Debug.Log("geen case matched!! ||  eHPState " + enemyHealthState + " pHPState " + playerHealthState + " enState " + energyState + " dState " + distanceState);
                break;
        }
        attacksCurrentlyInQueue = attackQueue.ToArray();
    }

    public void ConvertToReadableEnum(float enemyHealthState, float playerHealthState, float energyState, float distanceState)
    {
        switch (enemyHealthState)
        {
            case float state when state == _enemyHealthData.z:
                _enemyHealth = EnemyHealth.Healthy;
                break;
            case float state when state == _enemyHealthData.y:
                _enemyHealth = EnemyHealth.Hurt;
                break;
            case float state when state == _enemyHealthData.x:
                _enemyHealth = EnemyHealth.Critical;
                break;
        }

        switch (playerHealthState)
        {
            case float state when state == _playerHealthData.z:
                _playerHealth = PlayerHealth.Healthy;
                break;
            case float state when state == _playerHealthData.y:
                _playerHealth = PlayerHealth.Hurt;
                break;
            case float state when state == _playerHealthData.x:
                _playerHealth = PlayerHealth.Critical;
                break;
        }

        switch (energyState)
        {
            case float state when state == _energyData.z:
                _energy = Energy.High;
                break;
            case float state when state == _energyData.y:
                _energy = Energy.Medium;
                break;
            case float state when state == _energyData.x:
                _energy = Energy.Low;
                break;
        }

        switch (distanceState)
        {
            case float state when state == _distanceData.z:
                _distance = Distance.Far;
                break;
            case float state when state == _distanceData.y:
                _distance = Distance.Medium;
                break;
            case float state when state == _distanceData.x:
                _distance = Distance.Close;
                break;
        }
    }

    public void GetFuzzyData()
    {
        _playerHealthData = _fuzzyLogic.fuzzyPlayerHealth;
        _enemyHealthData = _fuzzyLogic.fuzzyEnemyHealth;
        _energyData = _fuzzyLogic.fuzzyEnergy;
        _distanceData = _fuzzyLogic.fuzzyDistance;
    }

    public enum Attacks
    {
        SpikeThrow,
        LeftClaw,
        RightClaw,
        LeftClawBackAttack,
        RightClawBackAttack,
        SpikeSlamAttack,
        Engage,
        DisengageDash,
        RegainEnergy,
    }

    public enum EnemyHealth
    {
        Critical,
        Hurt,
        Healthy,
    }

    public enum PlayerHealth
    {
        Critical,
        Hurt,
        Healthy,
    }

    public enum Energy
    {
        Low,
        Medium,
        High,
    }

    public enum Distance
    {
        Close,
        Medium,
        Far,
    }
}