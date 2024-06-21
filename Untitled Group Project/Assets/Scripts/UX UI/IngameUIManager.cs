using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class IngameUIManager : MonoBehaviour
{
    CharStateMachine _charStateMachine;
    PlayerStats _playerStats;

    [SerializeField] private PlayerInput playerInput = null;
    public PlayerInput PlayerInput => playerInput; PlayerInput _playerInput;

    #region Panels
    [Header("Panels")]
    public GameObject inventoryCanvas;
    public GameObject craftingCanvas;
    public GameObject hudCanvas;
    public GameObject upgradesCanvas;
    [SerializeField] GameObject _interactPanel;
    [SerializeField] GameObject _pausePanel;

    [SerializeField] GameObject _devPanel;
    bool _openedWithCrafting;
    #endregion

    #region Interacting
    [Header("Interacting")]
    [SerializeField] float _interactionRange = 3f;
    public LayerMask interactableLayers;
    [SerializeField] TMP_Text _interactableTxt;
    Transform _closestTransform = null;
    float _closestDistance = Mathf.Infinity;
    #endregion

    [Header("QuestBoard")]
    [SerializeField] Camera _questBoardCam;
    [SerializeField] Camera _normalCam;

    [Header("XP")]
    public bool smoothXpSlider;
    [SerializeField] Transform _xpBar;
    [SerializeField] Image _xpSliderImage;
    [SerializeField] TMP_Text _xpSliderText;
    [SerializeField] TMP_Text _levelText;

    #region Health
    [Header("Health")]
    [SerializeField] Image _healthSliderImage;
    [SerializeField] TMP_Text _healthTxt;

    [SerializeField] Color _greenHp;
    [SerializeField] Color _yellowHp;
    [SerializeField] Color _redHp;
    #endregion

    #region Settings
    [Header("General Settings")]
    [SerializeField] Toggle _toggleRunBtn;
    [SerializeField] Slider _sensSlider;
    [SerializeField] Slider _distanceSlider;

    [SerializeField] float _mouseSensitivity;
    [SerializeField] bool _toggleRun;
    [SerializeField] float _distanceToCam;

    [Header("Audio Settings")]
    [SerializeField] AudioMixer _mainMixer;
    [SerializeField] AudioSource _buttonPopAudio;

    [SerializeField] Slider _masterSlider;
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _sfxSlider;
    [SerializeField] Slider _ambienceSlider;

    #endregion
    #region PlayerPrefs
    [Header("PlayerPrefs")]
    [SerializeField] string _mouseSens;
    [SerializeField] string _runToggle;
    [SerializeField] string _camDistance;
    #endregion

    bool _onInteract;
    bool _onPause;

    [SerializeField] bool _paused;

    private void OnEnable()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
        _playerInput = FindObjectOfType<PlayerInput>();

        _playerStats.OnXpGained += OnXpGained;

        _playerInput.actions.FindActionMap("Game").Enable();
        _playerInput.actions.FindActionMap("Menu").Enable();

        _playerInput.actions.FindAction("Pause").started += OnPause;

        _playerInput.actions.FindAction("Inventory").started += OnInventory;

        _playerInput.actions.FindAction("Interact").started += OnInteract;
        _playerInput.actions.FindAction("Interact").canceled += OnInteract;
    }

    private void OnDisable()
    {
        _playerStats.OnXpGained -= OnXpGained;
        if (playerInput != null)
        {
            _playerInput.actions.FindAction("Pause").started -= OnPause;
        }
    }


    #region Inputs

    void OnPause(InputAction.CallbackContext context)
    {
        _onPause = context.ReadValueAsButton();
        if (_onPause)
        {
            if (!_paused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    void OnInventory(InputAction.CallbackContext context)
    {
        ToggleInventory();
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        _onInteract = context.ReadValueAsButton();
    }

    #endregion

    void Start()
    {
        _xpSliderImage = _xpBar.GetChild(2).GetComponent<Image>();
        if (_playerStats.xp != 0)
            _xpSliderImage.fillAmount = _playerStats.xp / _playerStats.xpGoal;
        else
        {
            _xpSliderImage.fillAmount = 0;
        }
        _xpSliderText.text = _playerStats.xp + "/" + _playerStats.xpGoal;
        _levelText.text = _playerStats.level.ToString();


        hudCanvas.GetComponent<Canvas>().enabled = false;
        upgradesCanvas.GetComponent<Canvas>().enabled = false;
        upgradesCanvas.GetComponent<GraphicRaycaster>().enabled = false;

        if (_questBoardCam)
        {
            _questBoardCam.gameObject.SetActive(false);
            _normalCam.gameObject.SetActive(true);
        }

        Time.timeScale = 1;

        #region PlayerPrefs
        _masterSlider.value = PlayerPrefs.GetFloat("MasterVol", 1);
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVol", 1);
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVol", 1);
        _ambienceSlider.value = PlayerPrefs.GetFloat("AmbienceVol", 1);

        SetMasterVol(_masterSlider.value);
        SetMusicVol(_musicSlider.value);
        SetSfxVol(_sfxSlider.value);
        SetAmbienceVol(_ambienceSlider.value);

        ChangeSensitivity(PlayerPrefs.GetFloat(_mouseSens, 1));
        _sensSlider.value = _mouseSensitivity;
        ChangeCamDistance(PlayerPrefs.GetFloat(_camDistance, 4));
        _distanceSlider.value = _distanceToCam;

        if (PlayerPrefs.GetInt(_runToggle) == 0)
        {
            ToggleRun(false);
            _toggleRunBtn.isOn = false;
        }
        else if (PlayerPrefs.GetInt(_runToggle) == 1)
        {
            ToggleRun(true);
            _toggleRunBtn.isOn = true;
        }

        #endregion


        _charStateMachine = FindObjectOfType<CharStateMachine>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
            CheckInteractable();

        if (IsUIShowing())
        {
            Time.timeScale = 0;
            if (_playerInput.actions.FindActionMap("Game").enabled)
            {
                _playerInput.actions.FindActionMap("Game").Disable();
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            _interactPanel.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            Time.timeScale = 1;
            if (_closestTransform != null)
            {
                _interactPanel.SetActive(true);
            }
            else
            {
                _interactPanel.SetActive(false);
            }
            if (!_playerInput.actions.FindActionMap("Game").enabled)
            {
                _playerInput.actions.FindActionMap("Game").Enable();
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (hudCanvas.GetComponent<Canvas>().enabled == false && SceneManager.GetActiveScene().name != "MainMenu")
        {
            hudCanvas.GetComponent<Canvas>().enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.P) && SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (_devPanel)
                _devPanel.SetActive(!_devPanel.activeSelf);
        }

        HandleHealth();

    }

    void HandleHealth()
    {
        _healthSliderImage.fillAmount = _charStateMachine.GetHealth() / _charStateMachine.GetMaxHealth();
        _healthTxt.text = $"{_charStateMachine.GetHealth()}/{_charStateMachine.GetMaxHealth()}";

        if (_healthSliderImage.fillAmount * 100 > 70)
        {
            _healthSliderImage.color = _greenHp;
        }
        else if (_healthSliderImage.fillAmount * 100 < 70)
        {
            _healthSliderImage.color = _yellowHp;

            if (_healthSliderImage.fillAmount * 100 < 40)
            {
                _healthSliderImage.color = _redHp;
            }
        }
    }
    #region UI Panels
    public void ToggleInventory()
    {
        if (craftingCanvas.activeSelf && inventoryCanvas.activeSelf)
        {
            craftingCanvas.GetComponent<Canvas>().enabled = false;
            craftingCanvas.GetComponent<GraphicRaycaster>().enabled = false;
        }
        if (_questBoardCam && _questBoardCam.gameObject.activeSelf)
        {
            ToggleQuestBoard();
        }
        if (upgradesCanvas.GetComponent<Canvas>().enabled && inventoryCanvas.GetComponent<Canvas>().enabled)
        {
            ToggleUpgrades();
        }
        inventoryCanvas.GetComponent<Canvas>().enabled = !inventoryCanvas.GetComponent<Canvas>().enabled;
        inventoryCanvas.GetComponent<GraphicRaycaster>().enabled = inventoryCanvas.GetComponent<Canvas>().enabled;
    }

    public void ToggleCrafting()
    {
        craftingCanvas.GetComponent<Canvas>().enabled = !craftingCanvas.GetComponent<Canvas>().enabled;
        craftingCanvas.GetComponent<GraphicRaycaster>().enabled = craftingCanvas.GetComponent<Canvas>().enabled;

        if (!inventoryCanvas.GetComponent<Canvas>().enabled)
        {
            inventoryCanvas.GetComponent<Canvas>().enabled = true;
            inventoryCanvas.GetComponent<GraphicRaycaster>().enabled = true;
            _openedWithCrafting = true;
        }
        else if (_openedWithCrafting)
        {
            inventoryCanvas.GetComponent<Canvas>().enabled = false;
            inventoryCanvas.GetComponent<GraphicRaycaster>().enabled = false;
            _openedWithCrafting = false;
        }
    }

    void ToggleQuestBoard()
    {
        if (!_questBoardCam)
            return;

        if (_questBoardCam.gameObject.activeSelf)
        {
            // _questBoardCam.enabled = false;
            _questBoardCam.gameObject.SetActive(false);
            // _normalCam.enabled = true;
            _normalCam.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // _questBoardCam.enabled = true;
            _questBoardCam.gameObject.SetActive(true);
            // _normalCam.enabled = false;
            _normalCam.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void ToggleUpgrades()
    {
        if (craftingCanvas.GetComponent<Canvas>().enabled)
        {
            ToggleCrafting();
        }
        if (!inventoryCanvas.GetComponent<Canvas>().enabled && !upgradesCanvas.GetComponent<Canvas>().enabled ||
        inventoryCanvas.GetComponent<Canvas>().enabled && upgradesCanvas.GetComponent<Canvas>().enabled)
        {
            // ToggleInventory();
            inventoryCanvas.GetComponent<Canvas>().enabled = !inventoryCanvas.GetComponent<Canvas>().enabled;
            inventoryCanvas.GetComponent<GraphicRaycaster>().enabled = inventoryCanvas.GetComponent<Canvas>().enabled;
        }
        upgradesCanvas.GetComponent<Canvas>().enabled = !upgradesCanvas.GetComponent<Canvas>().enabled;
        upgradesCanvas.GetComponent<GraphicRaycaster>().enabled = upgradesCanvas.GetComponent<Canvas>().enabled;
    }
    #endregion
    void OnXpGained(int xpAmount)
    {
        Debug.Log($"Gained {xpAmount} xp");
        _xpBar.GetComponent<Animator>().SetTrigger("Trigger");

        if (_playerStats.xp != 0)
            StartCoroutine(XpSlider(xpAmount));
        else
        {
            _xpSliderImage.fillAmount = 0;
            _xpSliderText.text = _playerStats.xp + "/" + _playerStats.xpGoal;
            _levelText.text = _playerStats.level.ToString();
        }
    }


    private void CheckInteractable()
    {
        Collider[] colliders = Physics.OverlapSphere(_playerStats.transform.position, _interactionRange, interactableLayers);


        if (colliders.Length != 0)
        {
            foreach (var collider in colliders)
            {

                Vector3 towards = collider.transform.position - _playerStats.transform.position;
                if (Physics.Raycast(_playerStats.transform.position, towards, out RaycastHit hit, Mathf.Infinity, interactableLayers))
                {
                    Debug.Log(hit.transform.name);
                    if (Vector3.Distance(hit.point, _playerStats.transform.position) < _closestDistance)
                    {
                        _closestTransform = hit.transform;
                        _closestDistance = Vector3.Distance(hit.point, _playerStats.transform.position);
                    }
                }
                if (collider.transform.GetComponent<DroppedItem>() && _closestTransform == collider.transform)
                {
                    DroppedItem droppedItem = collider.transform.GetComponent<DroppedItem>();
                    if (droppedItem.item.Count > 1)
                    {
                        if (_interactableTxt != null)
                        {
                            _interactableTxt.text = $"Press ({_playerInput.actions.FindAction("Interact", true).GetBindingDisplayString()}) to pick up stack of items";
                        }
                    }
                    else
                    {
                        if (_interactableTxt != null)
                        {

                            _interactableTxt.text = $"Press ({_playerInput.actions.FindAction("Interact", true).GetBindingDisplayString()}) to pick up {droppedItem.amount[0]} {droppedItem.item[0].name}";
                        }
                    }
                    if (_interactPanel != null)
                    {
                        _interactPanel.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning("No InteractPanel Set, Interaction Text Won't Show Up");
                    }

                    if (_onInteract)
                    {
                        _onInteract = false;
                        for (int i = 0; i < droppedItem.item.Count; i++)
                        {
                            if (droppedItem.amount[i] > droppedItem.item[i].maxStack)
                            {
                                if (InventoryManager.Instance.HasSpace(droppedItem.item[i], droppedItem.amount[i]))
                                {
                                    InventoryManager.Instance.AddItem(droppedItem.item[i].itemID, droppedItem.amount[i]);
                                    droppedItem.amount[i] -= droppedItem.amount[i];
                                }
                            }
                            else if (InventoryManager.Instance.HasSpace(droppedItem.item[i], droppedItem.amount[i]))
                            {
                                InventoryManager.Instance.AddItem(droppedItem.item[i].itemID, droppedItem.amount[i]);
                                Destroy(droppedItem.gameObject);
                                // Debug.Log($"Pressed {_playerInput.actions.FindAction("Interact").GetBindingDisplayString()} To Pick Up {droppedItem.amount} {droppedItem.item}");
                            }
                            else
                            {
                                // Debug.Log($"[NO SPACE] Pressed {_playerInput.actions.FindAction("Interact").GetBindingDisplayString()} To Pick Up {droppedItem.amount} {droppedItem.item}, but had no room in inventory");
                            }
                        }

                    }
                }
                if (collider.transform.CompareTag("Crafting") && _closestTransform == collider.transform)
                {
                    if (_interactPanel != null)
                    {
                        _interactPanel.SetActive(true);
                        if (_interactableTxt != null)
                        {
                            _interactableTxt.text = $"Press ({_playerInput.actions.FindAction("Interact", true).GetBindingDisplayString()}) to open Anvil";
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No InteractPanel Set, Interaction Text Won't Show Up");
                    }
                    if (_onInteract)
                    {
                        _onInteract = false;
                        ToggleCrafting();
                        if (upgradesCanvas.GetComponent<Canvas>().enabled)
                        {
                            upgradesCanvas.GetComponent<Canvas>().enabled = false;
                            upgradesCanvas.GetComponent<GraphicRaycaster>().enabled = false;
                        }
                    }

                }
                else if (collider.transform.GetComponent<QuestBoardManager>() && _closestTransform == collider.transform)
                {
                    if (_interactPanel != null)
                    {
                        _interactPanel.SetActive(true);
                        if (_interactableTxt != null)
                        {
                            _interactableTxt.text = $"Press ({_playerInput.actions.FindAction("Interact", true).GetBindingDisplayString()}) to open Quest Board";
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No InteractPanel Set, Interaction Text Won't Show Up");
                    }
                    if (_onInteract)
                    {
                        _onInteract = false;
                        ToggleQuestBoard();
                    }

                }
                else if (collider.transform.CompareTag("Upgrade") && _closestTransform == collider.transform)
                {
                    if (_interactPanel != null)
                    {
                        _interactPanel.SetActive(true);
                        if (_interactableTxt != null)
                        {
                            _interactableTxt.text = $"Press ({_playerInput.actions.FindAction("Interact", true).GetBindingDisplayString()}) to open Forge";
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No InteractPanel Set, Interaction Text Won't Show Up");
                    }
                    if (_onInteract)
                    {
                        _onInteract = false;
                        ToggleUpgrades();
                    }

                }
                else
                {
                    if (_interactPanel != null)
                    {
                        _interactPanel.SetActive(false);
                        Debug.Log("SetCanvasFalse");
                        _interactableTxt.text = "";
                    }
                    else
                    {
                        Debug.LogWarning("No InteractPanel Set, Interaction Text Won't Show Up");
                    }
                    _closestTransform = null;
                    _closestDistance = Mathf.Infinity;
                }
            }

        }
        else
        {
            if (_interactPanel != null)
            {
                _interactPanel.SetActive(false);
                // Debug.Log("SetCanvasFalse");
                _interactableTxt.text = "";
            }
            else
            {
                Debug.LogWarning("No InteractPanel Set, Interaction Text Won't Show Up");
            }
            _closestTransform = null;
            _closestDistance = Mathf.Infinity;
        }
    }

    bool IsUIShowing()
    {
        if (inventoryCanvas.GetComponent<Canvas>().enabled || craftingCanvas.GetComponent<Canvas>().enabled || _devPanel.activeSelf || _pausePanel.activeSelf)
        {
            return true;
        }
        if (_questBoardCam && _questBoardCam.gameObject.activeSelf)
        {
            return true;
        }

        return false;
    }

    IEnumerator XpSlider(int xpAmount, bool firstCall = true)
    {
        float requiredFillAmount = (float)_playerStats.xp / (float)_playerStats.xpGoal;
        Debug.Log($"Required fill amount: {requiredFillAmount}");
        if (firstCall)
        {
            _xpSliderText.text = _playerStats.xp + "/" + _playerStats.xpGoal;
            _levelText.text = _playerStats.level.ToString();
            yield return new WaitForSeconds(0.2f);
        }
        if (smoothXpSlider)
        {
            yield return new WaitForSeconds(0.1f);

            if (_xpSliderImage.fillAmount < requiredFillAmount)
            {
                _xpSliderImage.fillAmount += xpAmount / 100;
                Debug.Log($"Slider FillAmount Set To {_xpSliderImage.fillAmount}");
                StartCoroutine(XpSlider(xpAmount, false));
            }
            else
            {
                Debug.Log($"XpSlider Value Was {_xpSliderImage.fillAmount} and needed to be {requiredFillAmount}");
                _xpSliderImage.fillAmount = requiredFillAmount;
            }
        }
        else
        {
            _xpSliderImage.fillAmount = requiredFillAmount;
            Debug.Log($"Updated Xp Slider {_playerStats.xp}/ {_playerStats.xpGoal}");
        }
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
    public void SetAmbienceVol(float vol)
    {
        _mainMixer.SetFloat("AmbienceVol", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("AmbienceVol", vol);
        _buttonPopAudio.Play();
    }

    public void ChangeSensitivity(float value)
    {
        _mouseSensitivity = value;
        PlayerPrefs.SetFloat(_mouseSens, _mouseSensitivity);
    }

    public void ChangeCamDistance(float value)
    {
        _distanceToCam = value;

        PlayerPrefs.SetFloat(_camDistance, _distanceToCam);
    }

    public void ToggleRun(bool value = false)
    {

        _toggleRun = value;
        if (_toggleRun)
        {
            PlayerPrefs.SetInt(_runToggle, 1);
        }
        else
        {
            PlayerPrefs.SetInt(_runToggle, 0);
        }

    }

    public void Pause()
    {
        _playerInput.actions.FindActionMap("Game").Disable();
        Time.timeScale = 0;
        _pausePanel.SetActive(true);
        _paused = true;
    }

    public void Resume()
    {
        _playerInput.actions.FindActionMap("Game").Enable();
        _charStateMachine.SetSettings();
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
        _paused = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        if (DataPersistenceManager.instance)
            DataPersistenceManager.instance.SaveAutoGame();
        SceneFader.Instance.FadeTo("MainMenu");
    }
}
