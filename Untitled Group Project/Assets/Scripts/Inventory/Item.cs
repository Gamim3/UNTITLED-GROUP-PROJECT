using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [Tooltip("ItemID Is Set Automatically [DO NOT CHANGE]")]
    public int itemID;
    public bool itemIdSet;
    public GameObject itemPrefab;

    [Header("Settings")]
    public Sprite image;
    public int maxStack = 1;

    [Header("Upgrades")]
    public bool isWeapon;
    public bool isArmor;
    [Tooltip("Leave Empty If Item Is Not A Weapon Or Armor")]
    public Upgrade upgrade;
}