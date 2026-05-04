using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemId;
    public string itemName;
    public Sprite itemIcon;
    public TextAsset inkJSON;
    public bool isReadable = false;

    private bool alreadyCollected = false;

    private void Start()
    {
        if (WorldState.Instance != null && WorldState.Instance.IsItemCollected(itemId))
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void PickUp()
    {
        if (alreadyCollected)
            return;

        if (inkJSON != null)
        {
            InkManager.Instance.StartStory(inkJSON);
        }

        InventoryItemData data = new InventoryItemData(
            itemName,
            itemIcon,
            inkJSON,
            isReadable
        );

        Inventory.Instance.AddItem(data);

        alreadyCollected = true;

        if (WorldState.Instance != null)
        {
            WorldState.Instance.MarkItemCollected(itemId);
        }

        if (itemName == "Letter")
        {
            GameState.Instance.SetLetterRead();
        }

        gameObject.SetActive(false);
    }
}