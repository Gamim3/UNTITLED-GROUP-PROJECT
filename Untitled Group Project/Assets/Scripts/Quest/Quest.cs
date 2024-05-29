using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest")]
public class Quest : ScriptableObject
{
    [Header("UI")]
    [Tooltip("Quest Type Is Used To Determine Which Type To Check")]
    public QuestType questType;
    [Tooltip("Title Of Quest, As Displayed In The Quest UI")]
    public string questName;
    [Tooltip("Description Of Quest, As Displayed In The Quest UI")]
    public string questDescription;

    [Tooltip("Set This As A Unique Id, Preferebaly In Order Of Quests (NOT 0)")]
    public int questId;

    [Header("Options")]
    [Tooltip("Amount Of Items, Kills Or Hits Needed To Complete Quest")]
    public int questGoalAmount;

    [Tooltip("Leave Empty If Not A Collect Quest")]
    public ItemInfo itemToCollect;
    [Tooltip("Any Will Complete The Quest For Any Monster. Select A Specific Monster To Only Listen For That Monster")]
    public Enemy.EnemyType enemyToHunt;

    [Header("Reward")]
    public int xpReward;
    [Tooltip("Leave Empty If No Reward Should Be Granted")]
    public Recipe recipeToUnlock;
    [Tooltip("Leave Empty If No Reward Should Be Granted")]
    public ItemInfo itemToGet;
}

[System.Serializable]
public enum QuestType
{
    COLLECT,
    KILL,
    HIT
}
