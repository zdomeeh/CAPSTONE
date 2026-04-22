using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;

    public void PickUp()
    {
        Inventory.Instance.AddItem(itemName, itemIcon);

        if (itemName == "Clue")
        {
            Debug.Log("Fine del gioco!");
        }

        gameObject.SetActive(false);
    }
}
