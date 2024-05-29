
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

    [Header("Player")]
    public int level;
    public int xp;
    public int xpGoal;

    [Header("Quest")]
    public int[] questIds;
    public int[] completionAmounts;

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

        xp = 0;
        xpGoal = 0;
        level = 0;

        questIds = new int[4];
        completionAmounts = new int[4];

        for (int i = 0; i < questIds.Length; i++)
        {
            questIds[i] = 0;
            completionAmounts[i] = 0;
        }
    }
}
