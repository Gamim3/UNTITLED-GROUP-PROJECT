using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    CharStateMachine _charStateMachine;

    [Header("Panels")]
    public GameObject inventoryCanvas;
    public GameObject craftingCanvas;
    public GameObject hudCanvas;

    [SerializeField] GameObject _devPanel;
    bool _openedWithCrafting;

    //Debug Raycast, Replace With Actual Player Raycast
    public Transform playerRaycastPos;
    public LayerMask interactableLayers;

    [Header("QuestBoard")]
    [SerializeField] Camera _questBoardCam;
    [SerializeField] Camera _normalCam;

    [Header("XP")]
    public bool smoothXpSlider;
    [SerializeField] Transform _xpBar;
    [SerializeField] Image _xpSliderImage;
    [SerializeField] TMP_Text _xpSliderText;
    [SerializeField] TMP_Text _levelText;

    [Header("Health")]
    [SerializeField] Image _healthSliderImage;
    [SerializeField] TMP_Text _healthTxt;

    PlayerStats _playerStats;

    private void OnEnable()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
        _playerStats.OnXpGained += OnXpGained;
    }

    private void OnDisable()
    {
        _playerStats.OnXpGained -= OnXpGained;
    }
    // Start is called before the first frame update
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

        if (_questBoardCam)
        {
            // _questBoardCam.enabled = false;
            _questBoardCam.gameObject.SetActive(false);
            // _normalCam.enabled = true;
            _normalCam.gameObject.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _charStateMachine = FindObjectOfType<CharStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsUIShowing())
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (hudCanvas.GetComponent<Canvas>().enabled == false && SceneManager.GetActiveScene().name != "MainMenu")
        {
            hudCanvas.GetComponent<Canvas>().enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.E) && playerRaycastPos != null)
        {
            if (Physics.Raycast(playerRaycastPos.position, playerRaycastPos.forward, out RaycastHit hit, interactableLayers))
            {
                if (hit.transform.tag == "Crafting")
                {
                    ToggleCrafting();
                }
                else if (hit.transform.GetComponent<QuestBoardManager>())
                {
                    ToggleQuestBoard();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_devPanel)
                _devPanel.SetActive(!_devPanel.activeSelf);
        }

        _healthSliderImage.fillAmount = _charStateMachine.GetHealth() / _charStateMachine.GetMaxHealth();
        _healthTxt.text = $"{_charStateMachine.GetHealth()}/{_charStateMachine.GetMaxHealth()}";
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

    bool IsUIShowing()
    {
        bool value = false;

        if (inventoryCanvas.GetComponent<Canvas>().enabled || craftingCanvas.GetComponent<Canvas>().enabled || _devPanel.activeSelf)
        {
            return true;
        }
        if (_questBoardCam && _questBoardCam.gameObject.activeSelf)
        {
            return true;
        }

        return value;
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
}
