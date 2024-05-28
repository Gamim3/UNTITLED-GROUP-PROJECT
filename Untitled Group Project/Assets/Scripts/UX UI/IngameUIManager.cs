using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject inventoryCanvas;
    public GameObject craftingCanvas;

    [SerializeField] GameObject _devPanel;
    bool _openedWithCrafting;

    //Debug Raycast, Replace With Actual Player Raycast
    public Transform playerRaycastPos;
    public LayerMask interactableLayers;


    [Header("XP")]
    [SerializeField] Transform _xpBar;
    [SerializeField] Image _xpSliderImage;
    [SerializeField] TMP_Text _xpSliderText;
    [SerializeField] TMP_Text _levelText;

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
    }

    // Update is called once per frame
    void Update()
    {
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
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_devPanel)
                _devPanel.SetActive(!_devPanel.activeSelf);
        }
    }

    public void ToggleInventory()
    {
        if (craftingCanvas.activeSelf && inventoryCanvas.activeSelf)
        {
            craftingCanvas.GetComponent<Canvas>().enabled = false;
        }
        inventoryCanvas.GetComponent<Canvas>().enabled = !inventoryCanvas.GetComponent<Canvas>().enabled;
    }

    public void ToggleCrafting()
    {
        craftingCanvas.GetComponent<Canvas>().enabled = !craftingCanvas.GetComponent<Canvas>().enabled;
        if (!inventoryCanvas.GetComponent<Canvas>().enabled)
        {
            inventoryCanvas.GetComponent<Canvas>().enabled = true;
            _openedWithCrafting = true;
        }
        else if (_openedWithCrafting)
        {
            inventoryCanvas.GetComponent<Canvas>().enabled = false;
            _openedWithCrafting = false;
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
        }
        _xpSliderText.text = _playerStats.xp + "/" + _playerStats.xpGoal;
        _levelText.text = _playerStats.level.ToString();
    }

    IEnumerator XpSlider(int xpAmount)
    {
        yield return new WaitForSeconds(0.005f);

        if (_xpSliderImage.fillAmount < _playerStats.xp / _playerStats.xpGoal)
        {
            _xpSliderImage.fillAmount += xpAmount / 100;
            StartCoroutine(XpSlider(xpAmount));
        }
    }
}
