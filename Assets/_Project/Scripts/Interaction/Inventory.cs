using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public InventoryUI inventoryUI;

    private List<InventoryItemData> items = new List<InventoryItemData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(InventoryItemData itemData)
    {
        items.Add(itemData);
        inventoryUI.AddItemToUI(itemData);
    }

    public bool HasItem(string itemName)
    {
        return items.Exists(i => i.itemName == itemName);
    }

    public InventoryItemData GetItem(string itemName)
    {
        return items.Find(i => i.itemName == itemName);
    }
}