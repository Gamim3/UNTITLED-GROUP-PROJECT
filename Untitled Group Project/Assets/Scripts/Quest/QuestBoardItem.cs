using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestBoardItem : MonoBehaviour
{
    public Quest quest;

    QuestManager _questManager;

    [SerializeField] TMP_Text _questTitle;
    [SerializeField] TMP_Text _questDescription;

    [SerializeField] TMP_Text _xpReward;

    [SerializeField] Button _claimButton;

    [SerializeField] Color _completedColor = Color.yellow;

    public bool completed;


    // Update is called once per frame
    void Update()
    {
        if (_questManager)
        {
            if (!completed)
            {
                completed = _questManager.CheckForCompletion(quest.questId);
                _claimButton.interactable = false;
            }
            else
            {
                if (!_claimButton.interactable)
                {
                    _claimButton.interactable = true;
                }
                Renderer rend = transform.GetComponent<Renderer>();
                if (rend != null)
                {
                    Color color = rend.material.color;
                    if (color != null)
                        color = _completedColor;
                }
            }

        }
    }

    public void CompleteQuest()
    {
        _questManager.CompleteQuest(quest.questId);
        Destroy(gameObject);
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

        _questManager = FindObjectOfType<QuestManager>();
    }
}
