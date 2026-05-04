using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public GameObject itemSlotPrefab;
    public Transform inventoryPanel;

    private List<GameObject> slots = new List<GameObject>();

    public void AddItemToUI(InventoryItemData itemData)
    {
        // Crea un nuovo slot nell'interfaccia
        GameObject slot = Instantiate(itemSlotPrefab, inventoryPanel);

        // Imposta l'immagine dell'oggetto
        Image img = slot.GetComponent<Image>();
        img.sprite = itemData.icon;

        Button btn = slot.GetComponent<Button>();

        // Aggiunge il comportamento al click
        btn.onClick.AddListener(() =>
        {
            // Seleziona l'oggetto nell'inventario
            InventorySelection.Instance.SelectItem(itemData.itemName);

            // Se l'oggetto e' leggibile, avvia il dialogo
            if (itemData.isReadable && itemData.inkJSON != null)
            {
                InkManager.Instance.StartStory(itemData.inkJSON);
            }
        });

        // Salva lo slot nella lista
        slots.Add(slot);
    }

    public void ClearUI()
    {
        // Distrugge tutti gli slot presenti
        foreach (GameObject slot in slots)
        {
            Destroy(slot);
        }

        // Svuota la lista
        slots.Clear();
    }
}