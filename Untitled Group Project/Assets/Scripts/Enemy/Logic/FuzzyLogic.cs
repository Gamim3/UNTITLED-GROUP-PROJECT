using System;
using Unity.VisualScripting;
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
    [SerializeField] int _fuzzyValueDecimals;
    [NonSerialized] string _decimalText;

    [Header("FuzzyLogicSets")]

    [Header("EnemyHealthSet")]
    [SerializeField] float _criticalHealthLimit;

    [SerializeField] float _minHurtValue;
    [SerializeField] float _fullHurt;
    [SerializeField] float _maxHurtValue;

    [SerializeField] float _beginHealthyValue;

    [Header("PlayerHealthSet")]
    [SerializeField] float _playerCriticalHealthLimit;

    [SerializeField] float _playerMinHurtValue;
    [SerializeField] float _playerFullHurt;
    [SerializeField] float _playerMaxHurtValue;

    [SerializeField] float _playerBeginHealthyValue;

    [Header("DistanceSet")]
    [SerializeField] float _nearDistanceLimit;

    [SerializeField] float _minMiddleDistance;
    [SerializeField] float _midMiddleDistance;
    [SerializeField] float _maxMiddleDistance;

    [SerializeField] float _beginFarDistance;

    [Header("EnergySet")]
    [SerializeField] float _lowEnergyLimit;

    [SerializeField] float _minMiddleEnergy;
    [SerializeField] float _midMiddleEnergy;
    [SerializeField] float _maxMiddleEnergy;

    [SerializeField] float _beginfullEnergy;

    public void Start()
    {
        _decimalText = "1";

        for (int i = 0; i < _fuzzyValueDecimals; i++)
        {
            _decimalText += "0";
        }

        int.TryParse(_decimalText, out _fuzzyValueDecimals);
    }

    public void Update()
    {
        //moet nog verbeteren
        fuzzyEnemyHealth.x = GetDataFromGraph(1, _criticalHealthLimit , enemyHealth);
        fuzzyEnemyHealth.y = GetDataFromGraph(_minHurtValue, _fullHurt, _maxHurtValue, enemyHealth);
        fuzzyEnemyHealth.z = GetDataFromGraph(0, _beginHealthyValue, enemyHealth);

        fuzzyPlayerHealth.x = GetDataFromGraph(1, _playerCriticalHealthLimit, playerHealth);
        fuzzyPlayerHealth.y = GetDataFromGraph(_playerMinHurtValue, _playerFullHurt, _playerMaxHurtValue, playerHealth);
        fuzzyPlayerHealth.z = GetDataFromGraph(0, _playerBeginHealthyValue, playerHealth);

        fuzzyEnergy.x = GetDataFromGraph(1, _lowEnergyLimit, energy);
        fuzzyEnergy.y = GetDataFromGraph(_minMiddleEnergy, _midMiddleEnergy, _maxMiddleEnergy, energy);
        fuzzyEnergy.z = GetDataFromGraph(0, _beginfullEnergy, energy);

        fuzzyDistance.x = GetDataFromGraph(1, _nearDistanceLimit, distance);
        fuzzyDistance.y = GetDataFromGraph(_minMiddleDistance, _midMiddleDistance, _maxMiddleDistance, distance);
        fuzzyDistance.z = GetDataFromGraph(0, _beginFarDistance, distance);
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

            return Mathf.Round((y * _fuzzyValueDecimals)) / _fuzzyValueDecimals;
        }
        else if (evaluationPoint >= 0 && evaluationPoint >= GraphPoint && beginFloat == 0)
        {
            Vector2 pointA = new Vector2(GraphPoint, 0);
            Vector2 pointB = new Vector2(100, 1);

            float slope = (pointB.y - pointA.y) / (pointB.x - pointA.x);

            float yIntercept = pointA.y - slope * pointA.x;

            // Use the line equation y = mx + b to calculate the y value at x
            float y = slope * evaluationPoint + yIntercept;

            return Mathf.Round((y * _fuzzyValueDecimals)) / _fuzzyValueDecimals;
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

            return Mathf.Round((y * _fuzzyValueDecimals)) / _fuzzyValueDecimals;
        }
        else if (evaluationPoint >= GraphPointMiddle && evaluationPoint <= GraphPointEnd)
        {
            Vector2 pointB = new Vector2(GraphPointMiddle, 1);
            Vector2 pointA = new Vector2(GraphPointEnd, 0);

            float slope = (pointB.y - pointA.y) / (pointB.x - pointA.x);

            float yIntercept = pointA.y - slope * pointA.x;

            // Use the line equation y = mx + b to calculate the y value at x
            float y = slope * evaluationPoint + yIntercept;

            return Mathf.Round((y * _fuzzyValueDecimals)) / _fuzzyValueDecimals;
        }

            return 0;
    }
}