using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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
    //Debug Raycast, Replace With Actual Player Raycast
    public Transform playerRaycastPos;
    [SerializeField] Vector3 _shootOffset = new Vector3(0, 0, -1);
    [SerializeField] float _rayRadius = 1f;
    [SerializeField] float _interactRange = 4f;
    public LayerMask interactableLayers;
    RaycastHit _interactableHit;
    [SerializeField] TMP_Text _interactableTxt;
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
    [Header("Settings")]
    [SerializeField] Toggle _toggleRunBtn;
    [SerializeField] Slider _sensSlider;
    [SerializeField] Slider _distanceSlider;

    [SerializeField] float _mouseSensitivity;
    [SerializeField] bool _toggleRun;
    [SerializeField] float _distanceToCam;
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
        ChangeSensitivity(PlayerPrefs.GetFloat(_mouseSens));
        _sensSlider.value = _mouseSensitivity;
        ChangeCamDistance(PlayerPrefs.GetFloat(_camDistance));
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
        if (IsUIShowing())
        {
            if (_playerInput.actions.FindActionMap("Game").enabled)
            {
                _playerInput.actions.FindActionMap("Game").Disable();
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (SceneManager.GetActiveScene().name != "MainMenu")
        {
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

        if (Input.GetKeyDown(KeyCode.E) && playerRaycastPos != null)
        {
            if (Physics.Raycast(playerRaycastPos.position + _shootOffset, playerRaycastPos.forward, out RaycastHit hit, interactableLayers))
            {
                if (hit.transform.CompareTag("Crafting"))
                {
                    ToggleCrafting();
                }
                else if (hit.transform.GetComponent<QuestBoardManager>())
                {
                    ToggleQuestBoard();
                }
                else if (hit.transform.CompareTag("Upgrade"))
                {
                    ToggleUpgrades();
                }
            }
        }

        CheckInteractable();

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

    void OnXpGained(int xpAmount)
    {
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
        if (Physics.SphereCast(_normalCam.transform.position + _shootOffset, _rayRadius, _normalCam.transform.forward, out _interactableHit, _interactRange + 3f))
        {
            Debug.DrawRay(_normalCam.transform.position + _shootOffset, _interactableHit.point - _normalCam.transform.position + _shootOffset, Color.red);
            if (_interactableHit.transform.GetComponent<DroppedItem>())
            {
                DroppedItem droppedItem = _interactableHit.transform.GetComponent<DroppedItem>();
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
                    _interactPanel.SetActive(false);
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
            else
            {
                if (_interactPanel != null)
                {
                    _interactPanel.SetActive(false);
                    _interactableTxt.text = "";
                }
                else
                {
                    Debug.LogWarning("No InteractPanel Set, Interaction Text Won't Show Up");
                }
            }
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

    IEnumerator XpSlider(int xpAmount)
    {
        if (smoothXpSlider)
        {
            yield return new WaitForSeconds(0.01f);

            if (_xpSliderImage.fillAmount < _playerStats.xp / _playerStats.xpGoal)
            {
                _xpSliderImage.fillAmount += xpAmount / 100;
                StartCoroutine(XpSlider(xpAmount));
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            _xpSliderImage.fillAmount = _playerStats.xp / _playerStats.xpGoal;
            _xpSliderText.text = _playerStats.xp + "/" + _playerStats.xpGoal;
            _levelText.text = _playerStats.level.ToString();
        }

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
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
        _paused = false;
    }

    public void MainMenu()
    {
        if (DataPersistenceManager.instance)
            DataPersistenceManager.instance.SaveAutoGame();
        SceneManager.LoadScene("MainMenu");
    }
}
