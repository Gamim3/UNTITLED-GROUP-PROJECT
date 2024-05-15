using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.VisualScripting;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public TMP_Text countText;

    public Item item;
    public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    public bool isDragging;

    public InventorySlot lastInventorySlot;

    InventoryManager inventoryManager;

    [SerializeField] bool dropOnDrop;

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
    }

    public void InitializeItem(Item newItem, int amount)
    {
        if (newItem == null)
        {
            Debug.LogError("No Item To Initialize!");
            return;
        }
        count = amount;
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
        GetComponentInParent<InventorySlot>().SetInventoryItem(this);
        lastInventorySlot = GetComponentInParent<InventorySlot>();
    }

    public void RefreshCount()
    {
        if (count == 0)
        {
            Destroy(gameObject);
        }
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InventorySlot slot = transform.GetComponentInParent<InventorySlot>();
        slot.SetInventoryItem(null);
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.position = new Vector3(transform.position.x, transform.position.y, -15);
        isDragging = true;
        InventoryManager.Instance.heldItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        InventoryManager.Instance.UpdateItemsInfoList();

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -15);
        InventoryManager.Instance.heldItem = eventData.pointerDrag.GetComponent<InventoryItem>();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dropOnDrop)
        {
            InventoryManager.Instance.DropItem(item.itemID, count);
            lastInventorySlot = null;

            Destroy(gameObject);
        }
        else if (parentAfterDrag.childCount == 0)
        {
            image.raycastTarget = true;
            transform.SetParent(parentAfterDrag);
            transform.position = parentAfterDrag.position;
            isDragging = false;
            InventoryManager.Instance.heldItem = null;
            GetComponentInParent<InventorySlot>().SetInventoryItem(this);
            InventoryManager.Instance.UpdateItemsInfoList();

            lastInventorySlot = GetComponentInParent<InventorySlot>();
        }
        else if (parentAfterDrag != null)
        {
            Debug.Log($"{parentAfterDrag.gameObject.name} already had a child!");
            if (parentAfterDrag.GetComponent<InventorySlot>().GetInventoryItem().count + count <= item.maxStack)
            {
                parentAfterDrag.GetComponent<InventorySlot>().GetInventoryItem().count += count;
            }
            else
            {
                int overflow = parentAfterDrag.GetComponent<InventorySlot>().GetInventoryItem().count + count - parentAfterDrag.GetComponent<InventorySlot>().GetInventoryItem().item.maxStack;
                parentAfterDrag.GetComponent<InventorySlot>().GetInventoryItem().count = item.maxStack;
                InventoryManager.Instance.AddItem(item.itemID, overflow);
            }
            parentAfterDrag.GetComponent<InventorySlot>().GetInventoryItem().RefreshCount();
            InventoryManager.Instance.UpdateItemsInfoList();

            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Had No ParentAfterDrag");
            InventoryManager.Instance.AddItem(item.itemID, count);
            Destroy(gameObject);
        }
    }

    public void SetItemParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    [Header("UI Raycasting")]
    [SerializeField] List<GraphicRaycaster> _raycaster = new();
    PointerEventData _pointerEventData;
    [SerializeField] EventSystem _eventSystem;

    private void Update()
    {
        if (isDragging)
        {
            if (!_raycaster.Any())
            {
                _raycaster = inventoryManager.graphicRaycasters;
            }
            if (_eventSystem == null)
            {
                _eventSystem = inventoryManager.eventSystem;
            }
            List<RaycastResult> results1 = new();
            _pointerEventData = new PointerEventData(_eventSystem);
            _pointerEventData.position = transform.position;

            for (int i = 0; i < _raycaster.Count; i++)
            {
                _raycaster[i].Raycast(_pointerEventData, results1);
            }

            if (results1.Count <= 0)
            {
                dropOnDrop = true;
                Debug.Log("DropOnDrop True");
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    InventoryManager.Instance.DropItem(item.itemID, 1);
                    count--;
                    RefreshCount();
                }
                Debug.Log("Results Count Was <= 0");
                return;
            }
            if (results1[0].gameObject.transform.GetComponent<Button>())
            {
                Debug.Log("DropOnDrop True");
                dropOnDrop = true;

                return;
            }
            dropOnDrop = false;
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Debug.Log($"Hit {results1[0].gameObject.name}");
                InventorySlot slot;
                results1[0].gameObject.transform.TryGetComponent<InventorySlot>(out slot);
                if (slot != null)
                {
                    slot.AddItemToSlot(this);
                    Debug.Log($"Dropped {item} In Slot {slot.name}");
                }
                else
                {
                    InventoryItem item;
                    results1[0].gameObject.transform.TryGetComponent<InventoryItem>(out item);
                    if (item != null)
                    {
                        if (item.count < item.item.maxStack)
                        {
                            item.GetComponentInParent<InventorySlot>().AddItemToSlot(this);
                        }
                        return;
                    }
                    Debug.Log("No Slot Found!");
                }

            }

        }
    }
}