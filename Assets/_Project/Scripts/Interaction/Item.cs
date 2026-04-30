using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public TextAsset inkJSON;

    public bool isReadable = false;

    private bool alreadyCollected = false;

    public void PickUp()
    {
        if (inkJSON != null)
        {
            InkManager.Instance.StartStory(inkJSON);
        }

        if (!alreadyCollected)
        {
            InventoryItemData data = new InventoryItemData(
                itemName,
                itemIcon,
                inkJSON,
                isReadable
            );

            Inventory.Instance.AddItem(data);

            alreadyCollected = true;

            if (itemName == "Letter")
            {
                GameState.Instance.hasReadLetter = true;
            }
        }

        gameObject.SetActive(false);
    }
}