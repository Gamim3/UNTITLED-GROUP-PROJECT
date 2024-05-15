using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    public void AddItem(int itemID)
    {
        InventoryManager.Instance.AddItem(itemID, 1);
    }
}
