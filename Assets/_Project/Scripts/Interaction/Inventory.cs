using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public InventoryUI inventoryUI;

    private List<InventoryItem> items = new List<InventoryItem>();

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

    public void AddItem(string itemName, Sprite sprite, TextAsset inkJSON)
    {
        InventoryItem newItem = new InventoryItem(itemName, sprite, inkJSON);
        items.Add(newItem);

        inventoryUI.AddItemToUI(newItem);

        Debug.Log("Aggiunto: " + itemName);
    }

    public bool HasItem(string itemName)
    {
        foreach (var item in items)
        {
            if (item.itemName == itemName)
                return true;
        }
        return false;
    }
}
