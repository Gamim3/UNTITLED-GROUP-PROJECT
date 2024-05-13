using UnityEditor;
using UnityEngine;

public class FuzzyLogic : MonoBehaviour
{
    [Header("FuzzyStats")]
    public Vector3 fuzzyEnemyHealth;
    public Vector3 fuzzyPlayerHealth;

    [Header("Inputs")]
    public float enemyHealth;
    public float playerHealth;
    public float distance;

    [Header("Outputs")]
    public float healthRiskPercentage;

    [Header("FuzzyficationSettings")]
    public Transform playerPosition;
    public int fuzzyValueDecimals;
    private string decimalText;

    [Header("FuzzyLogicSets")]

    [Header("HealthSet")]
    public float criticalHealthLimit;

    public float minHurtValue;
    public float fullHurt;
    public float maxHurtValue;

    public float beginHealthyValue;

    public AnimationCurve criticalCurve;
    public AnimationCurve hurtCurve;
    public AnimationCurve healthyCurve;

    [Header("DistanceSet")]
    public float nearDistanceLimit;

    public float beginFarDistance;

    public AnimationCurve nearCurve;
    public AnimationCurve middleCurve;
    public AnimationCurve farCurve;

    public void Start()
    {
        decimalText = "1";

        for (int i = 0; i < fuzzyValueDecimals; i++)
        {
            decimalText += "0";
        }

        int.TryParse(decimalText, out fuzzyValueDecimals);

        SetCurves();
    }

    public void Update()
    {
        distance = Vector3.Distance(transform.position, playerPosition.position);

        fuzzyEnemyHealth = new Vector3(Mathf.Round(criticalCurve.Evaluate(enemyHealth) * fuzzyValueDecimals) / fuzzyValueDecimals, Mathf.Round(hurtCurve.Evaluate(enemyHealth) * fuzzyValueDecimals) / fuzzyValueDecimals, Mathf.Round(healthyCurve.Evaluate(enemyHealth) * fuzzyValueDecimals) / fuzzyValueDecimals);

        fuzzyPlayerHealth = new Vector3(Mathf.Round(criticalCurve.Evaluate(playerHealth) * fuzzyValueDecimals) / fuzzyValueDecimals, Mathf.Round(hurtCurve.Evaluate(playerHealth) * fuzzyValueDecimals) / fuzzyValueDecimals, Mathf.Round(healthyCurve.Evaluate(playerHealth) * fuzzyValueDecimals) / fuzzyValueDecimals);

        Defuzzify(fuzzyEnemyHealth, fuzzyPlayerHealth);
    }

    public void Defuzzify(Vector3 EnemyFuzzyHp, Vector3 PlayerFuzzyHP)
    {
        float maxEnemyValue = Mathf.Max(EnemyFuzzyHp.x, Mathf.Max(EnemyFuzzyHp.y, EnemyFuzzyHp.z));
        float maxPlayerValue = Mathf.Max(PlayerFuzzyHP.x, Mathf.Max(PlayerFuzzyHP.y, PlayerFuzzyHP.z));

        float enemyRisk = 0;
        float playerRisk = 0;

        // Switch case based on the highest value
        switch (maxEnemyValue)
        {
            case float n when n == EnemyFuzzyHp.x:
                enemyRisk = 100;
                break;
            case float n when n == EnemyFuzzyHp.y:
                enemyRisk = 50;
                break;
            case float n when n == EnemyFuzzyHp.z:
                enemyRisk = 0;
                break;
        }

        switch (maxPlayerValue)
        {
            case float n when n == PlayerFuzzyHP.x:
                playerRisk = 100;
                break;
            case float n when n == PlayerFuzzyHP.y:
                playerRisk = 50;
                break;
            case float n when n == PlayerFuzzyHP.z:
                playerRisk = 0;
                break;
        }

        healthRiskPercentage = enemyRisk - playerRisk;
    }

    public void SetCurves()
    {
        //Critical Curve
        criticalCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(criticalHealthLimit, 0));

        for (int i = 0; i < criticalCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(criticalCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(criticalCurve, i, AnimationUtility.TangentMode.Linear);
        }
        criticalCurve.preWrapMode = WrapMode.Clamp;
        criticalCurve.postWrapMode = WrapMode.Clamp;

        //hurt Curve
        hurtCurve = new AnimationCurve(new Keyframe(minHurtValue, 0), new Keyframe(fullHurt, 1), new Keyframe(maxHurtValue, 0));

        for (int i = 0; i < hurtCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(hurtCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(hurtCurve, i, AnimationUtility.TangentMode.Linear);
        }
        hurtCurve.preWrapMode = WrapMode.Clamp;
        hurtCurve.postWrapMode = WrapMode.Clamp;

        //Healthy Curve
        healthyCurve = new AnimationCurve(new Keyframe(beginHealthyValue, 0), new Keyframe(100, 1));

        for (int i = 0; i < healthyCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(healthyCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(healthyCurve, i, AnimationUtility.TangentMode.Linear);
        }
        healthyCurve.preWrapMode = WrapMode.Clamp;
        healthyCurve.postWrapMode = WrapMode.Clamp;


        //Near Curve
        nearCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(nearDistanceLimit, 0));

        for (int i = 0; i < nearCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(nearCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(nearCurve, i, AnimationUtility.TangentMode.Linear);
        }
        nearCurve.preWrapMode = WrapMode.Clamp;
        nearCurve.postWrapMode = WrapMode.Clamp;

        //Middle Curve
        middleCurve = new AnimationCurve(new Keyframe(minHurtValue, 0), new Keyframe(fullHurt, 1), new Keyframe(maxHurtValue, 0));

        for (int i = 0; i < middleCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(middleCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(middleCurve, i, AnimationUtility.TangentMode.Linear);
        }
        middleCurve.preWrapMode = WrapMode.Clamp;
        middleCurve.postWrapMode = WrapMode.Clamp;

        //far Curve
        farCurve = new AnimationCurve(new Keyframe(beginFarDistance, 0), new Keyframe(100, 1));

        for (int i = 0; i < farCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(farCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(farCurve, i, AnimationUtility.TangentMode.Linear);
        }
        farCurve.preWrapMode = WrapMode.Clamp;
        farCurve.postWrapMode = WrapMode.Clamp;
    }
}