using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public TextAsset inkJSON;

    private bool alreadyCollected = false;

    public void PickUp()
    {
        // Mostra dialogo SEMPRE
        if (inkJSON != null)
        {
            InkManager.Instance.StartStory(inkJSON);
        }

        // Aggiunge all’inventario solo una volta
        if (!alreadyCollected)
        {
            Inventory.Instance.AddItem(itemName, itemIcon, inkJSON);
            alreadyCollected = true;

            if (itemName == "Letter")
            {
                GameState.Instance.hasReadLetter = true;
                Debug.Log("LETTERA LETTA = TRUE");
            }

            gameObject.SetActive(false);
        }
    }
}