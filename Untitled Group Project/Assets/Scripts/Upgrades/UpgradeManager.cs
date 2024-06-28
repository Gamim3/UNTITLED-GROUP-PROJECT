using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public InventorySlot upgradeSlot;
    [SerializeField] Upgrade _upgrade;

    [SerializeField] float _currentDamageMultiplier;
    [SerializeField] float _currentArmorMultiplier;

    [Header("UI")]
    [SerializeField] Button _upgradeBtn;
    [SerializeField] TMP_Text _upgradeTxt;

    [SerializeField] string _damageString;
    [SerializeField] string _armorString;

    [SerializeField] TMP_Text _currentDamageMuliplierTxt;
    [SerializeField] TMP_Text _currentArmorMultiplierTxt;

    private void Start()
    {
        _currentDamageMultiplier = FindObjectOfType<CharStateMachine>().DamageMultiplier;
        _currentDamageMuliplierTxt.text = _currentDamageMultiplier.ToString() + "x";
        // _currentArmorTxt.text = FindObjectOfType<CharStateMachine>().Armor.ToString();
        _currentArmorMultiplierTxt.text = "1x";

        _damageString = _currentDamageMuliplierTxt.text;
        _armorString = _currentArmorMultiplierTxt.text;
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
                if (_currentDamageMuliplierTxt != null)
                    _currentDamageMuliplierTxt.text = _damageString;
                if (_currentArmorMultiplierTxt != null)
                    _currentArmorMultiplierTxt.text = _armorString;
            }
            _upgrade = null;
            if (_upgradeBtn != null)
                _upgradeBtn.interactable = false;
            return;
        }
        else if (upgradeSlot.GetInventoryItem().item.isWeapon || upgradeSlot.GetInventoryItem().item.isArmor)
        {
            _upgrade = upgradeSlot.GetInventoryItem().item.upgrade;
            if (_upgradeBtn != null)
                _upgradeBtn.interactable = true;
            if (_upgradeTxt != null)
            {
                _upgradeTxt.text = $"Upgrade {_upgrade.upgradeType.ToString().ToLower()} by {_upgrade.upgradeMultiplier}x";
            }

            if (_upgrade.upgradeType == UpgradeType.WEAPON)
            {
                if (_currentDamageMuliplierTxt != null)
                {
                    if (!_currentDamageMuliplierTxt.text.Contains($"->{_upgrade.upgradeMultiplier}"))
                    {
                        _currentDamageMuliplierTxt.text = $"{_currentDamageMuliplierTxt.text}->{_upgrade.upgradeMultiplier}";
                    }
                }
            }
            else if (_upgrade.upgradeType == UpgradeType.ARMOR)
            {
                if (_currentArmorMultiplierTxt != null)
                {
                    if (!_currentArmorMultiplierTxt.text.Contains($"->{_upgrade.upgradeMultiplier}"))
                    {
                        _currentArmorMultiplierTxt.text = $"{_currentArmorMultiplierTxt.text}->{_upgrade.upgradeMultiplier}";
                    }
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
            if (_currentDamageMuliplierTxt != null)
                _currentDamageMuliplierTxt.text = _damageString;
            if (_currentArmorMultiplierTxt != null)
                _currentArmorMultiplierTxt.text = _armorString;
            _upgrade = null;
        }
    }

    public void Upgrade()
    {
        if (_upgrade == null)
        {
            Debug.LogError("Clicked Upgrade With No Selected Upgrade (BUTTON SHOULD NOT BE INTERACTABLE)");
            return;
        }
        else if (_upgrade.upgradeType == UpgradeType.WEAPON)
        {
            _currentDamageMultiplier = _upgrade.upgradeMultiplier;
            _currentDamageMuliplierTxt.text = _currentDamageMultiplier.ToString();
            _damageString = _currentDamageMuliplierTxt.text;
            FindObjectOfType<CharStateMachine>().DamageMultiplier = _currentDamageMultiplier;
            //Upgrade Dmg Amount by _upgrade.upgradeAmount
            //Update _damageString to new value
        }
        else if (_upgrade.upgradeType == UpgradeType.ARMOR)
        {
            //Upgrade Dmg Taken by _upgrade.upgradeAmount
            //Update _armorString to new value
        }
        Destroy(upgradeSlot.GetInventoryItem().gameObject);
    }
}
