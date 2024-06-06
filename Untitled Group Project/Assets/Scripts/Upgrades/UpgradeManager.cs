using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public InventorySlot upgradeSlot;
    [SerializeField] Upgrade _upgrade;

    [Header("UI")]
    [SerializeField] Button _upgradeBtn;
    [SerializeField] TMP_Text _upgradeTxt;

    [SerializeField] string _damageString;
    [SerializeField] string _armorString;

    [SerializeField] TMP_Text _currentDamageTxt;
    [SerializeField] TMP_Text _currentArmorTxt;

    private void Start()
    {
        _damageString = _currentDamageTxt.text;
        _armorString = _currentArmorTxt.text;
    }

    void Update()
    {
        if (upgradeSlot.GetInventoryItem() == null)
        {
            if (_upgradeBtn != null)
                _upgradeBtn.interactable = false;
            if (_upgradeTxt != null)
            {
                _upgradeTxt.text = "Insert a weapon or armor into the upgrade slot";
            }
            if (_upgrade != null)
            {
                if (_currentDamageTxt != null)
                    _currentDamageTxt.text = _damageString;
                if (_currentArmorTxt != null)
                    _currentArmorTxt.text = _armorString;
            }
            _upgrade = null;
            return;
        }
        else if (upgradeSlot.GetInventoryItem().item.isWeapon || upgradeSlot.GetInventoryItem().item.isArmor)
        {
            _upgrade = upgradeSlot.GetInventoryItem().item.upgrade;
            if (_upgradeBtn != null)
                _upgradeBtn.interactable = true;
            if (_upgradeTxt != null)
            {
                _upgradeTxt.text = $"Upgrade {_upgrade.upgradeType.ToString().ToLower()} by {_upgrade.upgradeAmount}";
            }
            if (_upgrade == null)
            {
                Debug.LogError("Clicked Upgrade With No Selected Upgrade (BUTTON SHOULD NOT BE INTERACTABLE)");
            }
            else if (_upgrade.upgradeType == UpgradeType.WEAPON)
            {
                if (_currentDamageTxt != null)
                {
                    if (!_currentDamageTxt.text.Contains($" + {_upgrade.upgradeAmount}"))
                    {
                        _currentDamageTxt.text = $"{_currentDamageTxt.text} + {_upgrade.upgradeAmount}";
                    }
                }
            }
            else if (_upgrade.upgradeType == UpgradeType.ARMOR)
            {
                if (_currentArmorTxt.text != _damageString)
                {
                    Debug.LogWarning($"_currentArmorTxt.text != {_currentArmorTxt.text} + {_upgrade.upgradeAmount}");
                    _currentArmorTxt.text = $"{_currentArmorTxt.text} + {_upgrade.upgradeAmount}";
                    Debug.LogWarning($"Set _currentArmorTxt.text To {_currentArmorTxt.text}");
                }
            }

        }
        else
        {
            if (_upgradeBtn != null)
                _upgradeBtn.interactable = false;
            if (_upgradeTxt != null)
            {
                _upgradeTxt.text = "Invalid Item Upgrade";
            }
            if (_currentDamageTxt != null)
                _currentDamageTxt.text = _damageString;
            if (_currentArmorTxt != null)
                _currentArmorTxt.text = _armorString;
            _upgrade = null;
        }
    }

    public void Upgrade()
    {
        if (_upgrade == null)
        {
            Debug.LogError("Clicked Upgrade With No Selected Upgrade (BUTTON SHOULD NOT BE INTERACTABLE)");
        }
        else if (_upgrade.upgradeType == UpgradeType.WEAPON)
        {
            //Upgrade Dmg Amount by _upgrade.upgradeAmount
            //Update _damageString to new value
        }
        else if (_upgrade.upgradeType == UpgradeType.ARMOR)
        {
            //Upgrade Dmg Taken by _upgrade.upgradeAmount
            //Update _armorString to new value
        }
    }
}
