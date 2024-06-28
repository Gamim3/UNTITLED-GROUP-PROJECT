using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TpToArena : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponentInParent<PlayerStats>())
        {
            SceneFader.Instance.FadeTo("Game");
            DataPersistenceManager.instance.SaveManualGame();
        }
    }
}