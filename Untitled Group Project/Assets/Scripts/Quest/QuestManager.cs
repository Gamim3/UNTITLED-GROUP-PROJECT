using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour, IDataPersistence
{
    QuestBoardManager _questBoardManager;

    [SerializeField] Quest[] _allQuests;
    [SerializeField] List<int> _activeQuestIds = new List<int>();

    [SerializeField] int _maxActiveQuests = 4;



    public List<Quest> activeQuests = new();

    PlayerStats _playerStats;

    public List<int> completionAmount = new();

    // Start is called before the first frame update
    void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
        if (_playerStats == null)
        {
            Debug.LogError("No PlayerStats In Scene!");
        }
        _questBoardManager = FindObjectOfType<QuestBoardManager>();
        if (_questBoardManager == null && SceneManager.GetActiveScene().name == "GuildHall")
        {
            Debug.LogError("No QuestBoardManager In Scene!");
        }

        for (int i = 0; i < _activeQuestIds.Count; i++)
        {
            activeQuests.Add(GetQuestById(_activeQuestIds[i]));
            AddQuestBoardItem(activeQuests[i]);
        }
    }

    private void OnEnable()
    {
        //Subscribe To Hit Event
        if (InventoryManager.Instance)
            InventoryManager.Instance.OnItemRecieved += OnItemRecieved;
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance)
            InventoryManager.Instance.OnItemRecieved -= OnItemRecieved;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeQuests.Count == 0)
        {
            activeQuests.Add(GetRandomQuest());
        }

        // for (int i = 0; i < activeQuests.Count; i++)
        // {
        //     if (CheckForCompletion(activeQuests[i].questId))
        //     {
        //          CompleteQuest(activeQuests[i].questId);
        //     }
        // }
    }

    public bool CheckForCompletion(int id)
    {
        Quest quest = GetQuestById(id);

        if (quest == null)
        {
            Debug.LogError($"No Quest Found With Id Of {id}");
            return false;
        }

        for (int i = activeQuests.Count; i > 0; i--)
        {
            if (activeQuests[i - 1] == quest)
            {
                if (i - 1 < completionAmount.Count)
                {
                    if (completionAmount[i - 1] >= quest.questGoalAmount)
                    {
                        return true;
                    }
                }
                else
                {
                    continue;
                }
            }
        }
        return false;
    }

    public void AddQuestAmount(QuestType questType)
    {
        for (int i = 0; i < activeQuests.Count; i++)
        {
            if (activeQuests[i].questType == questType)
            {
                completionAmount[i]++;
            }
        }
    }

    public void DebugAddQuestAmount()
    {
        completionAmount[0]++;
    }

    public void TypeCheck(int id)
    {
        Quest quest = GetQuestById(id);

        AddQuestAmount(quest.questType);
    }

    void OnItemRecieved()
    {
        for (int i = 0; i < activeQuests.Count; i++)
        {
            if (activeQuests[i].questType == QuestType.COLLECT)
            {
                if (InventoryCheck(activeQuests[i].questId))
                {
                    completionAmount[i] = activeQuests[i].itemToCollect.amount;
                }
            }
        }
    }

    public bool InventoryCheck(int id)
    {
        Quest quest = GetQuestById(id);

        int itemAmount = InventoryManager.Instance.GetTotalItemAmount(quest.itemToCollect.item);
        if (itemAmount >= quest.questGoalAmount)
        {
            return true;
        }

        return false;
    }

    public void CompleteQuest(int id)
    {
        Quest questToComplete = GetQuestById(id);
        if (questToComplete == null)
        {
            Debug.LogError($"COULD NOT COMPLETE QUEST OF ID {id} BECAUSE IT'S ID DOES NOT EXIST OR = 0");
        }
        _playerStats.AddXp(questToComplete.xpReward);

        if (questToComplete.itemToGet.item != null)
        {
            InventoryManager.Instance.AddItem(questToComplete.itemToGet.item.itemID, questToComplete.itemToGet.amount);
        }
        if (questToComplete.recipeToUnlock != null)
        {
            CraftingManager.Instance.AddRecipe(questToComplete.recipeToUnlock);
        }

        for (int i = activeQuests.Count; i > 0; i--)
        {
            if (activeQuests[i - 1] == questToComplete)
            {
                completionAmount.RemoveAt(i - 1);
                Debug.Log("Removed One Completion Amount");
            }
        }
        activeQuests.Remove(questToComplete);
        _activeQuestIds.Remove(questToComplete.questId);


        int randomQuestAmount = Random.Range(1, 5 - activeQuests.Count);
        Debug.Log($"Random Quest Amount: " + randomQuestAmount);

        for (int i = 0; i < randomQuestAmount; i++)
        {
            activeQuests.Add(GetRandomQuest());
        }

    }

    Quest GetRandomQuest()
    {
        if (activeQuests.Count == _maxActiveQuests)
        {
            Debug.Log("Reached Maximum Quest Amount");
            return null;
        }
        Quest randomQuest = _allQuests[Random.Range(0, _allQuests.Length)];


        AddQuestBoardItem(randomQuest);
        completionAmount.Add(0);
        _activeQuestIds.Add(randomQuest.questId);
        return randomQuest;
    }

    void AddQuestBoardItem(Quest quest)
    {
        if (_questBoardManager != null)
            _questBoardManager.AddQuest(quest);
    }

    public Quest GetQuestById(int id)
    {
        if (id == 0)
        {
            Debug.LogWarning($"Id was 0, Set Quests Id In Editor");
            return null;
        }

        Quest quest = _allQuests[0];

        for (int i = 0; i < _allQuests.Length; i++)
        {
            if (_allQuests[i].questId == id)
            {
                quest = _allQuests[i];
                break;
            }
        }

        if (quest == null)
        {
            Debug.LogError($"Invalid Quest Id {id}, Id Does Not Exist");
        }

        return quest;
    }

    public void LoadData(GameData data)
    {
        Debug.Log("LOADING QUESTMANAGER");
        for (int i = 0; i < data.questIds.Length; i++)
        {
            if (data.questIds[i] > 0)
            {
                _activeQuestIds.Add(data.questIds[i]);
                completionAmount.Add(data.completionAmounts[i]);
            }
        }
    }

    public void SaveData(GameData data)
    {
        Debug.Log("SAVING QUESTMANAGER");

        data.questIds = _activeQuestIds.ToArray();
        data.completionAmounts = completionAmount.ToArray();
    }
}
