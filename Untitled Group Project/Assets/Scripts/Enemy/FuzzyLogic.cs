using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FuzzyLogic : MonoBehaviour
{
    [Header("FuzzyStats")]
    public Vector3 fuzzyCustomEnemyHealth;
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
    [SerializeField] float CalculatedFloat;

    [SerializeField] float criticalHealthLimit;

    [SerializeField] float minHurtValue;
    [SerializeField] float fullHurt;
    [SerializeField] float maxHurtValue;

    [SerializeField] float beginHealthyValue;

    [Header("PlayerHealthSet")]
    [SerializeField] float playerCriticalHealthLimit;

    [SerializeField] float playerMinHurtValue;
    [SerializeField] float playerFullHurt;
    [SerializeField] float playerMaxHurtValue;

    [SerializeField] float playerBeginHealthyValue;

    [Header("DistanceSet")]
    [SerializeField] float nearDistanceLimit;

    [SerializeField] float minMiddleDistance;
    [SerializeField] float midMiddleDistance;
    [SerializeField] float maxMiddleDistance;

    [SerializeField] float beginFarDistance;

    [Header("EnergySet")]
    [SerializeField] float lowEnergyLimit;

    [SerializeField] float minMiddleEnergy;
    [SerializeField] float midMiddleEnergy;
    [SerializeField] float maxMiddleEnergy;

    [SerializeField] float beginfullEnergy;

    public void Start()
    {
        decimalText = "1";

        for (int i = 0; i < fuzzyValueDecimals; i++)
        {
            decimalText += "0";
        }

        int.TryParse(decimalText, out fuzzyValueDecimals);
    }

    public void Update()
    {
        //moet nog verbeteren
        fuzzyEnemyHealth.x = GetDataFromGraph(1, criticalHealthLimit , enemyHealth);
        fuzzyEnemyHealth.y = GetDataFromGraph(minHurtValue, fullHurt, maxHurtValue, enemyHealth);
        fuzzyEnemyHealth.z = GetDataFromGraph(0, beginHealthyValue, enemyHealth);

        fuzzyPlayerHealth.x = GetDataFromGraph(1, playerCriticalHealthLimit, playerHealth);
        fuzzyPlayerHealth.y = GetDataFromGraph(playerMinHurtValue, playerFullHurt, playerMaxHurtValue, playerHealth);
        fuzzyPlayerHealth.z = GetDataFromGraph(0, playerBeginHealthyValue, playerHealth);

        fuzzyEnergy.x = GetDataFromGraph(1, lowEnergyLimit, energy);
        fuzzyEnergy.y = GetDataFromGraph(minMiddleEnergy, midMiddleEnergy, maxMiddleEnergy, energy);
        fuzzyEnergy.z = GetDataFromGraph(0, beginfullEnergy, energy);

        fuzzyDistance.x = GetDataFromGraph(1, nearDistanceLimit, distance);
        fuzzyDistance.y = GetDataFromGraph(minMiddleDistance, midMiddleDistance, maxMiddleDistance, distance);
        fuzzyDistance.z = GetDataFromGraph(0, beginFarDistance, distance);
    }

    public float GetDataFromGraph(float beginFloat , float GraphPoint, float evaluationPoint)
    {
        if (evaluationPoint >= 0 && evaluationPoint <= GraphPoint && beginFloat == 1)
        {
            Vector2 pointA = new Vector2(0, beginFloat);
            Vector2 pointB = new Vector2(GraphPoint, 0);

            float slope = (pointB.y - pointA.y) / (pointB.x - pointA.x);

            float yIntercept = pointA.y - slope * pointA.x;

            // Use the line equation y = mx + b to calculate the y value at x
            float y = slope * evaluationPoint + yIntercept;

            return Mathf.Round((y * fuzzyValueDecimals)) / fuzzyValueDecimals;
        }
        else if (evaluationPoint >= 0 && evaluationPoint >= GraphPoint && beginFloat == 0)
        {
            Vector2 pointA = new Vector2(GraphPoint, 0);
            Vector2 pointB = new Vector2(100, 1);

            float slope = (pointB.y - pointA.y) / (pointB.x - pointA.x);

            float yIntercept = pointA.y - slope * pointA.x;

            // Use the line equation y = mx + b to calculate the y value at x
            float y = slope * evaluationPoint + yIntercept;

            return Mathf.Round((y * fuzzyValueDecimals)) / fuzzyValueDecimals;
        }

        return 0;
    }

    public float GetDataFromGraph(float GraphPointBegin, float GraphPointMiddle, float GraphPointEnd, float evaluationPoint)
    {
        if (evaluationPoint >= GraphPointBegin && evaluationPoint <= GraphPointMiddle)
        {
            Vector2 pointA = new Vector2(GraphPointBegin, 0);
            Vector2 pointB = new Vector2(GraphPointMiddle, 1);

            float slope = (pointB.y - pointA.y) / (pointB.x - pointA.x);

            float yIntercept = pointA.y - slope * pointA.x;

            // Use the line equation y = mx + b to calculate the y value at x
            float y = slope * evaluationPoint + yIntercept;

            return Mathf.Round((y * fuzzyValueDecimals)) / fuzzyValueDecimals;
        }
        else if (evaluationPoint >= GraphPointMiddle && evaluationPoint <= GraphPointEnd)
        {
            Vector2 pointB = new Vector2(GraphPointMiddle, 1);
            Vector2 pointA = new Vector2(GraphPointEnd, 0);

            float slope = (pointB.y - pointA.y) / (pointB.x - pointA.x);

            float yIntercept = pointA.y - slope * pointA.x;

            // Use the line equation y = mx + b to calculate the y value at x
            float y = slope * evaluationPoint + yIntercept;

            return Mathf.Round((y * fuzzyValueDecimals)) / fuzzyValueDecimals;
        }

            return 0;
    }
}