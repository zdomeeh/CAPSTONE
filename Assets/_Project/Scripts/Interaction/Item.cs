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
        // Se l'oggetto e' gia' stato raccolto in precedenza, lo nasconde
        if (WorldState.Instance != null && WorldState.Instance.IsItemCollected(itemId))
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void PickUp()
    {
        // Evita di raccogliere lo stesso oggetto piu' volte
        if (alreadyCollected)
            return;

        // Se esiste un dialogo associato, lo avvia
        if (inkJSON != null)
        {
            InkManager.Instance.StartStory(inkJSON);
        }

        // Crea i dati dell'oggetto per l'inventario
        InventoryItemData data = new InventoryItemData(
            itemName,
            itemIcon,
            inkJSON,
            isReadable
        );

        // Aggiunge l'oggetto all'inventario
        Inventory.Instance.AddItem(data);

        // Segna l'oggetto come gia' raccolto
        alreadyCollected = true;

        // Salva lo stato nel mondo di gioco
        if (WorldState.Instance != null)
        {
            WorldState.Instance.MarkItemCollected(itemId);
        }

        // Se l'oggetto e' una lettera, aggiorna lo stato del gioco
        if (itemName == "Letter")
        {
            GameState.Instance.SetLetterRead();
        }

        // Nasconde l'oggetto dalla scena
        gameObject.SetActive(false);
    }
}