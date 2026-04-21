using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public InventoryUI inventoryUI;

    private List<string> items = new List<string>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddItem(string item, Sprite sprite)
    {
        items.Add(item);
        inventoryUI.AddItemToUI(item, sprite);
        Debug.Log("Aggiunto: " + item);
    }

    public bool HasItem(string item)
    {
        return items.Contains(item);
    }
}
