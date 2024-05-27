using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] Quest[] _allQuests;
    [SerializeField] int _questIndex;

    public Quest activeQuest;

    PlayerStats _playerStats;

    public int currentCompletionAmount;


    // Start is called before the first frame update
    void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    private void OnEnable()
    {
        //Subscribe To Kill Event
        //Subscribe To Hit Event
        InventoryManager.Instance.OnItemRecieved += TypeCheck;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeQuest != null)
        {
            if (CheckForCompletion())
            {
                StartNewQuest();
            }
            else if (activeQuest.completed)
            {
                StartNewQuest();
            }
        }

    }

    public bool CheckForCompletion()
    {
        if (currentCompletionAmount >= activeQuest.questGoalAmount)
        {
            return true;
        }
        return false;
    }

    public void AddQuestAmount(QuestType questType)
    {
        if (activeQuest.questType == questType)
        {
            currentCompletionAmount++;
        }
    }

    void TypeCheck()
    {
        AddQuestAmount(activeQuest.questType);
    }

    void StartNewQuest()
    {
        _playerStats.AddXp(activeQuest.xpReward);

        if (activeQuest.itemToGet.item != null)
        {
            InventoryManager.Instance.AddItem(activeQuest.itemToGet.item.itemID, activeQuest.itemToGet.amount);
        }
        if (activeQuest.recipeToUnlock != null)
        {
            CraftingManager.Instance.AddRecipe(activeQuest.recipeToUnlock);
        }

        activeQuest.completed = true;

        activeQuest = activeQuest.nextQuest;
        _questIndex = activeQuest.questId;
        currentCompletionAmount = 0;
    }

    public void LoadData(GameData data)
    {
        _questIndex = data.questIndex;
    }

    public void SaveData(GameData data)
    {
        data.questIndex = _questIndex;
    }
}
