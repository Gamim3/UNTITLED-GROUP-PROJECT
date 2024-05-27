using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestBoardManager : MonoBehaviour
{
    [SerializeField] GameObject[] _questPrefabs;
    [SerializeField] Transform[] _boardSlots;

    [SerializeField] List<Quest> _quests;


    QuestManager _questManager;
    // Start is called before the first frame update
    void Start()
    {
        _questManager = GetComponent<QuestManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddQuest(Quest quest)
    {
        _quests.Add(quest);
        for (int i = 0; i < _boardSlots.Length; i++)
        {
            if (_boardSlots[i].childCount == 0)
            {
                GameObject _questboardItem = Instantiate(_questPrefabs[Random.Range(0, _questPrefabs.Length)], _boardSlots[i]);
                _questboardItem.GetComponent<QuestBoardItem>().InitiateBoardItem(quest);
                return;
            }
        }
        Destroy(_boardSlots[0].GetChild(0).gameObject);
        Instantiate(_questPrefabs[Random.Range(0, _questPrefabs.Length)], _boardSlots[0]);
    }
}
