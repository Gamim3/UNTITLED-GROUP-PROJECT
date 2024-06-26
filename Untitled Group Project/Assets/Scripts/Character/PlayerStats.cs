using System.Linq;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerStats : MonoBehaviour, IDataPersistence
{
    public int xp;
    public int level;
    public int xpGoal;
    public int swordId;

    [SerializeField] Item _currentWeapon;

    [Header("Settings")]
    [SerializeField] int tutorialXpGoal = 100;
    [SerializeField] int initialXpGoal = 1000;
    public float xpGoalIncrement = 1.5f;
    [SerializeField] Recipe[] _recipeToUnlock;

    [Header("Audio Effects")]
    [SerializeField] AudioSource _playerAudioSource;
    [SerializeField] AudioClip[] _hurtSounds;
    [SerializeField] AudioClip _levelUpSound;
    [SerializeField] AudioClip _enemyHitSound;

    public event XpChanged OnXpGained;

    private void Start()
    {
        if (swordId > 0)
        {
            _currentWeapon = InventoryManager.Instance.GetItemById(swordId);

            if (_currentWeapon != null)
            {
                GetComponent<CharStateMachine>().DamageMultiplier = _currentWeapon.upgrade.upgradeMultiplier;
            }
        }

        for (int i = 0; i < level; i++)
        {
            if (_recipeToUnlock.Length > level && _recipeToUnlock[i] != null)
            {
                CraftingManager.Instance.AddRecipe(_recipeToUnlock[i]);
            }
        }
    }

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
        _playerAudioSource.clip = _levelUpSound;
        _playerAudioSource.Play();

        int extraXP = xpGoal - xp;
        xp = extraXP;

        if (level == 0)
        {
            xpGoal = initialXpGoal;
        }
        else
        {
            xpGoal = (int)(xpGoal * xpGoalIncrement);
        }
        if (_recipeToUnlock.Length > level && _recipeToUnlock[level] != null)
        {
            CraftingManager.Instance.AddRecipe(_recipeToUnlock[level]);
        }
        level++;
    }

    public void OnPlayerDamage()
    {
        int random = Random.Range(0, _hurtSounds.Length);
        if (_hurtSounds.Any() && _hurtSounds[random] != null)
        {
            _playerAudioSource.clip = _hurtSounds[random];
            _playerAudioSource.Play();
        }
    }

    public void OnEnemyHit()
    {
        if (_enemyHitSound != null)
        {
            if (AudioManager.Instance)
            {
                AudioManager.Instance.PlaySfx(_enemyHitSound);
            }
        }
    }

    public void AddXp(int xpToGet)
    {
        xp += xpToGet;
        PlayerPrefs.SetInt("Xp", xp);
        StartCoroutine(InvokeXp(xpToGet));
    }

    IEnumerator InvokeXp(int xpToGet)
    {
        yield return new WaitForNextFrameUnit();
        OnXpGained.Invoke(xpToGet);
    }
    public delegate void XpChanged(int xpAmount);

    public void LoadData(GameData data)
    {
        Debug.Log($"Data XP Goal == {data.xpGoal}");
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

        swordId = data.swordId;
    }

    public void SaveData(GameData data)
    {
        data.xp = xp;
        data.xpGoal = xpGoal;
        data.level = level;
        data.swordId = swordId;
    }

}
