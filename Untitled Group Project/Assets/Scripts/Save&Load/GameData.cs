
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;

    public string saveFileName;

    public string saveDataName;

    public string saveType;

    public Vector3 cubePos;

    public int[] itemId;
    public int[] itemAmount;

    public GameData()
    {
        cubePos = Vector3.zero;

        itemId = new int[24];
        itemAmount = new int[24];

        for (int i = 0; i < itemId.Length; i++)
        {
            itemId[i] = -1;
            itemAmount[i] = 0;
        }

    }
}
