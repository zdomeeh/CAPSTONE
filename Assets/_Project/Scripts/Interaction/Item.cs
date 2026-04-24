using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;

    public TextAsset inkJSON; 

    public void PickUp()
    {
        Inventory.Instance.AddItem(itemName, itemIcon);

        if (inkJSON != null)
        {
            InkManager.Instance.StartStory(inkJSON);
        }

        if (itemName == "Clue")
        {
            Debug.Log("Fine del gioco!");
        }

        gameObject.SetActive(false);
    }
}