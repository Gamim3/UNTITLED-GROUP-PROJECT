using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour, IDataPersistence
{

    public void LoadData(GameData data)
    {
        this.gameObject.transform.position = data.cubePos;
    }

    public void SaveData(GameData data)
    {
        Debug.Log($"{data.saveDataName}_{data.saveFileName}");
        data.cubePos = this.gameObject.transform.position;
    }
}
