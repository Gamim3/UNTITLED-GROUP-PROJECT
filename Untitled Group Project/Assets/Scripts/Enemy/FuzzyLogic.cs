using System;
using UnityEditor;
using UnityEngine;

public class FuzzyLogic : MonoBehaviour
{
    [Header("FuzzyStats")]
    public Vector3 fuzzyEnemyHealth;
    public Vector3 fuzzyPlayerHealth;
    public Vector3 fuzzyEnergy;
    public Vector3 fuzzyDistance;

    [Header("Inputs")]
    public float enemyHealth;
    public float playerHealth;
    public float energy;
    public float distance;

    [Header("FuzzyficationSettings")]
    [SerializeField] int fuzzyValueDecimals;
    [NonSerialized] string decimalText;

    [Header("FuzzyLogicSets")]

    [Header("EnemyHealthSet")]
    [SerializeField] float criticalHealthLimit;

    [SerializeField] float minHurtValue;
    [SerializeField] float fullHurt;
    [SerializeField] float maxHurtValue;

    [SerializeField] float beginHealthyValue;

    [SerializeField] AnimationCurve criticalCurve;
    [SerializeField] AnimationCurve hurtCurve;
    [SerializeField] AnimationCurve healthyCurve;


    [Header("PlayerHealthSet")]
    [SerializeField] float playerCriticalHealthLimit;

    [SerializeField] float playerMinHurtValue;
    [SerializeField] float playerFullHurt;
    [SerializeField] float playerMaxHurtValue;

    [SerializeField] float playerBeginHealthyValue;

    [SerializeField] AnimationCurve playerCriticalCurve;
    [SerializeField] AnimationCurve playerHurtCurve;
    [SerializeField] AnimationCurve playerHealthyCurve;

    [Header("DistanceSet")]
    [SerializeField] float nearDistanceLimit;

    [SerializeField] float minDistance;
    [SerializeField] float midMiddleDistance;
    [SerializeField] float maxMiddleDistance;

    [SerializeField] float beginFarDistance;

    [SerializeField] AnimationCurve nearCurve;
    [SerializeField] AnimationCurve middleCurve;
    [SerializeField] AnimationCurve farCurve;

    [Header("EnergySet")]
    [SerializeField] float lowEnergyLimit;

    [SerializeField] float minMiddleEnergy;
    [SerializeField] float midMiddleEnergy;
    [SerializeField] float maxMiddleEnergy;

    [SerializeField] float beginfullEnergy;

    [SerializeField] AnimationCurve lowEnergyCurve;
    [SerializeField] AnimationCurve mediumEnergyCurve;
    [SerializeField] AnimationCurve fullEnergyCurve;

    public void Start()
    {
        decimalText = "1";

        for (int i = 0; i < fuzzyValueDecimals; i++)
        {
            decimalText += "0";
        }

        int.TryParse(decimalText, out fuzzyValueDecimals);

        //SetCurves();
    }

    public void Update()
    {
        fuzzyEnemyHealth = new Vector3(Mathf.Round(criticalCurve.Evaluate(enemyHealth) * fuzzyValueDecimals) / fuzzyValueDecimals, Mathf.Round(hurtCurve.Evaluate(enemyHealth) * fuzzyValueDecimals) / fuzzyValueDecimals, Mathf.Round(healthyCurve.Evaluate(enemyHealth) * fuzzyValueDecimals) / fuzzyValueDecimals);

        fuzzyPlayerHealth = new Vector3(Mathf.Round(playerCriticalCurve.Evaluate(playerHealth) * fuzzyValueDecimals) / fuzzyValueDecimals, Mathf.Round(playerHurtCurve.Evaluate(playerHealth) * fuzzyValueDecimals) / fuzzyValueDecimals, Mathf.Round(playerHealthyCurve.Evaluate(playerHealth) * fuzzyValueDecimals) / fuzzyValueDecimals);

        fuzzyDistance = new Vector3(Mathf.Round(nearCurve.Evaluate(distance) * fuzzyValueDecimals) / fuzzyValueDecimals, Mathf.Round(middleCurve.Evaluate(distance) * fuzzyValueDecimals) / fuzzyValueDecimals, Mathf.Round(farCurve.Evaluate(distance) * fuzzyValueDecimals) / fuzzyValueDecimals);

        fuzzyEnergy = new Vector3(Mathf.Round(lowEnergyCurve.Evaluate(energy) * fuzzyValueDecimals) / fuzzyValueDecimals, Mathf.Round(mediumEnergyCurve.Evaluate(energy) * fuzzyValueDecimals) / fuzzyValueDecimals, Mathf.Round(fullEnergyCurve.Evaluate(energy) * fuzzyValueDecimals) / fuzzyValueDecimals);
    }

#if UNITY_EDITOR
    public void SetCurves()
    {
        //Critical Enemy Curve
        criticalCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(criticalHealthLimit, 0));

        for (int i = 0; i < criticalCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(criticalCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(criticalCurve, i, AnimationUtility.TangentMode.Linear);
        }
        criticalCurve.preWrapMode = WrapMode.Clamp;
        criticalCurve.postWrapMode = WrapMode.Clamp;

        //hurt Enemy Curve
        hurtCurve = new AnimationCurve(new Keyframe(minHurtValue, 0), new Keyframe(fullHurt, 1), new Keyframe(maxHurtValue, 0));

        for (int i = 0; i < hurtCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(hurtCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(hurtCurve, i, AnimationUtility.TangentMode.Linear);
        }
        hurtCurve.preWrapMode = WrapMode.Clamp;
        hurtCurve.postWrapMode = WrapMode.Clamp;

        //Healthy Enemy Curve
        healthyCurve = new AnimationCurve(new Keyframe(beginHealthyValue, 0), new Keyframe(100, 1));

        for (int i = 0; i < healthyCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(healthyCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(healthyCurve, i, AnimationUtility.TangentMode.Linear);
        }
        healthyCurve.preWrapMode = WrapMode.Clamp;
        healthyCurve.postWrapMode = WrapMode.Clamp;


        //Critical player Curve
        playerCriticalCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(playerCriticalHealthLimit, 0));

        for (int i = 0; i < playerCriticalCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(playerCriticalCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(playerCriticalCurve, i, AnimationUtility.TangentMode.Linear);
        }
        playerCriticalCurve.preWrapMode = WrapMode.Clamp;
        playerCriticalCurve.postWrapMode = WrapMode.Clamp;

        //hurt player Curve
        playerHurtCurve = new AnimationCurve(new Keyframe(playerMinHurtValue, 0), new Keyframe(playerFullHurt, 1), new Keyframe(playerMaxHurtValue, 0));

        for (int i = 0; i < playerHurtCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(playerHurtCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(playerHurtCurve, i, AnimationUtility.TangentMode.Linear);
        }
        playerHurtCurve.preWrapMode = WrapMode.Clamp;
        playerHurtCurve.postWrapMode = WrapMode.Clamp;

        //Healthy player Curve
        playerHealthyCurve = new AnimationCurve(new Keyframe(playerBeginHealthyValue, 0), new Keyframe(100, 1));

        for (int i = 0; i < playerHealthyCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(playerHealthyCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(playerHealthyCurve, i, AnimationUtility.TangentMode.Linear);
        }
        playerHealthyCurve.preWrapMode = WrapMode.Clamp;
        playerHealthyCurve.postWrapMode = WrapMode.Clamp;

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
        middleCurve = new AnimationCurve(new Keyframe(minDistance, 0), new Keyframe(midMiddleDistance, 1), new Keyframe(maxMiddleDistance, 0));

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

        //low energy Curve
        lowEnergyCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(lowEnergyLimit, 0));

        for (int i = 0; i < lowEnergyCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(lowEnergyCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(lowEnergyCurve, i, AnimationUtility.TangentMode.Linear);
        }
        lowEnergyCurve.preWrapMode = WrapMode.Clamp;
        lowEnergyCurve.postWrapMode = WrapMode.Clamp;

        //medium energy Curve
        mediumEnergyCurve = new AnimationCurve(new Keyframe(minMiddleEnergy, 0), new Keyframe(midMiddleEnergy, 1), new Keyframe(maxMiddleEnergy, 0));

        for (int i = 0; i < mediumEnergyCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(mediumEnergyCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(mediumEnergyCurve, i, AnimationUtility.TangentMode.Linear);
        }
        mediumEnergyCurve.preWrapMode = WrapMode.Clamp;
        mediumEnergyCurve.postWrapMode = WrapMode.Clamp;

        //full energy Curve
        fullEnergyCurve = new AnimationCurve(new Keyframe(beginfullEnergy, 0), new Keyframe(100, 1));

        for (int i = 0; i < fullEnergyCurve.length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(fullEnergyCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(fullEnergyCurve, i, AnimationUtility.TangentMode.Linear);
        }
        fullEnergyCurve.preWrapMode = WrapMode.Clamp;
        fullEnergyCurve.postWrapMode = WrapMode.Clamp;
    }
#endif
}