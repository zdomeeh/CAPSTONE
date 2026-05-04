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
        GameObject slot = Instantiate(itemSlotPrefab, inventoryPanel);

        Image img = slot.GetComponent<Image>();
        img.sprite = itemData.icon;

        Button btn = slot.GetComponent<Button>();

        btn.onClick.AddListener(() =>
        {
            InventorySelection.Instance.SelectItem(itemData.itemName);

            // SOLO se leggibile
            if (itemData.isReadable && itemData.inkJSON != null)
            {
                InkManager.Instance.StartStory(itemData.inkJSON);
            }
        });

        slots.Add(slot);
    }

    public void ClearUI()
    {
        foreach (GameObject slot in slots)
        {
            Destroy(slot);
        }

        slots.Clear();
    }
}