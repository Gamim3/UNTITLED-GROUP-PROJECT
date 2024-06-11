using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] FuzzyLogic _fuzzyLogic;

    Vector3 _playerHealthData;
    Vector3 _enemyHealthData;
    Vector3 _energyData;
    Vector3 _distanceData;

    public Queue<Attacks> attackQueue;
    [NonSerialized] public int enemyState;

    [SerializeField] Attacks[] _notImplementedAttacks;
    public Attacks[] attacksCurrentlyInQueue;

    [NonSerialized] EnemyHealth _enemyHealth;
    [NonSerialized] PlayerHealth _playerHealth;
    [NonSerialized] Energy _energy;
    [NonSerialized] Distance _distance;

    [NonSerialized] Enemy _enemy;

    public void Start()
    {
        _fuzzyLogic = GetComponent<FuzzyLogic>();

        _enemy = GetComponent<Enemy>();

        attackQueue = new Queue<Attacks>();

        MakeDesicion();
    }

    public void Update()
    {
        GetFuzzyData();

        //Queue debugging
        if (Input.GetKeyDown(KeyCode.E))
        {
            attackQueue.Clear();
        }

        if (_enemy.playerInSight)
        {
            for (int i = 0; i < _notImplementedAttacks.Length; i++)
            {
                if (attackQueue.Count != 0)
                {
                    if (attackQueue.Peek() == _notImplementedAttacks[i])
                    {
                        attackQueue.Clear();
                        MakeDesicion();
                    }
                }
            }
        }
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
                enemyState = 1;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(Healthy) | player(Healthy) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.Charge);
                enemyState = 2;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(Healthy) | player(Healthy) | energy(full) | distance(close)");
                enemyState = 4;
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
                enemyState = 5;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Medium && _distance == Distance.Medium:
                //("enemy(Healthy) | player(Healthy) | energy(medium) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                enemyState = 6;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Medium && _distance == Distance.Close:
                //("enemy(Healthy) | player(Healthy) | energy(medium) | distance(close)");
                enemyState = 7;
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
                enemyState = 8;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(Healthy) | player(Healthy) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                enemyState = 9;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(Healthy) | player(Healthy) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                enemyState = 10;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(Healthy) | player(hurt) | energy(full) | distance(far)");
                enemyState = 11;
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
                attackQueue.Enqueue(Attacks.Charge);
                enemyState = 12;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(Healthy) | player(hurt) | energy(full) | distance(close)");
                enemyState = 13;
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
                enemyState = 14;
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
                enemyState = 15;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Medium && _distance == Distance.Close:
                //("enemy(Healthy) | player(hurt) | energy(medium) | distance(close)");
                enemyState = 16;
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
                enemyState = 17;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(Healthy) | player(hurt) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                enemyState = 18;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(Healthy) | player(hurt) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                enemyState = 19;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(Healthy) | player(critical) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.Engage);
                enemyState = 20;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(Healthy) | player(critical) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.Engage);
                enemyState = 21;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(Healthy) | player(critical) | energy(full) | distance(close)");
                enemyState = 22;
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
                enemyState = 23;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.Medium && _distance == Distance.Medium:
                //("enemy(Healthy) | player(critical) | energy(medium) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                enemyState = 24;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.Medium && _distance == Distance.Close:
                //("enemy(Healthy) | player(critical) | energy(medium) | distance(close)");
                enemyState = 25;
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
                enemyState = 26;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(Healthy) | player(critical) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                enemyState = 27;
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && _playerHealth == PlayerHealth.Critical && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(Healthy) | player(critical) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                enemyState = 28;
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(hurt) | player(Healthy) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                enemyState = 29;
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(hurt) | player(Healthy) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                enemyState = 30;
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(hurt) | player(Healthy) | energy(full) | distance(close)");
                enemyState = 31;
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
                enemyState = 32;
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
                enemyState = 32;
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Medium && _distance == Distance.Close:
                //("enemy(hurt) | player(Healthy) | energy(medium) | distance(close)");
                enemyState = 33;
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
                enemyState = 34;
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(hurt) | player(Healthy) | energy(low) | distance(middle)");
                enemyState = 35;
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
                enemyState = 36;
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(hurt) | player(hurt) | energy(full) | distance(far)");
                enemyState = 37;
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(hurt) | player(hurt) | energy(full) | distance(middle)");
                enemyState = 38;
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(hurt) | player(hurt) | energy(full) | distance(close)");
                enemyState = 39;
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
                enemyState = 40;
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
                enemyState = 41;
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
                enemyState = 42;
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
                enemyState = 43;
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(hurt) | player(hurt) | energy(low) | distance(middle)");
                enemyState = 44;
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(hurt) | player(hurt) | energy(low) | distance(close)");
                enemyState = 45;
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(hurt) | player(critical) | energy(full) | distance(far)");
                enemyState = 46;
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(hurt) | player(critical) | energy(full) | distance(middle)");
                enemyState = 47;
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(hurt) | player(critical) | energy(full) | distance(close)");
                enemyState = 48;
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
                enemyState = 49;
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Critical && _energy == Energy.Medium && _distance == Distance.Medium:
                //("enemy(hurt) | player(critical) | energy(medium) | distance(middle)");
                enemyState = 50;
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
                enemyState = 51;
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
                enemyState = 52;
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Critical && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(hurt) | player(critical) | energy(low) | distance(middle)");
                enemyState = 53;
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && _playerHealth == PlayerHealth.Critical && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(hurt) | player(critical) | energy(low) | distance(close)");
                enemyState = 54;
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(critical) | player(Healthy) | energy(full) | distance(far)");
                enemyState = 55;
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(critical) | player(Healthy) | energy(full) | distance(middle)");
                enemyState = 56;
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(critical) | player(Healthy) | energy(full) | distance(close)");
                enemyState = 57;
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Medium && _distance == Distance.Far:
                //("enemy(critical) | player(Healthy) | energy(medium) | distance(far)")
                enemyState = 58;
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Medium && _distance == Distance.Medium:
                //("enemy(critical) | player(Healthy) | energy(medium) | distance(middle)");
                enemyState = 59;
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Medium && _distance == Distance.Close:
                //("enemy(critical) | player(Healthy) | energy(medium) | distance(close)");
                enemyState = 60;
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Low && _distance == Distance.Far:
                //("enemy(critical) | player(Healthy) | energy(low) | distance(far)");
                enemyState = 61;
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(critical) | player(Healthy) | energy(low) | distance(middle)");
                enemyState = 62;
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Healthy && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(critical) | player(Healthy) | energy(low) | distance(close)");
                enemyState = 63;
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(critical) | player(hurt) | energy(full) | distance(far)");
                enemyState = 64;
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(critical) | player(hurt) | energy(full) | distance(middle)");
                enemyState = 65;
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(critical) | player(hurt) | energy(full) | distance(close)");
                enemyState = 66;
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Medium && _distance == Distance.Far:
                //("enemy(critical) | player(hurt) | energy(medium) | distance(far)");
                enemyState = 67;
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Medium && _distance == Distance.Medium:
                //("enemy(critical) | player(hurt) | energy(medium) | distance(middle)");
                enemyState = 68;
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
                enemyState = 69;
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Far:
                //("enemy(critical) | player(hurt) | energy(low) | distance(far)");
                enemyState = 70;
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(critical) | player(hurt) | energy(low) | distance(middle)");
                enemyState = 71;
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Hurt && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(critical) | player(hurt) | energy(low) | distance(close)");
                enemyState = 72;
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Far:
                //("enemy(critical) | player(critical) | energy(full) | distance(far)");
                enemyState = 73;
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Medium:
                //("enemy(critical) | player(critical) | energy(full) | distance(middle)");
                enemyState = 74;
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.High && _distance == Distance.Close:
                //("enemy(critical) | player(critical) | energy(full) | distance(close)");
                enemyState = 75;
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
                enemyState = 76;
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.Medium && _distance == Distance.Medium:
                //("enemy(critical) | player(critical) | energy(medium) | distance(middle)");
                enemyState = 77;
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.Medium && _distance == Distance.Close:
                //("enemy(critical) | player(critical) | energy(medium) | distance(close)");
                enemyState = 78;
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
                enemyState = 79;
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.Low && _distance == Distance.Medium:
                //("enemy(critical) | player(critical) | energy(low) | distance(middle)");
                enemyState = 80;
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && _playerHealth == PlayerHealth.Critical && _energy == Energy.Low && _distance == Distance.Close:
                //("enemy(critical) | player(critical) | energy(low) | distance(close)");
                enemyState = 81;
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
        Charge,
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