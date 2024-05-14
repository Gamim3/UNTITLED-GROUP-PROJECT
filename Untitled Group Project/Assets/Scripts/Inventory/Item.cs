using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [Tooltip("ItemID Is Set Automatically [DO NOT CHANGE]")]
    public int itemID;
    public GameObject itemPrefab;

    [Header("Settings")]
    public Sprite image;
    public int maxStack = 1;

    public bool itemIdSet;
}