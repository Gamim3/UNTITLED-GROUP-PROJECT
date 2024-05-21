using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public FuzzyLogic fuzzyLogic;
    public int DecisionTime;

    Vector3 playerHealthData;
    Vector3 enemyHealthData;
    Vector3 energyData;
    Vector3 distanceData;

    public Queue<Attacks> attackQueue;

    public Attacks[] notImplementedAttacks;
    public Attacks[] attacksCurrentlyInQueue;

    private EnemyHealth enemyHealth;
    private PlayerHealth playerHealth;
    private Energy energy;
    private Distance distance;

    private Enemy enemy;

    public void Start()
    {
        enemy = GetComponent<Enemy>();

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

        if (enemy.detectedPlayer && attackQueue.Count != 0)
        {
            switch (attackQueue.Peek())
            {
                case Attacks attack when attack == Attacks.Engage:
                    enemy.engaging = true;
                    enemy.Engage();
                    break;
                case Attacks attack when attack == Attacks.DisengageDash:
                    enemy.disengaging = true;
                    enemy.Disengage();
                    break;
                case Attacks attack when attack == Attacks.RegainEnergy:
                    enemy.regainingEnergy = true;
                    enemy.RegainEnergy();
                    break;
                case Attacks attack when attack == Attacks.SpikeThrow:
                    enemy.throwingSpike = true;
                    enemy.SpikeThrow();
                    break;
            }

            for (int i = 0; i < notImplementedAttacks.Length; i++)
            {
                if (attackQueue.Count != 0)
                {
                    if (attackQueue.Peek() == notImplementedAttacks[i])
                    {
                        attackQueue.Clear();
                    }
                }
            }
        }
    }

    public IEnumerator Think()
    {
        yield return new WaitForSeconds(DecisionTime);

        MakeDesicion();

        StartCoroutine(Think());
    }

    public void MakeDesicion()
    {
        float enemyHealthState = Mathf.Max(enemyHealthData.x, Mathf.Max(enemyHealthData.y, enemyHealthData.z));
        float playerHealthState = Mathf.Max(playerHealthData.x, Mathf.Max(playerHealthData.y, playerHealthData.z));
        float energyState = Mathf.Max(energyData.x, Mathf.Max(energyData.y, energyData.z));
        float distanceState = Mathf.Max(distanceData.x, Mathf.Max(distanceData.y, distanceData.z));

        ConvertToReadableEnum(enemyHealthState, playerHealthState, energyState, distanceState);

        // Switch case based on the highest value
        switch (enemyHealth)
        {
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Far:
                //("enemy(Healthy) | player(Healthy) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Medium:
                //("enemy(Healthy) | player(Healthy) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Close:
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
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Far:
                //("enemy(Healthy) | player(Healthy) | energy(medium) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Medium:
                //("enemy(Healthy) | player(Healthy) | energy(medium) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Close:
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
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Far:
                //("enemy(Healthy) | player(Healthy) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Medium:
                //("enemy(Healthy) | player(Healthy) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Close:
                //("enemy(Healthy) | player(Healthy) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Far:
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
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Medium:
                //("enemy(Healthy) | player(hurt) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Close:
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
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Far:
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
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Medium:
                //("enemy(Healthy) | player(hurt) | energy(medium) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Close:
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
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Far:
                //("enemy(Healthy) | player(hurt) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Medium:
                //("enemy(Healthy) | player(hurt) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Close:
                //("enemy(Healthy) | player(hurt) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Far:
                //("enemy(Healthy) | player(critical) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Medium:
                //("enemy(Healthy) | player(critical) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Close:
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
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Far:
                //("enemy(Healthy) | player(critical) | energy(medium) | distance(far)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Medium:
                //("enemy(Healthy) | player(critical) | energy(medium) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Close:
                //("enemy(Healthy) | player(critical) | energy(medium) | distance(close)");
                attackQueue.Enqueue(Attacks.SpikeSlamAttack);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Far:
                //("enemy(Healthy) | player(critical) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Medium:
                //("enemy(Healthy) | player(critical) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Close:
                //("enemy(Healthy) | player(critical) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Far:
                //("enemy(hurt) | player(Healthy) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Medium:
                //("enemy(hurt) | player(Healthy) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Close:
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
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Far:
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
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Medium:
                //("enemy(hurt) | player(Healthy) | energy(medium) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Close:
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
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Far:
                //("enemy(hurt) | player(Healthy) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Medium:
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
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Close:
                //("enemy(hurt) | player(Healthy) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Far:
                //("enemy(hurt) | player(hurt) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Medium:
                //("enemy(hurt) | player(hurt) | energy(full) | distance(middle)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.Engage);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.SpikeThrow);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Close:
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
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Far:
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
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Medium:
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
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Close:
                //("enemy(hurt) | player(hurt) | energy(medium) | distance(close)");
                switch (Random.Range(0, 2))
                {
                    case 0:
                        attackQueue.Enqueue(Attacks.LeftClaw);
                        break;

                    case 1:
                        attackQueue.Enqueue(Attacks.DisengageDash);
                        break;
                }
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Far:
                //("enemy(hurt) | player(hurt) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Medium:
                //("enemy(hurt) | player(hurt) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Close:
                //("enemy(hurt) | player(hurt) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Far:
                //("enemy(hurt) | player(critical) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Medium:
                //("enemy(hurt) | player(critical) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.Engage);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Close:
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
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Far:
                //("enemy(hurt) | player(critical) | energy(medium) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Medium:
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
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Close:
                //("enemy(hurt) | player(critical) | energy(medium) | distance(close)");
                attackQueue.Enqueue(Attacks.SpikeSlamAttack);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Far:
                //("enemy(hurt) | player(critical) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Medium:
                //("enemy(hurt) | player(critical) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Close:
                //("enemy(hurt) | player(critical) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Far:
                //("enemy(critical) | player(Healthy) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Medium:
                //("enemy(critical) | player(Healthy) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Close:
                //("enemy(critical) | player(Healthy) | energy(full) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Far:
                //("enemy(critical) | player(Healthy) | energy(medium) | distance(far)")
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Medium:
                //("enemy(critical) | player(Healthy) | energy(medium) | distance(middle)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Close:
                //("enemy(critical) | player(Healthy) | energy(medium) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Far:
                //("enemy(critical) | player(Healthy) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Medium:
                //("enemy(critical) | player(Healthy) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Close:
                //("enemy(critical) | player(Healthy) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Far:
                //("enemy(critical) | player(hurt) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Medium:
                //("enemy(critical) | player(hurt) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Close:
                //("enemy(critical) | player(hurt) | energy(full) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Far:
                //("enemy(critical) | player(hurt) | energy(medium) | distance(far)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Medium:
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
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Close:
                //("enemy(critical) | player(hurt) | energy(medium) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Far:
                //("enemy(critical) | player(hurt) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Medium:
                //("enemy(critical) | player(hurt) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Close:
                //("enemy(critical) | player(hurt) | energy(low) | distance(close)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Far:
                //("enemy(critical) | player(critical) | energy(full) | distance(far)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Medium:
                //("enemy(critical) | player(critical) | energy(full) | distance(middle)");
                attackQueue.Enqueue(Attacks.DisengageDash);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Close:
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
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Far:
                //("enemy(critical) | player(critical) | energy(medium) | distance(far)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Medium:
                //("enemy(critical) | player(critical) | energy(medium) | distance(middle)");
                attackQueue.Enqueue(Attacks.SpikeThrow);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Close:
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
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Far:
                //("enemy(critical) | player(critical) | energy(low) | distance(far)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Medium:
                //("enemy(critical) | player(critical) | energy(low) | distance(middle)");
                attackQueue.Enqueue(Attacks.RegainEnergy);
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Close:
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
            case float state when state == enemyHealthData.z:
                enemyHealth = EnemyHealth.Healthy;
                break;
            case float state when state == enemyHealthData.y:
                enemyHealth = EnemyHealth.Hurt;
                break;
            case float state when state == enemyHealthData.x:
                enemyHealth = EnemyHealth.Critical;
                break;
        }

        switch (playerHealthState)
        {
            case float state when state == playerHealthData.z:
                playerHealth = PlayerHealth.Healthy;
                break;
            case float state when state == playerHealthData.y:
                playerHealth = PlayerHealth.Hurt;
                break;
            case float state when state == playerHealthData.x:
                playerHealth = PlayerHealth.Critical;
                break;
        }

        switch (energyState)
        {
            case float state when state == energyData.z:
                energy = Energy.High;
                break;
            case float state when state == energyData.y:
                energy = Energy.Medium;
                break;
            case float state when state == energyData.x:
                energy = Energy.Low;
                break;
        }

        switch (distanceState)
        {
            case float state when state == distanceData.z:
                distance = Distance.Far;
                break;
            case float state when state == distanceData.y:
                distance = Distance.Medium;
                break;
            case float state when state == distanceData.x:
                distance = Distance.Close;
                break;
        }
    }

    public void GetFuzzyData()
    {
        playerHealthData = fuzzyLogic.fuzzyPlayerHealth;
        enemyHealthData = fuzzyLogic.fuzzyEnemyHealth;
        energyData = fuzzyLogic.fuzzyEnergy;
        distanceData = fuzzyLogic.fuzzyDistance;
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
        Block,
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