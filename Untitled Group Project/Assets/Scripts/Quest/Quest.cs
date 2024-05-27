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

    [Tooltip("Set This As A Unique Id, Preferebaly In Order Of Quests")]
    public int questId;
    [Tooltip("The Next Quest After This Is Completed, Leave Empty If Last")]
    public Quest nextQuest;

    [Header("Options")]
    [Tooltip("Amount Of Items, Kills Or Hits Needed To Complete Quest")]
    public int questGoalAmount;

    [Tooltip("Leave Empty If Not A Collect Quest")]
    public Item itemToCollect;
    [Tooltip("Leave Empty If Any Enemy Can Complete Quest")]
    public Enemy enemyToKill;

    [Header("Reward")]
    public int xpReward;
    [Tooltip("Leave Empty If No Reward Should Be Granted")]
    public Recipe recipeToUnlock;
    [Tooltip("Leave Empty If No Reward Should Be Granted")]
    public ItemInfo itemToGet;

    public bool completed;
}

[System.Serializable]
public enum QuestType
{
    COLLECT,
    KILL,
    HIT
}
