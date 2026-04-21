using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject itemSlotPrefab;
    public Transform inventoryPanel;

    private List<GameObject> slots = new List<GameObject>();

    public void AddItemToUI(string itemName, Sprite itemSprite)
    {
        GameObject slot = Instantiate(itemSlotPrefab, inventoryPanel);

        Image img = slot.GetComponent<Image>();
        img.sprite = itemSprite;

        Button btn = slot.GetComponent<Button>();

        btn.onClick.AddListener(() =>
        {
            InventorySelection.Instance.SelectItem(itemName);
        });
    }
}