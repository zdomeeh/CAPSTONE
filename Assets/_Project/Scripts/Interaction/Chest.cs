using UnityEngine;

public class Chest : Interactable
{
    public string requiredItem = "Tool";
    public GameObject hiddenObject;

    public override void Interact()
    {
        string selected = InventorySelection.Instance.selectedItem;

        if (selected == requiredItem)
        {
            Debug.Log("Cassetta aperta!");
            hiddenObject.SetActive(true);

            InventorySelection.Instance.ClearSelection();
        }
        else
        {
            Debug.Log("Non si apre...");
        }
    }
}