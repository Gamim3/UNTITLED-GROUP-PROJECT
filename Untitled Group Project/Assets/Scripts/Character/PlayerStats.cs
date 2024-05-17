using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDataPersistence
{
    public int xp;
    public int level;

    public int xpGoal;

    [Header("Settings")]
    [SerializeField] int tutorialXpGoal = 100;
    [SerializeField] int initialXpGoal = 1000;
    public float xpGoalIncrement = 1.5f;
    [SerializeField] Recipe[] _recipeToUnlock;

    void Update()
    {
        if (level == 0)
        {
            if (xp >= tutorialXpGoal)
            {
                LevelUp();
            }
        }
        else if (xp >= xpGoal)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        if (level == 0)
        {
            xpGoal = initialXpGoal;
        }
        else
        {
            xpGoal = (int)(xpGoal * xpGoalIncrement);
        }
        if (_recipeToUnlock.Length >= level && _recipeToUnlock[level] != null)
        {
            CraftingManager.Instance.AddRecipe(_recipeToUnlock[level]);
        }
        xp = 0;
        level++;
    }

    public void LoadData(GameData data)
    {
        xp = data.xp;
        level = data.level;

        if (data.xpGoal == 0)
        {
            xpGoal = tutorialXpGoal;
        }
        else
        {
            xpGoal = data.xpGoal;
        }
    }

    public void SaveData(GameData data)
    {
        data.xp = xp;
        data.xpGoal = xpGoal;
        data.level = level;
    }
}