using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject itemSlotPrefab;
    public Transform inventoryPanel;

    public void AddItemToUI(InventoryItem item)
    {
        GameObject slot = Instantiate(itemSlotPrefab, inventoryPanel);

        Image img = slot.GetComponent<Image>();
        img.sprite = item.icon;

        Button btn = slot.GetComponent<Button>();

        btn.onClick.AddListener(() =>
        {
            InventorySelection.Instance.SelectItem(item.itemName);

            if (item.inkJSON != null)
            {
                InkManager.Instance.StartStory(item.inkJSON);
            }
        });
    }
}