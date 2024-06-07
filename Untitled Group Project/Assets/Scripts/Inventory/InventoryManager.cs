using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public GameObject player;

    [SerializeField] Transform _slotParent;
    public List<InventorySlot> inventorySlots = new();

    public Transform rootTransform;

    [SerializeField] GameObject _inventoryItemPrefab;
    public GameObject InventoryItemPrefab { get { return _inventoryItemPrefab; } }

    int _selectedSlot = -1;

    public List<ItemInfo> itemsInInventory = new();

    public Item[] allItems;

    public InventoryItem heldItem;
    // [SerializeField] InGameUIManager _uiManager;

    [SerializeField] Item _itemToSpawn;

    [SerializeField] GameObject _droppedBag;

    [Header("Canvas Information")]
    public EventSystem eventSystem;
    public List<GraphicRaycaster> graphicRaycasters;
    public RectTransform rectTransform;

    public event ItemRecieved OnItemRecieved;
    public delegate void ItemRecieved();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else if (Instance == null)
        {
            Instance = this;
        }

        Debug.Log(Instance);
        // player = FindObjectOfType<CharStateMachine>().gameObject;
    }


    private void Start()
    {
        UpdateItemsInfoList();
        player = FindObjectOfType<CharStateMachine>().gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdateItemsInfoList();
            Debug.LogWarning("Manually Called UpdateItemInfoList (KeyCode.U)");
        }
    }

    public Item GetSelectedItem()
    {
        if (_selectedSlot > -1)
        {
            if (inventorySlots[_selectedSlot].GetInventoryItem() != null)
                return inventorySlots[_selectedSlot].GetInventoryItem().item;
            else return null;
        }
        else
        {
            return null;
        }
    }


    public bool AddItem(int itemId, int amount)
    {
        if (itemId <= 0)
        {
            _itemToSpawn = null;
            return false;
        }
        for (int i = 0; i < allItems.Length; i++)
        {
            if (allItems[i].itemID == itemId)
            {
                _itemToSpawn = allItems[i];
                Debug.Log($"Adding Item {allItems[i]}");
            }
        }
        if (!HasSpace(_itemToSpawn, amount))
        {
            Debug.Log("Inventory Was Full, Could Not Add " + _itemToSpawn);
            return false;
        }
        //Check for stackable slot
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();


            if (itemInSlot != null && itemInSlot.item == _itemToSpawn && itemInSlot.count < itemInSlot.item.maxStack)
            {
                if (itemInSlot.count + amount > itemInSlot.item.maxStack)
                {
                    int extra = itemInSlot.count + amount - itemInSlot.item.maxStack;
                    Debug.Log("Exta: " + extra);
                    itemInSlot.count = itemInSlot.item.maxStack;
                    AddItem(itemId, extra);
                    UpdateItemsInfoList();

                    itemInSlot.RefreshCount();
                    OnItemRecieved?.Invoke();
                    return true;
                }
                itemInSlot.count += amount;
                itemInSlot.RefreshCount();
                UpdateItemsInfoList();
                OnItemRecieved?.Invoke();

                return true;
            }

        }
        //Check for empty Slot
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            int slot = i;
            InventoryItem itemInSlot = inventorySlots[slot].GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                if (amount > _itemToSpawn.maxStack)
                {
                    int extra = amount - _itemToSpawn.maxStack;
                    Debug.Log("Extra: " + extra);

                    SpawnNewItem(itemId, _itemToSpawn.maxStack, slot);
                    AddItem(itemId, extra);
                    UpdateItemsInfoList();

                    OnItemRecieved.Invoke();
                    return true;
                }
                SpawnNewItem(itemId, amount, slot);
                OnItemRecieved?.Invoke();
                return true;
            }
        }

        UpdateItemsInfoList();

        DropItem(itemId, amount);
        return false;
    }

    public void SpawnNewItem(int itemID, int itemCount, int slotID)
    {
        GameObject newItemGO = Instantiate(_inventoryItemPrefab, inventorySlots[slotID].transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.count = itemCount;
        for (int i = 0; i < allItems.Length; i++)
        {
            if (allItems[i].itemID == itemID)
            {
                Debug.Log($"Id was found as {allItems[i].itemID}. Initializing Item {allItems[i]}");
                inventoryItem.InitializeItem(allItems[i], itemCount);
                break;
            }
        }

        UpdateItemsInfoList();
    }


    public void UseItem(int itemID, int itemCount)
    {
        int itemsLeft = itemCount;
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (itemsLeft == 0)
            {
                break;
            }
            if (itemsLeft <= 0)
            {
                Debug.LogError("Removed Too Many Items!");
                return;
            }
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            for (int z = 0; z < allItems.Length; z++)
            {
                if (allItems[z].itemID == itemID)
                {
                    if (itemInSlot != null && itemInSlot.item == allItems[z])
                    {
                        if (itemInSlot.count >= itemCount)
                        {
                            itemInSlot.count -= itemCount;
                            itemsLeft -= itemCount;
                            Debug.Log($"Used {itemCount} {allItems[z].name}");
                            itemInSlot.RefreshCount();
                            UpdateItemsInfoList();
                            break;
                        }
                        else
                        {
                            itemsLeft -= itemInSlot.count;
                            itemInSlot.count = 0;
                        }
                        Debug.Log($"Used {itemCount} {allItems[z].name}");
                        itemInSlot.RefreshCount();
                        UpdateItemsInfoList();
                    }
                    break;
                }
            }

        }
    }

    public void DropItem(int itemId, int amount, Transform spawnposition = null)
    {
        Item itemToDrop = GetItemById(itemId);
        GameObject droppedItem;

        if (spawnposition != null)
        {
            if (itemToDrop.itemPrefab != null)
            {
                droppedItem = Instantiate(itemToDrop.itemPrefab, spawnposition.position, Quaternion.identity);
                droppedItem.transform.GetComponent<DroppedItem>().item.Add(itemToDrop);
                droppedItem.transform.GetComponent<DroppedItem>().amount.Add(amount);
                Debug.Log($"Succesfully Dropped {amount} {itemToDrop}.");
            }
        }
        else
        {
            if (itemToDrop.itemPrefab != null)
            {
                droppedItem = Instantiate(itemToDrop.itemPrefab, player.transform.position, Quaternion.identity);
                droppedItem.transform.GetComponent<DroppedItem>().item.Add(itemToDrop);
                droppedItem.transform.GetComponent<DroppedItem>().amount.Add(amount);
                Debug.Log($"Succesfully Dropped {amount} {itemToDrop}.");
            }
        }
    }

    public void DropAllItems()
    {
        GameObject droppedItem = null;
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].GetInventoryItem())
            {
                if (droppedItem == null)
                {
                    droppedItem = Instantiate(_droppedBag, player.transform.position, Quaternion.identity);
                    droppedItem.GetComponent<DroppedItem>().item.Add(inventorySlots[i].GetInventoryItem().item);
                    droppedItem.GetComponent<DroppedItem>().amount.Add(inventorySlots[i].GetInventoryItem().count);
                }
                else
                {
                    droppedItem.GetComponent<DroppedItem>().item.Add(inventorySlots[i].GetInventoryItem().item);
                    droppedItem.GetComponent<DroppedItem>().amount.Add(inventorySlots[i].GetInventoryItem().count);
                }
            }
        }
    }

    public void UpdateItemsInfoList()
    {
        itemsInInventory.Clear();


        for (int i = 0; i < allItems.Length; i++)
        {
            int amount = GetTotalItemAmount(allItems[i]);
            itemsInInventory.Add(new(allItems[i], amount));
        }
    }

    public int GetTotalItemAmount(Item item)
    {
        int result = 0;

        foreach (var slot in inventorySlots)
        {
            if (slot.transform.childCount > 1)
            {
                InventoryItem thisItem = slot.GetComponentInChildren<InventoryItem>();

                // HashSet<Item> items = list.Select(o => thisItem.item).ToHashSet();
                if (thisItem != null)
                {
                    if (thisItem.item != item)
                    {
                        continue;
                    }

                    result += thisItem.count;
                }
            }
        }

        return result;
    }

    public bool HasSpace(Item itemToCheck, int amount)
    {
        //Check For Stackable Slot
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item == itemToCheck && itemInSlot.count + amount < itemInSlot.item.maxStack)
            {
                return true;
            }
        }
        //Check for empty Slot
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            int slot = i;
            InventoryItem itemInSlot = inventorySlots[slot].GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                return true;
            }
        }
        return false;
    }

    public Item GetItemById(int itemId)
    {
        for (int i = 0; i < allItems.Length; i++)
        {
            if (itemId == allItems[i].itemID)
            {
                return allItems[i];
            }
        }
        Debug.LogError("Could Not Find Item By Id " + itemId);
        return null;
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        var items = (Item[])Resources.FindObjectsOfTypeAll(typeof(Item));
        allItems = items;

        var itemsToId = new List<Item>();
        Debug.Log($"Items Found In Files: {items.Length}");

        foreach (var item in items)
        {
            if (!item.itemIdSet)
            {
                itemsToId.Add(item);
            }
        }

        foreach (var itemToId in itemsToId)
        {
            int highestId = 0;
            foreach (var item in items)
            {
                if (item.itemID > highestId)
                {
                    highestId = item.itemID;
                    Debug.Log($"Set highest id to {highestId}");
                }
            }
            itemToId.itemID = highestId + 1;
            itemToId.itemIdSet = true;
        }

        _slotParent = transform.GetChild(transform.childCount - 1);
        if (_slotParent.childCount != inventorySlots.Count)
        {
            inventorySlots.Clear();
            for (int i = 0; i < _slotParent.childCount; i++)
            {
                var slot = _slotParent.GetChild(i).GetComponent<InventorySlot>();
                inventorySlots.Add(slot);
                slot.slotId = i;
            }
        }
#endif
    }
}
