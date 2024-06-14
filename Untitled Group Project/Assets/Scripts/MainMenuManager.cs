using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Collections;

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
    [SerializeField] TMP_Text _createGameStatusTxt;


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

    [Header("General")]
    [SerializeField] Slider _sensSlider;
    [SerializeField] Slider _distanceSlider;
    [SerializeField] Toggle _sprintToggle;

    [SerializeField] TMP_Text _sensTxt;
    [SerializeField] TMP_Text _distanceTxt;

    [Header("Audio")]
    [SerializeField] GameObject _audioPanel;
    [SerializeField] GameObject _firstAudioButton;

    [SerializeField] AudioSource _buttonPopAudio;
    [SerializeField] AudioMixer _mainMixer;

    [SerializeField] Slider _masterSlider;
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _sfxSlider;

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
        DataPersistenceManager.instance.couldNotSaveEvent += CouldNotSave;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        DataPersistenceManager.instance.couldNotSaveEvent -= CouldNotSave;
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // DontDestroyOnLoad(FindObjectOfType<Camera>().gameObject);
    }

    private void Start()
    {
        GetComponent<AudioSource>().volume = 0;
        RefreshSaveFiles();

        if (_createGameStatusTxt != null)
            _createGameStatusTxt.text = "";

        // TODO - more checks to see if this works ( seems promosing )

        // SceneManager.LoadSceneAsync(_gameScene, LoadSceneMode.Additive);

        _masterSlider.value = PlayerPrefs.GetFloat("MasterVol");
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVol");

        SetMasterVol(_masterSlider.value);
        SetMusicVol(_musicSlider.value);
        SetSfxVol(_sfxSlider.value);

        if (PlayerPrefs.HasKey("MouseSensitivity"))
        {
            _sensSlider.value = PlayerPrefs.GetFloat("MouseSensitivity");
            SetMouseSensitivity(_sensSlider.value);
        }
        else
        {
            _sensSlider.value = 1;
            SetMouseSensitivity(1);
        }

        if (PlayerPrefs.HasKey("CamDistance"))
        {
            _distanceSlider.value = PlayerPrefs.GetFloat("CamDistance");
            SetCamDistance(_distanceSlider.value);
        }
        else
        {
            _distanceSlider.value = 4;
            SetCamDistance(4);
        }
        if (PlayerPrefs.HasKey("ToggleRun"))
        {
            SetToggleSprint(PlayerPrefs.GetInt("ToggleRun") == 1);
            _sprintToggle.isOn = PlayerPrefs.GetInt("ToggleRun") == 1;
        }
        else
        {
            SetToggleSprint(false);
            _sprintToggle.isOn = false;
        }
        StartCoroutine(EnableAudio());
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

        if (_saveFiles.Count > 0)
            _firstLoadButton = _saveFiles[0].gameObject;
    }

    public void NewGame()
    {
        SaveFileNameInputField();

        DataPersistenceManager.instance.ChangeSelectedSaveFile(_currentSaveFileName);

        // TODO - this works for now maybe find better way later
        _currentSaveDataName = "data.game";
        DataPersistenceManager.instance.ChangeSelectedSaveData(_currentSaveDataName);

        if (_currentSaveFileName == "")
        {
            Debug.LogWarning("Save file name can not be nothing");
            if (_createGameStatusTxt != null)
                _createGameStatusTxt.text = "Save file name can not be nothing!";
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
            if (_createGameStatusTxt != null)
                _createGameStatusTxt.text = $"File with name {_currentSaveFileName} already exists!";
            return;
        }

        if (_createGameStatusTxt != null)
            _createGameStatusTxt.text = $"Save File Added!";

        // FindObjectOfType<GameMenuManager>().SetCurrentSaveFileAndData(_currentSaveFileName, _currentSaveDataName);

        DataPersistenceManager.instance.LoadGame(_currentSaveFileName, _currentSaveDataName);
    }

    public void ContinueBtn()
    {
        // DataPersistenceManager.instance.ChangeSelectedSaveFile(_saveSlots[0].SaveFileName);
        // DataPersistenceManager.instance.ChangeSelectedSaveData(_saveSlots[0].SaveDataName);
        // SceneManager.LoadScene(_gameScene);

        SceneFader.Instance.FadeTo(_gameScene);
        // SceneManager.UnloadSceneAsync(_mainMenuScene);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // LoadSaveFile(_saveSlots[0]);
    }

    void CouldNotSave()
    {
        _createGameStatusTxt.text = $"An Error Occured Whilst Creating Save File With Name{_currentSaveFileName}";
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

        // FindObjectOfType<GameMenuManager>().SetCurrentSaveFileAndData(_currentSaveFileName, _currentSaveDataName);

        SceneFader.Instance.FadeTo(_gameScene);
        // SceneManager.UnloadSceneAsync(_mainMenuScene);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void SetMasterVol(float vol)
    {
        _mainMixer.SetFloat("MasterVol", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("MasterVol", vol);
        _buttonPopAudio.Play();
    }
    public void SetMusicVol(float vol)
    {
        _mainMixer.SetFloat("MusicVol", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("MusicVol", vol);
        _buttonPopAudio.Play();
    }
    public void SetSfxVol(float vol)
    {
        _mainMixer.SetFloat("SFXVol", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("SFXVol", vol);
        _buttonPopAudio.Play();
    }

    public void SetMouseSensitivity(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        _sensTxt.text = value.ToString("0.##");
        _buttonPopAudio.Play();
    }

    public void SetCamDistance(float value)
    {
        PlayerPrefs.SetFloat("CamDistance", value);
        _buttonPopAudio.Play();
        _distanceTxt.text = value.ToString("0.##");
    }

    public void SetToggleSprint(bool value)
    {
        _buttonPopAudio.Play();
        PlayerPrefs.SetInt("ToggleRun", value ? 1 : 0);
    }

    public void PlayButtonPop()
    {
        _buttonPopAudio.Play();
    }

    public void DeleteSaveFileButton(int index)
    {
        _buttonPopAudio.Play();
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

    IEnumerator EnableAudio()
    {
        yield return new WaitForSeconds(0.6f);
        GetComponent<AudioSource>().volume = 1;
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
