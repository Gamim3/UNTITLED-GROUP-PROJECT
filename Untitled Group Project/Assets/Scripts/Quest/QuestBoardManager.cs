using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestBoardManager : MonoBehaviour
{
    [SerializeField] GameObject[] _questPrefabs;
    [SerializeField] Transform[] _boardSlots;

    [SerializeField] List<Quest> _quests;
    [SerializeField] Camera _boardCam;
    public Camera BoardCam { get { return _boardCam; } }

    QuestManager _questManager;
    // Start is called before the first frame update
    void Start()
    {
        _questManager = GetComponent<QuestManager>();
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
        GameObject _questBoardItem = Instantiate(_questPrefabs[Random.Range(0, _questPrefabs.Length)], _boardSlots[0]);
        _questBoardItem.GetComponent<QuestBoardItem>().InitiateBoardItem(quest);
    }
}
