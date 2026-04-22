using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public InventoryUI inventoryUI;

    private List<string> items = new List<string>();

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
