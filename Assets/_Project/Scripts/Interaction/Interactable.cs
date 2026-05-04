using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Item item;

    private void Start()
    {
        // Prende il componente Item se presente sull'oggetto
        item = GetComponent<Item>();
    }

    public virtual void Interact()
    {
        // Se l'oggetto e' un item, lo raccoglie
        if (item != null)
        {
            item.PickUp();
        }
        else
        {
            // Altrimenti stampa un messaggio di interazione
            Debug.Log("Interazione con " + gameObject.name);
        }
    }
}