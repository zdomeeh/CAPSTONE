using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Item item;

    private void Start()
    {
        item = GetComponent<Item>();
    }

    public virtual void Interact()
    {
        if (item != null)
        {
            item.PickUp();
        }
        else
        {
            Debug.Log("Interazione con " + gameObject.name);
        }
    }
}