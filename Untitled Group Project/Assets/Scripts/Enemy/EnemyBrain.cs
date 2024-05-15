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

    private EnemyHealth enemyHealth;
    private PlayerHealth playerHealth;
    private Energy energy;
    private Distance distance;

    public void Start()
    {
        StartCoroutine(MakeDesicion());
    }

    public void Update()
    {
        playerHealthData = fuzzyLogic.fuzzyPlayerHealth;
        enemyHealthData = fuzzyLogic.fuzzyEnemyHealth;
        energyData = fuzzyLogic.fuzzyEnergy;
        distanceData = fuzzyLogic.fuzzyDistance;
    }

    public IEnumerator MakeDesicion()
    {
        yield return new WaitForSeconds(DecisionTime);

        Think();

        StartCoroutine(MakeDesicion());
    }

    public void Think()
    {
        float enemyHealthState = Mathf.Max(enemyHealthData.x, Mathf.Max(enemyHealthData.y, enemyHealthData.z));
        float playerHealthState = Mathf.Max(playerHealthData.x, Mathf.Max(playerHealthData.y, playerHealthData.z));
        float energyState = Mathf.Max(energyData.x, Mathf.Max(energyData.y, energyData.z));
        float distanceState = Mathf.Max(distanceData.x, Mathf.Max(distanceData.y, distanceData.z));

        ConvertToReadableEnum(enemyHealthState, playerHealthState, energyState, distanceState);

        // Switch case based on the highest value
        switch (enemyHealth)
        {
            case EnemyHealth state when state == EnemyHealth.Healthy &&  playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Far:
                Debug.Log("enemy(Healthy) | player(Healthy) | energy(full) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Medium:
                Debug.Log("enemy(Healthy) | player(Healthy) | energy(full) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Close:
                Debug.Log("enemy(Healthy) | player(Healthy) | energy(full) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Far:
                Debug.Log("enemy(Healthy) | player(Healthy) | energy(medium) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Medium:
                Debug.Log("enemy(Healthy) | player(Healthy) | energy(medium) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Close:
                Debug.Log("enemy(Healthy) | player(Healthy) | energy(medium) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Far:
                Debug.Log("enemy(Healthy) | player(Healthy) | energy(low) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Medium:
                Debug.Log("enemy(Healthy) | player(Healthy) | energy(low) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Close:
                Debug.Log("enemy(Healthy) | player(Healthy) | energy(low) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Far:
                Debug.Log("enemy(Healthy) | player(hurt) | energy(full) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Medium:
                Debug.Log("enemy(Healthy) | player(hurt) | energy(full) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Close:
                Debug.Log("enemy(Healthy) | player(hurt) | energy(full) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Far:
                Debug.Log("enemy(Healthy) | player(hurt) | energy(medium) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Medium:
                Debug.Log("enemy(Healthy) | player(hurt) | energy(medium) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Close:
                Debug.Log("enemy(Healthy) | player(hurt) | energy(medium) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Far:
                Debug.Log("enemy(Healthy) | player(hurt) | energy(low) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Medium:
                Debug.Log("enemy(Healthy) | player(hurt) | energy(low) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Close:
                Debug.Log("enemy(Healthy) | player(hurt) | energy(low) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Far:
                Debug.Log("enemy(Healthy) | player(critical) | energy(full) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Medium:
                Debug.Log("enemy(Healthy) | player(critical) | energy(full) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Close:
                Debug.Log("enemy(Healthy) | player(critical) | energy(full) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Far:
                Debug.Log("enemy(Healthy) | player(critical) | energy(medium) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Medium:
                Debug.Log("enemy(Healthy) | player(critical) | energy(medium) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Close:
                Debug.Log("enemy(Healthy) | player(critical) | energy(medium) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Far:
                Debug.Log("enemy(Healthy) | player(critical) | energy(low) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Medium:
                Debug.Log("enemy(Healthy) | player(critical) | energy(low) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Healthy && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Close:
                Debug.Log("enemy(Healthy) | player(critical) | energy(low) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Far:
                Debug.Log("enemy(hurt) | player(Healthy) | energy(full) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Medium:
                Debug.Log("enemy(hurt) | player(Healthy) | energy(full) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Close:
                Debug.Log("enemy(hurt) | player(Healthy) | energy(full) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Far:
                Debug.Log("enemy(hurt) | player(Healthy) | energy(medium) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Medium:
                Debug.Log("enemy(hurt) | player(Healthy) | energy(medium) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Close:
                Debug.Log("enemy(hurt) | player(Healthy) | energy(medium) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Far:
                Debug.Log("enemy(hurt) | player(Healthy) | energy(low) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Medium:
                Debug.Log("enemy(hurt) | player(Healthy) | energy(low) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Close:
                Debug.Log("enemy(hurt) | player(Healthy) | energy(low) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Far:
                Debug.Log("enemy(hurt) | player(hurt) | energy(full) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Medium:
                Debug.Log("enemy(hurt) | player(hurt) | energy(full) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Close:
                Debug.Log("enemy(hurt) | player(hurt) | energy(full) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Far:
                Debug.Log("enemy(hurt) | player(hurt) | energy(medium) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Medium:
                Debug.Log("enemy(hurt) | player(hurt) | energy(medium) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Close:
                Debug.Log("enemy(hurt) | player(hurt) | energy(medium) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Far:
                Debug.Log("enemy(hurt) | player(hurt) | energy(low) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Medium:
                Debug.Log("enemy(hurt) | player(hurt) | energy(low) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Close:
                Debug.Log("enemy(hurt) | player(hurt) | energy(low) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Far:
                Debug.Log("enemy(hurt) | player(critical) | energy(full) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Medium:
                Debug.Log("enemy(hurt) | player(critical) | energy(full) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Close:
                Debug.Log("enemy(hurt) | player(critical) | energy(full) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Far:
                Debug.Log("enemy(hurt) | player(critical) | energy(medium) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Medium:
                Debug.Log("enemy(hurt) | player(critical) | energy(medium) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Close:
                Debug.Log("enemy(hurt) | player(critical) | energy(medium) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Far:
                Debug.Log("enemy(hurt) | player(critical) | energy(low) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Medium:
                Debug.Log("enemy(hurt) | player(critical) | energy(low) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Hurt && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Close:
                Debug.Log("enemy(hurt) | player(critical) | energy(low) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Far:
                Debug.Log("enemy(critical) | player(Healthy) | energy(full) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Medium:
                Debug.Log("enemy(critical) | player(Healthy) | energy(full) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.High && distance == Distance.Close:
                Debug.Log("enemy(critical) | player(Healthy) | energy(full) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Far:
                Debug.Log("enemy(critical) | player(Healthy) | energy(medium) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Medium:
                Debug.Log("enemy(critical) | player(Healthy) | energy(medium) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.Medium && distance == Distance.Close:
                Debug.Log("enemy(critical) | player(Healthy) | energy(medium) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Far:
                Debug.Log("enemy(critical) | player(Healthy) | energy(low) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Medium:
                Debug.Log("enemy(critical) | player(Healthy) | energy(low) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Healthy && energy == Energy.Low && distance == Distance.Close:
                Debug.Log("enemy(critical) | player(Healthy) | energy(low) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Far:
                Debug.Log("enemy(critical) | player(hurt) | energy(full) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Medium:
                Debug.Log("enemy(critical) | player(hurt) | energy(full) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.High && distance == Distance.Close:
                Debug.Log("enemy(critical) | player(hurt) | energy(full) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Far:
                Debug.Log("enemy(critical) | player(hurt) | energy(medium) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Medium:
                Debug.Log("enemy(critical) | player(hurt) | energy(medium) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.Medium && distance == Distance.Close:
                Debug.Log("enemy(critical) | player(hurt) | energy(medium) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Far:
                Debug.Log("enemy(critical) | player(hurt) | energy(low) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Medium:
                Debug.Log("enemy(critical) | player(hurt) | energy(low) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Hurt && energy == Energy.Low && distance == Distance.Close:
                Debug.Log("enemy(critical) | player(hurt) | energy(low) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Far:
                Debug.Log("enemy(critical) | player(critical) | energy(full) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Medium:
                Debug.Log("enemy(critical) | player(critical) | energy(full) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.High && distance == Distance.Close:
                Debug.Log("enemy(critical) | player(critical) | energy(full) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Far:
                Debug.Log("enemy(critical) | player(critical) | energy(medium) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Medium:
                Debug.Log("enemy(critical) | player(critical) | energy(medium) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.Medium && distance == Distance.Close:
                Debug.Log("enemy(critical) | player(critical) | energy(medium) | distance(close)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Far:
                Debug.Log("enemy(critical) | player(critical) | energy(low) | distance(far)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Medium:
                Debug.Log("enemy(critical) | player(critical) | energy(low) | distance(middle)");
                break;
            case EnemyHealth state when state == EnemyHealth.Critical && playerHealth == PlayerHealth.Critical && energy == Energy.Low && distance == Distance.Close:
                Debug.Log("enemy(critical) | player(critical) | energy(low) | distance(close)");
                break;

            default:
                Debug.Log("geen case matched!! ||  eHPState " + enemyHealthState + " pHPState " + playerHealthState + " enState " + energyState + " dState " + distanceState);
                break;
        }
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