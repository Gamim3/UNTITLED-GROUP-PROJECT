using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestBoardItem : MonoBehaviour
{
    public Quest quest;

    [SerializeField] TMP_Text _questTitle;
    [SerializeField] TMP_Text _questDescription;

    [SerializeField] TMP_Text _xpReward;


    public bool completed;


    // Update is called once per frame
    void Update()
    {
        completed = quest.completed;
    }

    public void InitiateBoardItem(Quest newQuest)
    {
        quest = newQuest;
        if (quest)
        {
            _questTitle.text = quest.questName;
            _questDescription.text = quest.questDescription;

            _xpReward.text = $"XP Reward:\n{quest.xpReward}";
        }
    }
}
