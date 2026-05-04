using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public InventoryUI inventoryUI;

    private List<InventoryItemData> items = new List<InventoryItemData>();

    void Awake()
    {
        // Se non esiste ancora un'istanza, la imposta
        if (Instance == null)
        {
            Instance = this;

            // Mantiene l'inventario tra le scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Evita duplicati
            Destroy(gameObject);
        }
    }

    public void AddItem(InventoryItemData itemData)
    {
        // Aggiunge l'oggetto alla lista
        items.Add(itemData);

        // Aggiorna l'interfaccia grafica
        inventoryUI.AddItemToUI(itemData);
    }

    public bool HasItem(string itemName)
    {
        // Controlla se esiste un oggetto con quel nome
        return items.Exists(i => i.itemName == itemName);
    }

    public InventoryItemData GetItem(string itemName)
    {
        // Restituisce l'oggetto con quel nome
        return items.Find(i => i.itemName == itemName);
    }

    public void ResetInventory()
    {
        // Svuota la lista degli oggetti
        items.Clear();

        // Pulisce l'interfaccia grafica se esiste
        if (inventoryUI != null)
            inventoryUI.ClearUI();
    }
}