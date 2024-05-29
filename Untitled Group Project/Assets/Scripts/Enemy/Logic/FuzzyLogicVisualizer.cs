using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FuzzyLogicVisualizer : MonoBehaviour
{
    [SerializeField] Color _falseColor;
    [SerializeField] Color _trueColor;

    [Header ("DataSet1")]
    [SerializeField] TextMeshProUGUI _dataSet1Name;
    [SerializeField] TextMeshProUGUI[] _dataSet1FuzzyValue;
    [SerializeField] TextMeshProUGUI[] _dataSet1States;

    [Header("DataSet2")]
    [SerializeField] TextMeshProUGUI _dataSet2Name;
    [SerializeField] TextMeshProUGUI[] _dataSet2FuzzyValue;
    [SerializeField] TextMeshProUGUI[] _dataSet2States;

    [Header("DataSet3")]
    [SerializeField] TextMeshProUGUI _dataSet3Name;
    [SerializeField] TextMeshProUGUI[] _dataSet3FuzzyValue;
    [SerializeField] TextMeshProUGUI[] _dataSet3States;

    [Header("DataSet4")]
    [SerializeField] TextMeshProUGUI _dataSet4Name;
    [SerializeField] TextMeshProUGUI[] _dataSet4FuzzyValue;
    [SerializeField] TextMeshProUGUI[] _dataSet4States;

    [SerializeField] FuzzyLogic _logic;

    public void Update()
    {
        _dataSet1FuzzyValue[0].text = $"FuzzyData\r\n{_logic.fuzzyEnemyHealth.x} | {_logic.fuzzyEnemyHealth.y} | {_logic.fuzzyEnemyHealth.z}";
        _dataSet2FuzzyValue[0].text = $"FuzzyData\r\n{_logic.fuzzyPlayerHealth.x} | {_logic.fuzzyPlayerHealth.y} | {_logic.fuzzyPlayerHealth.z}";
        _dataSet3FuzzyValue[0].text = $"FuzzyData\r\n{_logic.fuzzyEnergy.x} | {_logic.fuzzyEnergy.y} | {_logic.fuzzyEnergy.z}";
        _dataSet4FuzzyValue[0].text = $"FuzzyData\r\n{_logic.fuzzyDistance.x} | {_logic.fuzzyDistance.y} | {_logic.fuzzyDistance.z}";

        if(_logic.fuzzyEnemyHealth.x > _logic.fuzzyEnemyHealth.y && _logic.fuzzyEnemyHealth.x > _logic.fuzzyEnemyHealth.z)
        {
            _dataSet1States[0].color = _trueColor;
            _dataSet1States[1].color = _falseColor;
            _dataSet1States[2].color = _falseColor;
        }
        else if(_logic.fuzzyEnemyHealth.y > _logic.fuzzyEnemyHealth.x && _logic.fuzzyEnemyHealth.y > _logic.fuzzyEnemyHealth.z)
        {
            _dataSet1States[1].color = _trueColor;
            _dataSet1States[0].color = _falseColor;
            _dataSet1States[2].color = _falseColor;
        }
        else if(_logic.fuzzyEnemyHealth.z > _logic.fuzzyEnemyHealth.x && _logic.fuzzyEnemyHealth.z > _logic.fuzzyEnemyHealth.y)
        {
            _dataSet1States[2].color = _trueColor;
            _dataSet1States[0].color = _falseColor;
            _dataSet1States[1].color = _falseColor;
        }


        if (_logic.fuzzyPlayerHealth.x > _logic.fuzzyPlayerHealth.y && _logic.fuzzyPlayerHealth.x > _logic.fuzzyPlayerHealth.z)
        {
            _dataSet2States[0].color = _trueColor;
            _dataSet2States[1].color = _falseColor;
            _dataSet2States[2].color = _falseColor;
        }
        else if (_logic.fuzzyPlayerHealth.y > _logic.fuzzyPlayerHealth.x && _logic.fuzzyPlayerHealth.y > _logic.fuzzyPlayerHealth.z)
        {
            _dataSet2States[1].color = _trueColor;
            _dataSet2States[0].color = _falseColor;
            _dataSet2States[2].color = _falseColor;
        }
        else if (_logic.fuzzyPlayerHealth.z > _logic.fuzzyPlayerHealth.x && _logic.fuzzyPlayerHealth.z > _logic.fuzzyPlayerHealth.y)
        {
            _dataSet2States[2].color = _trueColor;
            _dataSet2States[0].color = _falseColor;
            _dataSet2States[1].color = _falseColor;
        }

        if (_logic.fuzzyEnergy.x > _logic.fuzzyEnergy.y && _logic.fuzzyEnergy.x > _logic.fuzzyEnergy.z)
        {
            _dataSet3States[0].color = _trueColor;
            _dataSet3States[1].color = _falseColor;
            _dataSet3States[2].color = _falseColor;
        }
        else if (_logic.fuzzyEnergy.y > _logic.fuzzyEnergy.x && _logic.fuzzyEnergy.y > _logic.fuzzyEnergy.z)
        {
            _dataSet3States[1].color = _trueColor;
            _dataSet3States[0].color = _falseColor;
            _dataSet3States[2].color = _falseColor;
        }
        else if (_logic.fuzzyEnergy.z > _logic.fuzzyEnergy.x && _logic.fuzzyEnergy.z > _logic.fuzzyEnergy.y)
        {
            _dataSet3States[2].color = _trueColor;
            _dataSet3States[0].color = _falseColor;
            _dataSet3States[1].color = _falseColor;
        }
        
        if (_logic.fuzzyDistance.x > _logic.fuzzyDistance.y && _logic.fuzzyDistance.x > _logic.fuzzyDistance.z)
        {
            _dataSet4States[2].color = _trueColor;
            _dataSet4States[1].color = _falseColor;
            _dataSet4States[0].color = _falseColor;
        }
        else if (_logic.fuzzyDistance.y > _logic.fuzzyDistance.x && _logic.fuzzyDistance.y > _logic.fuzzyDistance.z)
        {
            _dataSet4States[1].color = _trueColor;
            _dataSet4States[2].color = _falseColor;
            _dataSet4States[0].color = _falseColor;
        }
        else if (_logic.fuzzyDistance.z > _logic.fuzzyDistance.x && _logic.fuzzyDistance.z > _logic.fuzzyDistance.y)
        {
            _dataSet4States[0].color = _trueColor;
            _dataSet4States[2].color = _falseColor;
            _dataSet4States[1].color = _falseColor;
        }
    }

    float getMaxElement(Vector3 fuzzyData)
    {
        return Mathf.Max(Mathf.Max(fuzzyData.x, fuzzyData.y), fuzzyData.z);
    }
}
