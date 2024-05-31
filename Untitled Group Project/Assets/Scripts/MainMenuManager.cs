using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class MainMenuManager : MonoBehaviour
{
    [Header("CurrentSave")]
    #region CurrentSave

    [SerializeField] string _currentSaveFileName;
    [SerializeField] string _currentSaveDataName;

    #endregion

    [Header("ScenesToLoad")]
    #region ScenesToLoad

    [SerializeField] string _gameScene;
    [SerializeField] string _mainMenuScene;

    #endregion

    [Header("Menu Navigation")]
    #region Menu Navigation

    #endregion

    [Header("Continue")]
    #region Continue

    [SerializeField] Button _continueButton;

    #endregion

    [Header("New Game")]
    #region New Game

    [SerializeField] GameObject _newGamePanel;
    [SerializeField] GameObject _firstNewGameButton;

    [SerializeField] TMP_InputField _newGameInputField;


    #endregion

    [Header("Load Game")]
    #region Save File

    [SerializeField] GameObject _loadGamePanel;
    [SerializeField] GameObject _firstLoadButton;

    [SerializeField] List<GameData> _gameData = new();
    [SerializeField] List<SaveFile> _saveFiles = new();

    [SerializeField] Transform _saveFileParent;
    [SerializeField] GameObject _saveFilePrefab;

    [SerializeField] SaveFile _saveFileToDelete;

    [SerializeField] GameObject _confirmSaveFileDeletePanel;
    [SerializeField] GameObject _firstConfirmSaveFileDeleteButton;

    #endregion

    [Header("Options")]
    #region Options

    [SerializeField] GameObject _optionsPanel;
    [SerializeField] GameObject _firstOptionsButton;

    [Header("Audio")]
    [SerializeField] GameObject _audioPanel;
    [SerializeField] GameObject _firstAudioButton;

    [Header("Video")]
    [SerializeField] GameObject _videoPanel;
    [SerializeField] GameObject _firstVideoButton;

    [Header("Controls")]
    [SerializeField] GameObject _controlsPanel;
    [SerializeField] GameObject _firstControlsButton;

    #endregion

    [Header("Quit")]
    #region Quit

    [SerializeField] GameObject _quitPanel;

    #endregion


    [SerializeField] Animator _optionsAnimator;
    [SerializeField] Animator _loadAnimator;
    [SerializeField] Animator _newGameAnimator;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == _gameScene)
        {
            // FindObjectOfType<GameMenuManager>().SetCurrentSaveFileAndData(_currentSaveFileName, _currentSaveDataName);

            DataPersistenceManager.instance.LoadGame(_currentSaveFileName, _currentSaveDataName);
        }
    }

    private void Awake()
    {
        // TODO - maybe use this idk yet
        // DontDestroyOnLoad(FindObjectOfType<EventSystem>().gameObject);
        // DontDestroyOnLoad(FindObjectOfType<Camera>().gameObject);
    }

    private void Start()
    {
        RefreshSaveFiles();

        // TODO - more checks to see if this works ( seems promosing )
        SceneManager.LoadSceneAsync(_gameScene, LoadSceneMode.Additive);
    }

    public void RefreshSaveFiles()
    {
        _gameData.Clear();

        for (int i = 0; i < _saveFiles.Count; i++)
        {
            Destroy(_saveFiles[i].gameObject);
        }

        _saveFiles.Clear();

        GetAllSaveFiles();

        for (int i = 0; i < _saveFiles.Count; i++)
        {
            int index = i;
            _saveFiles[index].SaveFileButton.onClick.AddListener(delegate { LoadSaveFile(_saveFiles[index]); });
            _saveFiles[index].DeleteFileButton.onClick.AddListener(delegate { DeleteSaveFileButton(index); });
        }

        _firstLoadButton = _saveFiles[0].gameObject;
    }

    public void NewGame()
    {
        DataPersistenceManager.instance.ChangeSelectedSaveFile(_currentSaveFileName);

        // TODO - this works for now maybe find better way later
        _currentSaveDataName = "data.game";
        DataPersistenceManager.instance.ChangeSelectedSaveData(_currentSaveDataName);

        if (_currentSaveFileName == "")
        {
            Debug.LogWarning("Save file name can not be nothing");
            return;
        }

        if (!DataPersistenceManager.instance.CheckIfSelectedSaveFileExists(_currentSaveFileName))
        {
            DataPersistenceManager.instance.NewGame();
            RefreshSaveFiles();
        }
        else
        {
            Debug.LogWarning($"{_currentSaveFileName} already exists not creating a new game");
            return;
        }

        FindObjectOfType<GameMenuManager>().SetCurrentSaveFileAndData(_currentSaveFileName, _currentSaveDataName);

        DataPersistenceManager.instance.LoadGame(_currentSaveFileName, _currentSaveDataName);
    }

    public void ContinueBtn()
    {
        // DataPersistenceManager.instance.ChangeSelectedSaveFile(_saveSlots[0].SaveFileName);
        // DataPersistenceManager.instance.ChangeSelectedSaveData(_saveSlots[0].SaveDataName);
        // SceneManager.LoadScene(_gameScene);

        SceneManager.UnloadSceneAsync(_mainMenuScene);
        // LoadSaveFile(_saveSlots[0]);
    }

    public void OptionsBtn()
    {
        _optionsAnimator.SetTrigger("Animate");
    }

    public void LoadBtn()
    {
        _loadAnimator.SetTrigger("Animate");
    }

    public void NewGameBtn()
    {
        _newGameAnimator.SetTrigger("Animate");
    }

    public void SaveFileNameInputField()
    {
        _currentSaveFileName = _newGameInputField.text;
    }

    // TODO - more checks to see if this works ( seems promosing )
    public void LoadSaveFile(SaveFile saveSlot)
    {
        DataPersistenceManager.instance.ChangeSelectedSaveFile(saveSlot.SaveFileName);
        DataPersistenceManager.instance.ChangeSelectedSaveData(saveSlot.SaveDataName);

        _currentSaveFileName = saveSlot.SaveFileName;
        _currentSaveDataName = saveSlot.SaveDataName;

        DataPersistenceManager.instance.LoadGame(saveSlot.SaveFileName, saveSlot.SaveDataName);

        FindObjectOfType<GameMenuManager>().SetCurrentSaveFileAndData(_currentSaveFileName, _currentSaveDataName);

        SceneManager.UnloadSceneAsync(_mainMenuScene);
    }

    public void DeleteSaveFileButton(int index)
    {
        Debug.Log("Delete");
        // TODO - make this work if a save file has already been deleted
        DataPersistenceManager.instance.DeleteSelectedSaveFile(_saveFiles[index].SaveFileName);

        SaveFile saveSlotToDelete = _saveFiles[index];

        _gameData.RemoveAt(index);
        _saveFiles.RemoveAt(index);

        Destroy(saveSlotToDelete.gameObject);

        RefreshSaveFiles();
    }

    public void GetAllSaveFiles()
    {
        _gameData = DataPersistenceManager.instance.GetNewestSaveFiles();

        if (_gameData.Count == 0)
        {
            _continueButton.interactable = false;
            return;
        }
        else
        {
            _continueButton.interactable = true;
        }

        for (int i = 0; i < _gameData.Count; i++)
        {
            _saveFiles.Add(Instantiate(_saveFilePrefab, _saveFileParent).GetComponent<SaveFile>());
            _saveFiles[i].SetData(_gameData[i]);
        }

        DataPersistenceManager.instance.ChangeSelectedSaveFile(_saveFiles[0].SaveFileName);
        DataPersistenceManager.instance.ChangeSelectedSaveData(_saveFiles[0].SaveDataName);
    }

    public void QuitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        Application.Quit();
#endif
    }
}
